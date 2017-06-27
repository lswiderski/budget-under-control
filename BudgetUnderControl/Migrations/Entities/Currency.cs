using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        [StringLength(3)]
        public string Code { get; set; }
        [StringLength(250)]
        public string FullName { get; set; }
        public short Number { get; set; }
        [StringLength(3)]
        public string Symbol { get; set; }

        public List<Account> Accounts { get; set; }
        public List<ExchangeRate> FromExchangeRates { get; set; }
        public List<ExchangeRate> ToExchangeRates { get; set; }
    }
}
