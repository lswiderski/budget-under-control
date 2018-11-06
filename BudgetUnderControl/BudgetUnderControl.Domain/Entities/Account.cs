using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public AccountType Type { get; set; }
        public int? ParentAccountId { get; set; }
        public bool IsActive { get; set; }

        public AccountGroup AccountGroup { get; set; }
        public virtual Currency Currency { get; set; }
        public List<AccountSnapshot> AccountSnapshots { get; set; }
        public List<Transaction> Transactions { get; set; }

       
    }
}
