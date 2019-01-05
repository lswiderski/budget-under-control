using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class ExchangeRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public double Rate { get; set; }
        public DateTime Date { get; set; }

        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }

        protected ExchangeRate()
        {

        }

        public static ExchangeRate Create(int fromCurrencyId, int toCurrencyId, double rate, DateTime? date = null)
        {
            return new ExchangeRate
            {
                FromCurrencyId = fromCurrencyId,
                ToCurrencyId = toCurrencyId,
                Rate = rate,
                Date = date ?? DateTime.UtcNow
            };
        }

        public void Edit(int fromCurrencyId, int toCurrencyId, double rate, DateTime date)
        {
            this.Rate = rate;
            this.Date = date;
            this.FromCurrencyId = fromCurrencyId;
            this.ToCurrencyId = toCurrencyId;
        }
    }
}
