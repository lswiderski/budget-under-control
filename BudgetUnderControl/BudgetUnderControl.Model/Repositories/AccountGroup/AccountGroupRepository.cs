using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AccountGroupRepository : BaseModel, IAccountGroupRepository
    {
        public AccountGroupRepository(IContextFacade context) : base(context)
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
