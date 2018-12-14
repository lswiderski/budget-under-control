using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure;
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

        public async Task<User> GetAsync(string username)
        {
            var user = await this.Context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));

            return user;
        }
    }
}
