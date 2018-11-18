using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BudgetUnderControl.Infrastructure.Services
{
    public class SyncService : ISyncService
    {

        ITransactionRepository transactionRepository;
        IAccountRepository accountRepository;
        ICurrencyRepository currencyRepository;
        ICategoryRepository categoryRepository;
        IAccountGroupRepository accountGroupRepository;
        IUserRepository userRepository;

        private Dictionary<int, int> accountsMap; // key - old AccountId, value - new AccountId
        private Dictionary<int, int> transactionsMap; // key - old TransactionId, value - new TransactionId

        public SyncService(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICurrencyRepository currencyRepository,
            ICategoryRepository categoryRepository,
            IAccountGroupRepository accountGroupRepository,
            IUserRepository userRepository)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.currencyRepository = currencyRepository;
            this.categoryRepository = categoryRepository;
            this.accountGroupRepository = accountGroupRepository;
            this.userRepository = userRepository;
        }

        public async Task ImportBackUpAsync(BackUpDTO backupDto)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await CleanDataBaseAsync();
                //ImportCurrencies(backupDto.Currencies);
                await ImportAccountsAsync(backupDto.Accounts);
                await ImportTransactionsAsync(backupDto.Transactions);
                await ImportTransfersAsync(backupDto.Transfers);

                transaction.Complete();
            }
          
        }

        public async Task<BackUpDTO> GetBackUpAsync()
        {
            var currencies = (await this.currencyRepository.GetCurriencesAsync()).Select(x => new CurrencySyncDTO
            {
                Code = x.Code,
                FullName = x.FullName,
                Id = x.Id,
                Number = x.Number,
                Symbol = x.Symbol
            }).ToList();

            var accounts = (await this.accountRepository.GetAccountsAsync()).Select(x => new AccountSyncDTO
            {
                Id = x.Id,
                ExternalId = x.ExternalId,
                AccountGroupId = x.AccountGroupId,
                Comment = x.Comment,
                CurrencyId = x.CurrencyId,
                IsIncludedToTotal = x.IsIncludedToTotal,
                Name = x.Name,
                Order = x.Order,
                ParentAccountId = x.ParentAccountId,
                Type = x.Type,
            }).ToList();

            var transfers = (await this.transactionRepository.GetTransfersAsync()).Select(x => new TransferSyncDTO
            {
                Id = x.Id,
                FromTransactionId = x.FromTransactionId,
                Rate = x.Rate,
                ToTransactionId = x.ToTransactionId
            }).ToList();

            var transactions = (await this.transactionRepository.GetTransactionsAsync()).Select(x => new TransactionSyncDTO
            {
                Name = x.Name,
                Comment = x.Comment,
                AccountId = x.AccountId,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CreatedOn = x.CreatedOn,
                Date = x.Date,
                Id = x.Id,
                ExternalId = x.ExternalId,
                ModifiedOn = x.ModifiedOn,
                Type = x.Type
            }).ToList();

            var backUp = new BackUpDTO
            {
                Currencies= currencies,
                Accounts = accounts,
                Transfers = transfers,
                Transactions = transactions
            };

            return backUp;
        }

        public async Task<IEnumerable<string>> GenerateCSV()
        {
            var transactions = (await this.transactionRepository.GetTransactionsAsync())
               .Select(t => new {
                   AccountName = t.Account.Name,
                   CurrencyCode = t.Account.Currency.Code,
                   Amount = t.Amount,
                   TransactionId = t.Id,
                   Category = t.Category != null ? t.Category.Name : "",
                   TransactionName = t.Name,
                   Date = t.Date,
                   Type = t.Type,
                   Comment = t.Comment
               }).ToList();

            var lines = new List<string>();
            var firstLine = "TransactionId;Date;Time;Name;Amount;CurrencyCode;Category;Type;AccountName;Comment";
            var csv = firstLine + Environment.NewLine;
            lines.Add(firstLine);
            foreach (var item in transactions)
            {
                var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                    item.TransactionId, item.Date.ToLocalTime().ToString("dd/MM/yyyy"), item.Date.ToLocalTime().ToString("HH:mm"), item.TransactionName, item.Amount, item.CurrencyCode, item.Category, item.Type.ToString(), item.AccountName, item.Comment);
                lines.Add(line);
                csv += line + Environment.NewLine;
            }

            return lines;
        }

        private async Task CleanDataBaseAsync()
        {
            var transfers = await this.transactionRepository.GetTransfersAsync();
            await this.transactionRepository.RemoveTransfersAsync(transfers);

            var transactions = await this.transactionRepository.GetTransactionsAsync();
            await this.transactionRepository.RemoveTransactionsAsync(transactions);

            var accounts = await this.accountRepository.GetAccountsAsync();
            await this.accountRepository.RemoveAccountsAsync(accounts);

            //this.Context.Currencies.RemoveRange(this.Context.Currencies);
        }

        private async Task ImportAccountsAsync(List<AccountSyncDTO> accounts)
        {
            var user = await userRepository.GetFirstUserAsync();
            accountsMap = new Dictionary<int, int>();
            foreach (var item in accounts)
            {
                var account = Account.Create(item.Name, item.CurrencyId, item.AccountGroupId, item.IsIncludedToTotal, item.Comment, item.Order, item.Type, item.ParentAccountId, true, user.Id, item.ExternalId);
                await this.accountRepository.AddAccountAsync(account);

                accountsMap.Add(item.Id, account.Id);
            }

            //need to fix parentsAccountsIds
            await this.FixParentsAccountsIdsAsync();
        }

        private async Task FixParentsAccountsIdsAsync()
        {
            var accounts = (await this.accountRepository.GetAccountsAsync()).ToArray();

            for (int i = 0; i < accounts.Count(); i++)
            {
                if(accounts[i].ParentAccountId != null && accountsMap.ContainsKey(accounts[i].ParentAccountId.Value))
                {
                    accounts[i].SetParentAccountId(accountsMap[accounts[i].ParentAccountId.Value]);

                    await this.accountRepository.UpdateAsync(accounts[i]);

                }
            }
        }

        private async Task ImportTransactionsAsync(List<TransactionSyncDTO> transactions)
        {
            var user = await userRepository.GetFirstUserAsync();
            transactionsMap = new Dictionary<int, int>();
            foreach (var item in transactions)
            {
                var transaction = Domain.Transaction.Create(accountsMap[item.AccountId], item.Type, item.Amount, item.Date, item.Name, item.Comment, user.Id, item.CategoryId, item.ExternalId);
                transaction.SetCreatedOn(item.CreatedOn);
                transaction.SetModifiedOn(item.ModifiedOn);
                await this.transactionRepository.AddTransactionAsync(transaction);

                transactionsMap.Add(item.Id, transaction.Id);
            }
        }

        private async Task ImportTransfersAsync(List<TransferSyncDTO> transfers)
        {
            foreach (var item in transfers)
            {
                var transfer = Transfer.Create(transactionsMap[item.FromTransactionId], transactionsMap[item.ToTransactionId], item.Rate);
                await this.transactionRepository.AddTransferAsync(transfer);
            }
        }

        private async Task ImportCurrenciesAsync(List<CurrencySyncDTO> currencies)
        {
            foreach (var item in currencies)
            {
                var currency = Currency.Create(item.Code, item.FullName, item.Number, item.Symbol);
                currency.SetId(item.Id);
                await this.currencyRepository.AddCurrencyAsync(currency);
            }
        }
    }
}
