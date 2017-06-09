using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class TagToTransaction
    {
        public int TagId { get; set; }
        public int TransactionId { get; set; }
    }
}
