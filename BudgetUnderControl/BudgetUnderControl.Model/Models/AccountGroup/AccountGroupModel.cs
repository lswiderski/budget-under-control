using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AccountGroupModel : BaseModel, IAccountGroupModel
    {
        IContextConfig contextConfig;
        public AccountGroupModel(IContextConfig contextConfig) : base(contextConfig)
        {
            this.contextConfig = contextConfig;
        }
        

        public async Task<ICollection<AccountGroupItemDTO>> GetAccountGroups()
        {
            var list = (from ag in this.Context.AccountGroup
                       
                        select new AccountGroupItemDTO
                        {
                            Id = ag.Id,
                            Name = ag.Name,
                        }
                        ).ToListAsync();

            return await list;
        }
    }
}
