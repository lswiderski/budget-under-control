using BudgetUnderControl.Domain;
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
    }
}
