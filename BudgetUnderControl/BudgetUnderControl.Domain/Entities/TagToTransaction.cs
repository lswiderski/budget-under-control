using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class TagToTransaction
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; }
        public int TransactionId { get; set; }

        public Tag Tag { get; set; }
        public Transaction Transaction { get; set; }
    }
}
