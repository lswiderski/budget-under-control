using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }
        public int FromTransactionId { get; set; }
        public int ToTransactionId { get; set; }
        public decimal Rate { get; set; }
    }
}
