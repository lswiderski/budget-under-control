using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class AddTransactionCommand
    {
        public int AccountId { get; set; }
        public int? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public string Comment { get; set; }
        public ExtendedTransactionType Type { get; set; }

        public int TransferAccountId { get; set; }
        public DateTime TransferDate { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal Rate { get; set; }
    }
}
