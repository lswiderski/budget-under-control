using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public interface ICurrencyViewModel
    {
        ObservableCollection<ExchangeRateDTO> ExchangeRates { get; set; }
        Task LoadExchangeRatesAsync();

        List<CurrencyDTO> Currencies { get;}
        Task AddExchangeRateAsync();
        Task LoadCurrenciesAsync();
    }
}
