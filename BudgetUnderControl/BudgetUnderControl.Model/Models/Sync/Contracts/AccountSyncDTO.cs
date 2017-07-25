using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AccountSyncDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int AccountGroupId { get; set; }
        public bool IsIncludedToTotal { get; set; }
        public string Comment { get; set; }
        public int Order { get; set; }
    }
}
