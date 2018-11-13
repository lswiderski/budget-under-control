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
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
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
                    case ApplicationType.SQLiteMigrations:
                        return $"Filename={this.DbPath}";
                    case ApplicationType.SqlServerMigrations:
                    case ApplicationType.Web:
                        return $"Data Source=.;Initial Catalog={this.DbName};User ID={this.DbUser};Password={this.DbPassword}";                       
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
