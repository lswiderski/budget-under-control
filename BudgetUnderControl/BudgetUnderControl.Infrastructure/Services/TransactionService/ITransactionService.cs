using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
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
        
        Task EditTransactionAsync(EditTransactionDTO transaction);
        Task RemoveTransactionAsync(int transactionId);
        Task <EditTransactionDTO> GetEditTransactionAsync(int id);

        Task AddTransactionAsync(AddTransaction command);
    }
}
