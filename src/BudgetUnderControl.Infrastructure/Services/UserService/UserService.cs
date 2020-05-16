using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IUserIdentityContext CreateUserIdentityContext()
        {
            var user =  this.userRepository.GetFirstUserAsync().Result;
            var context = new UserIdentityContext
            {
                UserId = user.Id,
                ExternalId = user.ExternalId,
                RoleName = user.Role
            };
            return context;
        }

        public long GetIdOf1stUser()
        {
            var user = this.userRepository.GetFirstUserAsync().Result;
            if(user != null)
            {
                return user.Id;
            }

            return 0;
        }
    }
}
