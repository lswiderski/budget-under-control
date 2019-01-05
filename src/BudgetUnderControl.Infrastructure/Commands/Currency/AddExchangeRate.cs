using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class AddExchangeRate : ICommand
    {
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public double Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
