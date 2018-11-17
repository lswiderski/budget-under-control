using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IEditTransactionViewModel
    {
        Task EditTransactionAsync();
        Task GetTransactionAsync(Guid transactionId);
        Task DeleteTransactionAsync();
    }
}
