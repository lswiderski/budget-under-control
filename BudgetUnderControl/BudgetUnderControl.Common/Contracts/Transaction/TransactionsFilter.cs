using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class TransactionsFilter
    {
        public int? AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public TransactionsFilter()
        {
            this.ToDate = DateTime.UtcNow;
            this.FromDate = new DateTime();
        }
    }
}
