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
        Task BalanceAdjustment(int accountId, decimal targetBalance);

        decimal GetIncome(int accountId, DateTime fromDate, DateTime toDate);
        decimal GetExpense(int accountId, DateTime fromDate, DateTime toDate);


        
    }
}
