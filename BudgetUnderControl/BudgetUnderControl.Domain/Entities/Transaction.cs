using System;
using BudgetUnderControl.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetUnderControl.Domain
{
    class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}
