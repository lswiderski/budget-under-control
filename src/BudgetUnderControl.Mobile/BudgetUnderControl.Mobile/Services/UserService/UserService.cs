using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.MobileDomain.Repositiories;

namespace BudgetUnderControl.Mobile.Services
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
    }
}
