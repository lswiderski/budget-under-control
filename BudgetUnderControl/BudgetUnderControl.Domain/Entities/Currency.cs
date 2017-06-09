using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public short Number { get; set; }
    }
}
