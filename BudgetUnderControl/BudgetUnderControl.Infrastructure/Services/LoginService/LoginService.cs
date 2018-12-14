using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository userRepository;
        private readonly IEncrypter encrypter;
        public LoginService(IUserRepository userRepository, IEncrypter encrypter)
        {
            this.userRepository = userRepository;
            this.encrypter = encrypter;
        }

        public async Task<bool> ValidateLoginAsync(MobileLoginCommand command)
        {
            var user = await userRepository.GetAsync(command.Username);

            if(user == null)
            {
                return false;
            }

            var hash = encrypter.GetHash(command.Password, user.Salt);

            if(hash != user.Password)
            {
                return false;
            }

            return true;
        }

        public async Task<Guid> GetUserId(string username)
        {
            var user = await userRepository.GetAsync(username);

            return user.ExternalId;
        }
    }
}
