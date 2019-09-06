using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BudgetUnderControl.ViewModel
{
    public interface IAddTransactionViewModel
    {
        Task AddTransacionAsync();
        
        List<CategoryListItemDTO> Categories { get; }
        List<AccountListItemDTO> Accounts { get; }
        ICollection<string> Types { get; }
        int SelectedTypeIndex { get; set; }
        TimeSpan TransferTime { get; set; }
        DateTime TransferDate { get; set; }
        string TransferRate { get; set; }
        string TransferAmount { get; set; }
        int SelectedTransferAccountIndex { get; set; }
        int SelectedAccountIndex { get; set; }
        int SelectedCategoryIndex { get; set; }
        string Amount { get; set; }
        string Comment { get; set; }
        string Name { get; set; }
        DateTime Date { get; set; }
        TimeSpan Time { get; set; }
        TransactionType Type { get; }
        double? Longitude { get; set; }
        double? Latitude { get; set; }

        TagDTO SelectedTag { get; set; }
        ObservableCollection<TagDTO> Tags { get; set; }
        Task AddTagAsync(Guid tagId);
        Task RemoveTagFromListAsync(Guid tagId);

        bool IsItTransfer { get; }
        bool IsTransferOptionsVisible { get; }
        bool IsTransferInOtherCurrency { get; }
        bool IsValid { get; }
    }
}
