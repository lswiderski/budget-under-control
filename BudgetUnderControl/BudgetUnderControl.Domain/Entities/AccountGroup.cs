using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class AccountGroup : ISyncable
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public int OwnerId { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }
        public Guid ExternalId { get; protected set; }

        public virtual User Owner { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
