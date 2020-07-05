using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BudgetUnderControl.MobileDomain
{
    public class Synchronization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ConcurrencyCheck]
        public DateTime LastSyncAt { get; set; }
        public int UserId { get; set; }
        public SynchronizationComponent Component { get; set; }
        public string ComponentId { get; set; }
        public virtual User User { get; set; }
    }
}
