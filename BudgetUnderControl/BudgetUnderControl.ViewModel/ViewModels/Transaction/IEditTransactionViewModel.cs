using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IEditTransactionViewModel
    {
        void EditTransaction();
        void GetTransaction(int transactionId);
        void DeleteTransaction();
    }
}
