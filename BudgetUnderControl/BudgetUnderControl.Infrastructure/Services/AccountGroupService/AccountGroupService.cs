using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public class AccountGroupService : IAccountGroupService
    {

        private readonly IAccountGroupRepository accountGroupRepository;

        public AccountGroupService(IAccountGroupRepository accountGroupRepository)
        {
            this.accountGroupRepository = accountGroupRepository;
        }
        public async Task<ICollection<AccountGroupItemDTO>> GetAccountGroupsAsync()
        {
            var groups = await this.accountGroupRepository.GetAccountGroupsAsync();
            var list = (from ag in groups
                        select new AccountGroupItemDTO
                        {
                            Id = ag.Id,
                            Name = ag.Name,
                        }
                        ).ToList();

            return list;
        }
    }
}
