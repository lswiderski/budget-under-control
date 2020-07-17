using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public class CurrencyViewModel : ICurrencyViewModel, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        List<CurrencyDTO> currencies;
        public List<CurrencyDTO> Currencies => currencies;

        int selectedToCurrencyIndex;
        public int SelectedToCurrencyIndex
        {
            get
            {
                return selectedToCurrencyIndex;
            }
            set
            {
                if (selectedToCurrencyIndex != value)
                {
                    selectedToCurrencyIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedToCurrencyIndex)));
                }

            }
        }

        int selectedFromCurrencyIndex;
        public int SelectedFromCurrencyIndex
        {
            get
            {
                return selectedFromCurrencyIndex;
            }
            set
            {
                if (selectedFromCurrencyIndex != value)
                {
                    selectedFromCurrencyIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFromCurrencyIndex)));
                }

            }
        }

        private string rate;
        public string Rate
        {
            get => rate;
            set
            {
                if (rate != value)
                {
                    rate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rate)));
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
                }
            }
        }

        private ObservableCollection<ExchangeRateDTO> exchangeRates;
        public ObservableCollection<ExchangeRateDTO> ExchangeRates
        {
            get
            {
                return exchangeRates;
            }
            set
            {
                if (exchangeRates != value)
                {
                    exchangeRates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExchangeRates)));
                }

            }
        }

        private readonly ICurrencyService currencyService;
        private readonly ICommandDispatcher commandDispatcher;
        public CurrencyViewModel(ICurrencyService currencyService, ICommandDispatcher commandDispatcher)
        {
            this.currencyService = currencyService;
            this.commandDispatcher = commandDispatcher;
            Date = DateTime.Now;
        }

        public async Task LoadExchangeRatesAsync()
        {
            var dtos = await currencyService.GetExchangeRatesAsync();
            ExchangeRates = new ObservableCollection<ExchangeRateDTO>(dtos);
           
        }

        public async Task LoadCurrenciesAsync()
        {
            currencies = (await currencyService.GetCurriencesAsync()).OrderBy(x => x.Code).ToList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Currencies)));
        }

        public async Task AddExchangeRateAsync()
        {
            double value;
            double.TryParse(rate.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
            var command = new AddExchangeRate
            {
                Rate = value,
                Date = date,
                FromCurrencyId = Currencies[SelectedFromCurrencyIndex].Id,
                ToCurrencyId = Currencies[SelectedToCurrencyIndex].Id,
            };

            using (var scope = App.Container.BeginLifetimeScope())
            {
                await commandDispatcher.DispatchAsync(command, scope);
            }
        }
    }
}
