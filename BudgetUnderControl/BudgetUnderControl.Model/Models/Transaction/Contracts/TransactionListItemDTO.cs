using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class TransactionListItemDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Account { get; set; }
        public TransactionType Type { get; set; }
        public decimal Value { get; set; }
        public string ValueWithCurrency { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
