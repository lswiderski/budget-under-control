using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class AccountSnapshot
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? PreviousAccountSnapshotId { get; set; }
        public int LastTransactionId { get; set; }

        public Account Account { get; set; }
        public Transaction LastTransaction { get; set; }
        public AccountSnapshot PreviousAccountSnapshot { get; set; }
    }
}
