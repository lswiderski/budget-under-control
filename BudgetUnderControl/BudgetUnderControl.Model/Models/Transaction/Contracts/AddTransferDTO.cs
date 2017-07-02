using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AddTransferDTO
    {
        public int AccountId { get; set; }
        public int? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public DateTime TransferDate { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal Rate { get; set; }
    }
}
