using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public class SyncService : ISyncService
    {
        private static string BACKUP_FILE_NAME = "buc_backup.json";

        IFileHelper fileHelper;
        ITransactionRepository transactionRepository;
        IAccountRepository accountRepository;
        ICurrencyRepository currencyRepository;
        ICategoryRepository categoryRepository;
        IAccountGroupRepository accountGroupRepository;
        IUserRepository userRepository;

        public SyncService(IFileHelper fileHelper,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICurrencyRepository currencyRepository,
            ICategoryRepository categoryRepository,
            IAccountGroupRepository accountGroupRepository,
            IUserRepository userRepository
            )
        {
            this.fileHelper = fileHelper;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.currencyRepository = currencyRepository;
            this.categoryRepository = categoryRepository;
            this.accountGroupRepository = accountGroupRepository;
            this.userRepository = userRepository;
        }

        public async Task<string> GetTransactionsJSONAsync()
        {
            var transactions = await this.transactionRepository.GetTransactionsAsync();

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(transactions);
            return output;
        }

        public async Task<string> GetTransfersJSON()
        {
            var transfers = await this.transactionRepository.GetTransfersAsync();

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(transfers);
            return output;
        }

        public async Task<string>  GetAccountsJSON()
        {
            var accounts = await this.accountRepository.GetAccountsAsync();

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(accounts);
            return output;
        }

        public async Task<string> GetCurrenciesJSON()
        {
            var currencies = await this.currencyRepository.GetCurriencesAsync();

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(currencies);
            return output;
        }

        public async Task<string> GetBackUpJSONAsync()
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
                ModifiedOn = x.ModifiedOn,
                Type = x.Type
            }).ToList();

            var backUp = new
            {
                currencies,
                accounts,
                transfers,
                transactions
            };

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(backUp);
            return output;

        }


        public async Task ImportBackUpJSONAsync(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            try
            {
                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<BackUpDTO>(json);
                await CleanDataBaseAsync();
                //ImportCurrencies(deserialized.Currencies);
                await ImportAccountsAsync(deserialized.Accounts);
                await ImportTransactionsAsync(deserialized.Transactions);
                await ImportTransfersAsync(deserialized.Transfers);
            }
            catch (Exception e)
            {
                //Problem with JSON file
                throw;
            }

        }

        public async Task SaveBackupFileAsync()
        {
            var json = await GetBackUpJSONAsync();

            fileHelper.SaveText(BACKUP_FILE_NAME, json);
        }

        public async Task LoadBackupFileAsync()
        {
            var json = fileHelper.LoadText(BACKUP_FILE_NAME);
            await ImportBackUpJSONAsync(json);
        }

        public async Task ExportCSVAsync()
        {
            var fileName = string.Format("{0}_{1}.txt", "buc", DateTime.UtcNow.Ticks);
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
            fileHelper.SaveText(fileName, lines.ToArray());
            //fileHelper.SaveTextExternal(fileName, csv);
        }

        public async Task ExportDBAsync()
        {
            //temporary
            ExtractDB();
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

        private async Task ImportAccountsAsync(List<AccountSyncDTO> accounts)
        {
            var user = await userRepository.GetFirstUserAsync();
            foreach (var item in accounts)
            {
                var account = Account.Create(item.Name, item.CurrencyId, item.AccountGroupId, item.IsIncludedToTotal, item.Comment, item.Order, item.Type, item.ParentAccountId, true, user.Id);
                account.SetId(item.Id);
                await this.accountRepository.AddAccountAsync(account);
            }
        }

        private async Task ImportTransactionsAsync(List<TransactionSyncDTO> transactions)
        {
            var user = await userRepository.GetFirstUserAsync();
            foreach (var item in transactions)
            {
                var transaction = Transaction.Create(item.AccountId, item.Type, item.Amount, item.Date, item.Name, item.Comment, user.Id, item.CategoryId);
                transaction.SetId(item.Id);
                transaction.SetCreatedOn(item.CreatedOn);
                transaction.SetModifiedOn(item.ModifiedOn);
                await this.transactionRepository.AddTransactionAsync(transaction);
            }
        }

        private async Task ImportTransfersAsync(List<TransferSyncDTO> transfers)
        {
            foreach (var item in transfers)
            {
                var transfer = Transfer.Create(item.FromTransactionId, item.ToTransactionId, item.Rate);
                transfer.SetId(item.Id);
                await this.transactionRepository.AddTransferAsync(transfer);
            }
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

        public void ExtractDB()
        {
            var sourcePath = fileHelper.GetLocalFilePath(Settings.DB_SQLite_NAME);

            var databaseBackupPath = fileHelper.GetExternalFilePath(string.Format("{0}_{1}.db3", "buc_Backup", DateTime.UtcNow.Ticks));
            fileHelper.CopyFile(sourcePath, databaseBackupPath);

        }
    }
}
