using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class ContextConfig : IContextConfig
    {
        private string _dbPath;

        public string DbName { get; set; }
        public string DbPath { get
            {   
                return _dbPath;  
            }
        set
            {
                _dbPath = value;
            }
        }
    }
}
