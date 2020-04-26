using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain.Repositiories
{
    public interface ITransactionRepository
    {
        Task<ICollection<Transaction>> GetTransactionsAsync(TransactionsFilter filter = null);
        Task<ICollection<Transfer>> GetTransfersAsync();
        Task<Transaction> GetTransactionAsync(int id);
        Task<Transaction> GetTransactionAsync(string id);
        Task AddTransactionAsync(Transaction transaction);
        Task AddTransactionsAsync(IEnumerable<Transaction> transactions);
        Task UpdateAsync(Transaction transaction);
        Task UpdateAsync(IEnumerable<Transaction> transactions);
        Task AddTransferAsync(Transfer transfer);
        Task AddTransfersAsync(IEnumerable<Transfer> transfers);
        Task UpdateTransferAsync(Transfer transfer);
        Task RemoveTransactionAsync(Transaction transaction);
        Task RemoveTransferAsync(Transfer transfer);
        Task<Transfer> GetTransferAsync(int transactionId);
        Task<Transfer> GetTransferAsync(string transactionId);
        Task HardRemoveTransactionsAsync(IEnumerable<Transaction> transactions);
        Task HardRemoveTransfersAsync(IEnumerable<Transfer> transfers);
        Task<ICollection<Transfer>> GetTransfersModifiedSinceAsync(DateTime changedSince);
    }
}
