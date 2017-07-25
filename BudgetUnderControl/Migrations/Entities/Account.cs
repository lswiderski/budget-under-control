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
        [StringLength(250)]
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int AccountGroupId { get; set; }
        public bool IsIncludedToTotal { get; set; }
        public string Comment { get; set; }
        public int Order { get; set; }

        public AccountGroup AccountGroup { get; set; }
        public Currency Currency { get; set; }
        public List<AccountSnapshot> AccountSnapshots { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
