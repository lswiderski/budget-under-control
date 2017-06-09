using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int AccountGroupId { get; set; }
        public bool IsIncludedToTotal { get; set; }
        public string Comment { get; set; }
    }
}
