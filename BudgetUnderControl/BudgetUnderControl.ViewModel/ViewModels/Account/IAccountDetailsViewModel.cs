using BudgetUnderControl.Model;
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
        string ValueWithCurrency { get; set; }
        decimal Value { get; set; }
        void LoadAccount(int accountId);
        void RemoveAccount();
        ICollection<TransactionListItemDTO> Transactions { get; set; }

        void LoadTransactions(int accountId);
    }
}
