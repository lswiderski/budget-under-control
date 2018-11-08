using BudgetUnderControl.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<Account> GetAccountAsync(int id);
        Task UpdateAsync(Account account);
        Task<decimal> GetActualBalanceAsync(int accountId);
        Task AddAccountAsync(Account account);
        Task BalanceAdjustmentAsync(int accountId, decimal targetBalance);
        Task<List<int>> GetSubAccountsAsync(int accountId);
        Task<decimal> GetExpenseAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<decimal> GetIncomeAsync(int accountId, DateTime fromDate, DateTime toDate);


        
    }
}
