using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts
{
    public class DashboardDTO
    {
        public List<TransactionListItemDTO> Transactions { get; set; }

        public List<CurrencyStatusDTO> ActualStatus { get; set; }

        public List<CategoryShareDTO> ThisMonthCategoryChart { get; set; }

        public List<CategoryShareDTO> LastMonthCategoryChart { get; set; }

        public decimal Total { get; set; }

        public Dictionary<DateTime, decimal> Incomes { get; set; }

        public Dictionary<DateTime, decimal> Expenses { get; set; }

        public DashboardDTO()
        {
            this.Transactions = new List<TransactionListItemDTO>();
            this.Incomes = new Dictionary<DateTime, decimal>();
            this.Expenses = new Dictionary<DateTime, decimal>();
            this.ThisMonthCategoryChart = new List<CategoryShareDTO>();
            this.LastMonthCategoryChart = new List<CategoryShareDTO>();
            this.ActualStatus = new List<CurrencyStatusDTO>();
        }
    }
}
