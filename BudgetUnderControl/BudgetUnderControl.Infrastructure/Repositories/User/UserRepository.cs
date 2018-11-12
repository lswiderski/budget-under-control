using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Repositories
{
    public class UserRepository : BaseModel, IUserRepository
    {
        public UserRepository(IContextFacade context) : base(context)
        {
        }

        public async Task<User> GetFirstUserAsync()
        {
            var user = await this.Context.Users.FirstOrDefaultAsync();

            return user;
        }
    }
}
