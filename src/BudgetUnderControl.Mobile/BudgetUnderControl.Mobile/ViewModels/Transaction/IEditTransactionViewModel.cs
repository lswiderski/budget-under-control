using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        TagDTO SelectedTag { get; set; }
        ObservableCollection<TagDTO> Tags { get; set; }
        Task AddTagAsync(Guid tagId);
        Task RemoveTagFromListAsync(Guid tagId);
    }
}
