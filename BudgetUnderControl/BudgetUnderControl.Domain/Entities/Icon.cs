using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class Icon
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
    }
}
