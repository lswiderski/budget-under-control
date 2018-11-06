using BudgetUnderControl.Common;
using BudgetUnderControl.Contracts.Models;
using BudgetUnderControl.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class SyncService : BaseModel, ISyncService
    {
        private static string BACKUP_FILE_NAME = "buc_backup.json";

        IFileHelper fileHelper;
        public SyncService( IFileHelper fileHelper, IContextFacade context) : base(context)
        {
            this.fileHelper = fileHelper;
        }

        public string GetTransactionsJSON()
        {
            var transactions = this.Context.Transactions;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(transactions);
            return output;
        }

        public string GetTransfersJSON()
        {
            var transfers = this.Context.Transfers;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(transfers);
            return output;
        }

        public string GetAccountsJSON()
        {
            var accounts = this.Context.Accounts;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(accounts);
            return output;
        }

        public string GetCurrenciesJSON()
        {
            var currencies = this.Context.Transactions;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(currencies);
            return output;
        }

        public string GetBackUpJSON()
        {
            var currencies = this.Context.Currencies.Select(x => new CurrencySyncDTO
            {
                Code = x.Code,
                FullName = x.FullName,
                Id = x.Id,
                Number = x.Number,
                Symbol = x.Symbol
            }).ToList();

            var accounts = this.Context.Accounts.Select(x => new AccountSyncDTO
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

            var transfers = this.Context.Transfers.Select(x => new TransferSyncDTO
            {
                Id = x.Id,
                FromTransactionId = x.FromTransactionId,
                Rate = x.Rate,
                ToTransactionId = x.ToTransactionId
            }).ToList();

            var transactions = this.Context.Transactions.Select(x => new TransactionSyncDTO
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


        public void ImportBackUpJSON(string json)
        {
            if(string.IsNullOrEmpty(json))
            {
                return;
            }
            try
            {
                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<BackUpDTO>(json);
               CleanDataBase();
               //ImportCurrencies(deserialized.Currencies);
               ImportAccounts(deserialized.Accounts);
               ImportTransactions(deserialized.Transactions);
               ImportTransfers(deserialized.Transfers);
            }
            catch (Exception e)
            {
                //Problem with JSON file
                throw;
            }
            
        }

        public void SaveBackupFile()
        {
            var json = GetBackUpJSON();

            fileHelper.SaveText(BACKUP_FILE_NAME, json);
        }

        public void LoadBackupFile()
        {
           var json = fileHelper.LoadText(BACKUP_FILE_NAME);
            ImportBackUpJSON(json);
        }

        public void ExportCSV()
        {
            var fileName = string.Format("{0}_{1}.txt", "buc", DateTime.UtcNow.Ticks);

            var query = (from tr in this.Context.Transactions
                         join acc in this.Context.Accounts on tr.AccountId equals acc.Id
                         join cur in this.Context.Currencies on acc.CurrencyId equals cur.Id
                         from cat in this.Context.Categories.Where(x => x.Id == tr.CategoryId).DefaultIfEmpty()
                         select new
                         {
                             AccountName = acc.Name,
                             CurrencyCode = cur.Code,
                             Amount = tr.Amount,
                             TransactionId = tr.Id,
                             Category = cat != null ? cat.Name : "",
                             TransactionName = tr.Name,
                             
                             Date = tr.Date,
                             Type = tr.Type,
                             Comment = tr.Comment
                         }).ToList();

            var lines = new List<string>();
            var firstLine = "TransactionId;Date;Time;Name;Amount;CurrencyCode;Category;Type;AccountName;Comment";
            var csv = firstLine + Environment.NewLine;
            lines.Add(firstLine);
            foreach (var item in query)
            {
                var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                    item.TransactionId, item.Date.ToLocalTime().ToString("dd/MM/yyyy"), item.Date.ToLocalTime().ToString("HH:mm"), item.TransactionName,  item.Amount, item.CurrencyCode, item.Category,  item.Type.ToString(), item.AccountName,  item.Comment);
                lines.Add(line);
                csv += line + Environment.NewLine;
            }
            fileHelper.SaveText(fileName, lines.ToArray());
            //fileHelper.SaveTextExternal(fileName, csv);
        }

        private void ImportCurrencies(List<CurrencySyncDTO> currencies)
        {
            foreach (var item in currencies)
            {
                var currency = new Currency
                {
                    Code = item.Code,
                    FullName = item.FullName,
                    Id = item.Id,
                    Number = item.Number,
                    Symbol = item.Symbol,
                };
                this.Context.Currencies.Add(currency);
            }

            this.Context.SaveChanges();
        }

        private void ImportAccounts(List<AccountSyncDTO> accounts)
        {
            foreach (var item in accounts)
            {
                var account = new Account
                {
                    AccountGroupId = item.AccountGroupId,
                    Comment = item.Comment,
                    CurrencyId = item.CurrencyId,
                    Id = item.Id,
                    IsIncludedToTotal = item.IsIncludedToTotal,
                    Name = item.Name,
                    Order = item.Order,
                    Type = item.Type,
                    ParentAccountId = item.ParentAccountId

                };
                this.Context.Accounts.Add(account);
            }

            this.Context.SaveChanges();
        }

        private void ImportTransactions(List<TransactionSyncDTO> transactions)
        {
            foreach (var item in transactions)
            {
                var transaction = new Transaction
                {
                    AccountId = item.AccountId,
                    Amount = item.Amount,
                    CategoryId = item.CategoryId,
                    Comment = item.Comment,
                    CreatedOn = item.CreatedOn,
                    Date = item.Date,
                    Id = item.Id,
                    ModifiedOn = item.ModifiedOn,
                    Name = item.Name,
                    Type = item.Type,

                };
                this.Context.Transactions.Add(transaction);
            }
        }

        private void ImportTransfers(List<TransferSyncDTO> transfers)
        {
            foreach (var item in transfers)
            {
                var transfer = new Transfer
                {
                    FromTransactionId = item.FromTransactionId,
                    Id = item.Id,
                    Rate = item.Rate,
                    ToTransactionId = item.ToTransactionId
                };
                this.Context.Transfers.Add(transfer);
            }
            this.Context.SaveChanges();
        }

        private void CleanDataBase()
        {
            this.Context.Transfers.RemoveRange(this.Context.Transfers);
            this.Context.SaveChanges();
            this.Context.Transactions.RemoveRange(this.Context.Transactions);
            this.Context.SaveChanges();
            this.Context.Accounts.RemoveRange(this.Context.Accounts);
            this.Context.SaveChanges();
            //this.Context.Currencies.RemoveRange(this.Context.Currencies);

            this.Context.SaveChanges();
        }
    }
}
