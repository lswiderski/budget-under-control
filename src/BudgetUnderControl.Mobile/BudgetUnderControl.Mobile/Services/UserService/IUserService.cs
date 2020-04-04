using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.Services
{
    public interface IUserService
    {
        IUserIdentityContext CreateUserIdentityContext();
    }
}
