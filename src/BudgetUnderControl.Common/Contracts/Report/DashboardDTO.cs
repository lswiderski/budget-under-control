using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class DashboardDTO
    {
        public List<TransactionListItemDTO> Transactions { get; set; }

        public Dictionary<string, decimal> ActualStatus { get; set; }

        public decimal Total { get; set; }

        public DashboardDTO()
        {
            this.Transactions = new List<TransactionListItemDTO>();
        }
    }
}
