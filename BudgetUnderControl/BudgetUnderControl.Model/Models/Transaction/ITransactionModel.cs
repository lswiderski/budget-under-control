using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public interface ITransactionModel
    {
        void AddTransaction(AddTransactionDTO arg);
        void AddTransfer(AddTransferDTO args);
        Task<ICollection<TransactionListItemDTO>> GetTransactions();
        Task<ICollection<TransactionListItemDTO>> GetTransactions(int accountId);
        Task<ICollection<TransactionListItemDTO>> GetTransactions(DateTime fromDate, DateTime toDate);
        Task<ICollection<TransactionListItemDTO>> GetTransactions(int accountId, DateTime fromDate, DateTime toDate);
        EditTransactionDTO GetEditTransaction(int id);
        void EditTransaction(EditTransactionDTO arg);
        void DeleteTransaction(int id);
        List<int> GetSubAccounts(int accountId);
    }
}
