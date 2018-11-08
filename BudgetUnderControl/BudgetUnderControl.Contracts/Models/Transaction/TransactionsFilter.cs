using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Contracts.Models
{
    public class TransactionsFilter
    {
        public int? AccountId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
