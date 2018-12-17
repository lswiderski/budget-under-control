using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface ICurrencyRepository
    {
        Task<ICollection<Currency>> GetCurriencesAsync();
        Task AddCurrencyAsync(Currency currency);
        Task<Currency> GetCurrencyAsync(int id);
    }
}
