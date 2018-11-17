using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Services;
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
                Name = x.FullName,
                Number = x.Number,
                Symbol = x.Symbol,
            }).ToList();

            return dtos;
        }
        public async Task<CurrencyDTO> GetCurrencyAsync(int id)
        {
            var currency = await this.currencyRepository.GetCurrencyAsync(id);

            var dto = new CurrencyDTO
            {
                Id = currency.Id,
                Code = currency.Code,
                Name = currency.FullName,
                Number = currency.Number,
                Symbol = currency.Symbol,
            };

            return dto;
        }

        public async Task<bool> IsValidAsync(int currencyId)
        {
            var currencies = await this.currencyRepository.GetCurriencesAsync();
            return currencies.Any(x => x.Id == currencyId);
        }
    }
}
