using BudgetUnderControl.Common.Enums;
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

        public ApplicationType Application { get; set; }

        public string ConnectionString { get
            {
                switch (this.Application)
                {
                    case ApplicationType.Mobile:
                    case ApplicationType.Migrations:
                        return $"Filename={this.DbPath}";
                    case ApplicationType.Web:
                        return $"Server=localhost;User Id=SA;Password=Abcd1234!;Database={this.DbName}";                       
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
