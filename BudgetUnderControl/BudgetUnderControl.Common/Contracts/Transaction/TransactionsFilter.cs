using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class TransactionsFilter
    {
        public IEnumerable<int> AccountsIds { get; set; }
        public IEnumerable<Guid> AccountsExternalIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IncludeDeleted { get; set; }

        public TransactionsFilter()
        {
            this.ToDate = DateTime.UtcNow;
            this.FromDate = new DateTime();
        }
    }
}
