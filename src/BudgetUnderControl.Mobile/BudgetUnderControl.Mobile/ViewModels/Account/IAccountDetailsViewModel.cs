using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
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
        Task LoadAccount(Guid accountId);
        Task RemoveAccount();
        ICollection<TransactionListItemDTO> Transactions { get; set; }
        TransactionListItemDTO SelectedTransaction { get; set; }

        Task LoadTransactions(Guid accountId);
        Task SetNextMonth();
        Task SetPreviousMonth();
    }
}
