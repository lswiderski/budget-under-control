using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IAccountDetailsViewModel
    {
        string Name { get; set; }
        void LoadAccount(int accountId);
        void RemoveAccount();
    }
}
