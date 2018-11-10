using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Common.Contracts
{
    public class TransferSyncDTO
    {
        public int Id { get; set; }
        public int FromTransactionId { get; set; }
        public int ToTransactionId { get; set; }
        public decimal Rate { get; set; }
    }
}
