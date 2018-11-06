using BudgetUnderControl.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface IAccountService
    {
        Task ActivateAccountAsync(int id);
        Task<EditAccountDTO> GetEditAccountDTOAsync(int id);
    }
}
