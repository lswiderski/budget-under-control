using System;
using BudgetUnderControl.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BudgetUnderControl.Domain
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public List<AccountSnapshot> AccountSnapshots { get; set; }
        public List<TagToTransaction> TagsToTransaction { get; set; }
        public Category Category { get; set; }
        public Account Account { get; set; }
        public List<Transfer> ToTransfers { get; set; }
        public List<Transfer> FromTransfers { get; set; }
    }
}
