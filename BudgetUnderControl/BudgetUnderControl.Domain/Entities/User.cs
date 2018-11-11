using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BudgetUnderControl.Domain
{
    public class User : ISyncable
    {
        [Key]
        public int Id { get; protected set; }
        [StringLength(50)]
        public string Username { get; protected set; }
        public string Role { get; protected set; }
        [StringLength(150)]
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public Guid ExternalId { get; protected set; }

        public List<Account> Accounts { get; protected set; }
        public List<AccountGroup> AccountGroups { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
    }
}
