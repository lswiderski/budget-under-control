using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            this.currencyRepository = currencyRepository;
        }

        public async Task<ICollection<CurrencyDTO>> GetCurriencesAsync()
        {
            var currencies = await this.currencyRepository.GetCurriencesAsync();

            var dtos = currencies.Select(x => new CurrencyDTO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.FullName
            }).ToList();

            return dtos;
        }
    }
}
