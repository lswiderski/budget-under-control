using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.CommonInfrastructure.Commands;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository userRepository;
        private readonly IEncrypter encrypter;
        private readonly IJwtHandlerService jwtHandlerService;
        private readonly IMemoryCache cache;
        public LoginService(IUserRepository userRepository, IEncrypter encrypter, 
            IJwtHandlerService jwtHandlerService, IMemoryCache cache)
        {
            this.userRepository = userRepository;
            this.encrypter = encrypter;
            this.jwtHandlerService = jwtHandlerService;
            this.cache = cache;
        }

        public async Task ValidateLoginAsync(MobileLoginCommand command)
        {
            var user = await userRepository.GetAsync(command.Username);

            if(user == null)
            {
                return;
            }

            var hash = encrypter.GetHash(command.Password, user.Salt);

            if(hash != user.Password)
            {
                return;
            }

            var token = jwtHandlerService.CreateToken(user.ExternalId);
            cache.Set(command.TokenId, token);
        }

    }
}
