using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface ITransactionService
    {
        Task<ICollection<TransactionListItemDTO>> GetTransactionsAsync(TransactionsFilter filter = null);
        Task<ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>>> GetGroupedTransactionsAsync(TransactionsFilter filter = null);
        Task AddTransactionAsync(AddTransactionDTO transaction);
        Task EditTransactionAsync(EditTransactionDTO transaction);
        Task RemoveTransactionAsync(int transactionId);
        Task <EditTransactionDTO> GetEditTransactionAsync(int id);
        Task AddTransferAsync(AddTransferDTO arg);
    }
}
