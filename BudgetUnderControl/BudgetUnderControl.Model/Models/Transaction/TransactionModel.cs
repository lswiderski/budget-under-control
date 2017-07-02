using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class TransactionModel : BaseModel, ITransactionModel
    {
        IContextConfig contextConfig;
        public TransactionModel(IContextConfig contextConfig) : base(contextConfig)
        {
            this.contextConfig = contextConfig;
        }

        public void AddTransaction(AddTransactionDTO arg)
        {
            var transaction = new Transaction
            {
                 AccountId = arg.AccountId,
                 Amount = arg.Amount,
                 CategoryId = arg.CategoryId,
                 Comment = arg.Comment,
                 Name = arg.Name,
                 Type = arg.Type,
                 CreatedOn = arg.CreatedOn,    
            };

            this.Context.Transactions.Add(transaction);
            this.Context.SaveChanges();
        }

        public async Task<ICollection<TransactionListItemDTO>> GetTransactions()
        {
            var transactions = (from t in this.Context.Transactions
                                join a in this.Context.Accounts on t.AccountId equals a.Id
                                join c in this.Context.Currencies on a.CurrencyId equals c.Id
                                orderby t.CreatedOn descending
                                select new TransactionListItemDTO
                                {
                                    AccountId = t.AccountId,
                                    Date = t.CreatedOn,
                                    Id = t.Id,
                                    Value = t.Amount,
                                    Account = a.Name,
                                    ValueWithCurrency = t.Amount + c.Symbol,
                                    Type = t.Type,
                                    Name = t.Name,
                                }
                                ).ToListAsync();

            return await transactions;
        }

        public async Task<ICollection<TransactionListItemDTO>> GetTransactions(int accountId)
        {
            var transactions = (from t in this.Context.Transactions
                                join a in this.Context.Accounts on t.AccountId equals a.Id
                                join c in this.Context.Currencies on a.CurrencyId equals c.Id
                                where a.Id == accountId
                                orderby t.CreatedOn descending
                                select new TransactionListItemDTO
                                {
                                    AccountId = t.AccountId,
                                    Date = t.CreatedOn,
                                    Id = t.Id,
                                    Value = t.Amount,
                                    Account = a.Name,
                                    ValueWithCurrency = t.Amount + c.Symbol,
                                    Type = t.Type,
                                    Name = t.Name,
                                }
                                ).ToListAsync();

            return await transactions;
        }

        public void AddTransfer(AddTransferDTO arg)
        {
            var transactionExpense = new Transaction
            {
                AccountId = arg.AccountId,
                Amount = arg.Amount,
                CategoryId = arg.CategoryId,
                Comment = arg.Comment,
                Name = arg.Name,
                Type =  Common.Enums.TransactionType.Expense,
                CreatedOn = arg.Date,
            };

            var transactionIncome = new Transaction
            {
                AccountId = arg.AccountId,
                Amount = arg.TransferAmount,
                CategoryId = arg.CategoryId,
                Comment = arg.Comment,
                Name = arg.Name,
                Type = Common.Enums.TransactionType.Income,
                CreatedOn = arg.TransferDate,
            };

            this.Context.Transactions.Add(transactionExpense);
            this.Context.Transactions.Add(transactionIncome);
            this.Context.SaveChanges();

            var transfer = new Transfer
            {
                FromTransactionId = transactionExpense.Id,
                ToTransactionId = transactionIncome.Id,
                Rate = arg.Rate
            };
            this.Context.Transefres.Add(transfer);
            this.Context.SaveChanges();
        }
    }
}
