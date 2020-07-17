using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository currencyRepository;
        private readonly IUserIdentityContext userIdentityContext;

        public CurrencyService(ICurrencyRepository currencyRepository, IUserIdentityContext userIdentityContext)
        {
            this.currencyRepository = currencyRepository;
            this.userIdentityContext = userIdentityContext;
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

        public async Task<CurrencyDTO> GetCurrencyAsync(string code)
        {
            var currency = await this.currencyRepository.GetCurrencyAsync(code);

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

        public async Task<IEnumerable<ExchangeRateDTO>> GetExchangeRatesAsync()
        {
            var rates = (await this.currencyRepository.GetExchangeRatesAsync())
                .Select(x => new ExchangeRateDTO
                {
                    Id = x.Id,
                    Rate = x.Rate,
                    Date = x.Date,
                    FromCurrencyId = x.FromCurrencyId,
                    ToCurrencyId = x.ToCurrencyId,
                    FromCurrencyCode = x.FromCurrency.Code,
                    ToCurrencyCode = x.ToCurrency.Code,
                    CanDelete = x.UserId == this.userIdentityContext.UserId,
                })
                .ToList();

            return rates;
        }

        public async Task AddExchangeRateAsync(AddExchangeRate command)
        {
            var rate = ExchangeRate.Create(command.FromCurrencyId, command.ToCurrencyId, command.Rate, userIdentityContext.UserId, command.ExternalId, false, command.Date);

            await this.currencyRepository.AddExchangeRateAsync(rate);
        }

        public async Task<decimal> TransformAmountAsync(decimal amount, int fromCurrencyId, int toCurrencyId)
        {
            var exchangeRate = await this.currencyRepository.GetLatestExchangeRateAsync(fromCurrencyId, toCurrencyId);

            var result = await this.GetValueInDifferentCurrency(amount, fromCurrencyId, toCurrencyId, exchangeRate);

            return result;
        }

        private async Task<decimal> GetValueInDifferentCurrency(decimal amount, int fromCurrencyId, int toCurrencyId, ExchangeRate exchangeRate)
        {
            var result = amount;
            if (exchangeRate == null)
            {
                result = amount;
            }
            else if (exchangeRate.FromCurrencyId == fromCurrencyId)
            {
                result = amount * (decimal)exchangeRate.Rate;
            }
            else if (exchangeRate.ToCurrencyId == fromCurrencyId)
            {
                result = amount / ((decimal)exchangeRate.Rate != 0 ? (decimal)exchangeRate.Rate : 1);
            }

            return result;
        }

        public async Task<decimal> TransformAmountAsync(decimal amount, string fromCurrencyCode, string toCurrencyCode)
        {
            var fromCurrency = await this.currencyRepository.GetCurrencyAsync(fromCurrencyCode);
            var toCurrency = await this.currencyRepository.GetCurrencyAsync(toCurrencyCode);
            return await this.TransformAmountAsync(amount, fromCurrency.Id, toCurrency.Id);
        }
    }
}
