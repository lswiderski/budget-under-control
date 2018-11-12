using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface IUserRepository
    {
        Task<User> GetFirstUserAsync();
    }
}
