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
        public AccountGroupModel(IContextFacade context) : base(context)
        {
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
