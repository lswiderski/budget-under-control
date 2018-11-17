using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface ICurrencyService
    {
        Task<ICollection<CurrencyDTO>> GetCurriencesAsync();
        Task<CurrencyDTO> GetCurrencyAsync(int id);
        Task<bool> IsValidAsync(int currencyId);
    }
}
