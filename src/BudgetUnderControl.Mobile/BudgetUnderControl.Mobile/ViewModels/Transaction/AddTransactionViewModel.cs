using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;

using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Common.Extensions;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Essentials;

namespace BudgetUnderControl.ViewModel
{
    public class AddTransactionViewModel : IAddTransactionViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValid
        {
            get
            {
                if(IsItTransfer)
                {
                    if (IsTransferInOtherCurrency)
                    {
                        var valid = SelectedAccountIndex > -1
                         && SelectedTransferAccountIndex >-1
                         && !string.IsNullOrEmpty(Amount)
                         && !string.IsNullOrEmpty(TransferAmount)
                         && !string.IsNullOrEmpty(TransferAmount)
                         && decimal.TryParse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsed)
                         && decimal.TryParse(TransferAmount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out parsed)
                         && decimal.TryParse(TransferRate.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out parsed)
                         && !string.IsNullOrEmpty(Name)
                         && SelectedCategoryIndex > -1;
                        return valid;
                    }
                    else
                    {
                        var valid = SelectedAccountIndex > -1
                                                && SelectedTransferAccountIndex > -1
                                                && !string.IsNullOrEmpty(Amount)
                                                 && decimal.TryParse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsed)
                                                 && !string.IsNullOrEmpty(Name)
                                                 && SelectedCategoryIndex > -1;
                        return valid;
                    }

                }
                else
                {
                    var valid = SelectedAccountIndex > -1
                         && !string.IsNullOrEmpty(Amount)
                         && decimal.TryParse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsed)
                         && !string.IsNullOrEmpty(Name)
                         && SelectedCategoryIndex> -1;
                    return valid;

                }
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }

        public bool IsTransferInOtherCurrency
        {
            get
            {
                if (IsItTransfer)
                {
                    if (selectedAccountIndex > -1 
                        && SelectedTransferAccountIndex > -1
                        && Accounts[selectedAccountIndex].CurrencyId != Accounts[SelectedTransferAccountIndex].CurrencyId)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTransferInOtherCurrency)));
            }
        }

        public TransactionType Type
        {
            get
            {
                if(types.ElementAt(selectedTypeIndex) == "Income")
                {
                    return TransactionType.Income;
                }
                else if (types.ElementAt(selectedTypeIndex) == "Expense")
                {
                    return TransactionType.Expense;
                }
                else
                {
                    return TransactionType.Expense;
                }

            }
        }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }


        private double? longitude;
        public double? Longitude
        {
            get => longitude;
            set
            {
                if (longitude != value)
                {
                    longitude = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude)));
                }
            }
        }

        private double? latitude;

        public double? Latitude
        {
            get => latitude;
            set
            {
              
                if (latitude != value)
                {
                    latitude = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude)));
                }
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (comment != value)
                {
                    comment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
            }
        }

        private string amount;
        public string Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                    TransferAmount = Amount;
                }
            }
        }

        int selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get
            {
                return selectedCategoryIndex;
            }
            set
            {
                if (selectedCategoryIndex != value)
                {
                    selectedCategoryIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategoryIndex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }

            }
        }

        int selectedAccountIndex;
        public int SelectedAccountIndex
        {
            get
            {
                return selectedAccountIndex;
            }
            set
            {
                if (selectedAccountIndex != value)
                {
                    selectedAccountIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountIndex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTransferInOtherCurrency)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }

            }
        }

        int selectedTransferAccountIndex;
        public int SelectedTransferAccountIndex
        {
            get
            {
                return selectedTransferAccountIndex;
            }
            set
            {
                if (selectedTransferAccountIndex != value)
                {
                    selectedTransferAccountIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTransferAccountIndex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTransferInOtherCurrency)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }

            }
        }

        public bool IsTransferOptionsVisible
        {
            get
            {
                return IsItTransfer;
            }
        }

        public bool IsItTransfer
        {
            get
            {
                return types.ElementAt(selectedTypeIndex) == "Transfer";
            }
        }

        private string transferAmount;
        public string TransferAmount
        {
            get => transferAmount;
            set
            {
                if (transferAmount != value)
                {
                    transferAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransferAmount)));
                    decimal.TryParse(TransferAmount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsed);
                    if (!string.IsNullOrEmpty(Amount) && !string.IsNullOrEmpty(TransferAmount) && (parsed != 0))
                    {
                        decimal.TryParse(TransferAmount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal transferedParsed);
                        decimal.TryParse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal amountParsed);
                        transferRate = string.Format("{0}", amountParsed / transferedParsed);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransferRate)));
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
            }
        }

        private string transferRate;
        public string TransferRate
        {
            get => transferRate;
            set
            {
                if (transferRate != value)
                {
                    transferRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransferRate)));
                    if(!string.IsNullOrEmpty(Amount) && !string.IsNullOrEmpty(TransferRate))
                    {
                        transferAmount = string.Format("{0}",
                            decimal.Parse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) 
                            * decimal.Parse(TransferRate.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransferAmount)));
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
            }
        }

        private DateTime transferDate;
        public DateTime TransferDate
        {
            get => transferDate;
            set
            {
                if (transferDate != value)
                {
                    transferDate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransferDate)));
                }
            }
        }

        private TimeSpan transferTime;
        public TimeSpan TransferTime
        {
            get => transferTime;
            set
            {
                if (transferTime != value)
                {
                    transferTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransferTime)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
            }
        }


        ICollection<string> types = new List<string>
        {
           "Expense", "Income","Transfer"
        };

        public ICollection<string> Types => types;
        int selectedTypeIndex;
        public int SelectedTypeIndex 
        {
            get
            {
                return selectedTypeIndex;
            }
            set
            {
                if (selectedTypeIndex != value)
                {
                    selectedTypeIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTypeIndex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTransferOptionsVisible)));

                    if(types.ElementAt(value) == "Transfer")
                    {
                        SetTransfer();
                    }
                }

            }
        }

        ObservableCollection<TagDTO> tags;
        public ObservableCollection<TagDTO> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                if (tags != value)
                {
                    tags = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
                    TagListHeight = (tags.Count * 40) + (tags.Count * 10);
                }

            }
        }

        TagDTO selectedTag;
        public TagDTO SelectedTag
        {
            get
            {
                return selectedTag;
            }
            set
            {
                if (selectedTag != value)
                {
                    selectedTag = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTag)));
                    TagListHeight = (tags.Count * 40) + (tags.Count * 10);
                }

            }
        }

        private int tagListHeight;
        public int TagListHeight
        {
            get
            {
                return tagListHeight;
            }
            set
            {
                tagListHeight = value;
                OnPropertyChanged();
            }
        }


        List<AccountListItemDTO> accounts;
        public List<AccountListItemDTO> Accounts => accounts;

        List<CategoryListItemDTO> categories;
        public List<CategoryListItemDTO> Categories => categories;

        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    IAccountService accountService;
        ICategoryService categoryService;
        ICommandDispatcher commandDispatcher;
        ITagService tagService;
        public AddTransactionViewModel(IAccountService accountRepository, ICategoryService categoryService, ICommandDispatcher commandDispatcher, ITagService tagService)
        {
            this.accountService = accountRepository;
            this.categoryService = categoryService;
            this.commandDispatcher = commandDispatcher;
            this.tagService = tagService;
            SelectedTypeIndex = 0;
            SelectedCategoryIndex = -1;
            SelectedAccountIndex = -1;
            SelectedTransferAccountIndex = -1;
            Date = DateTime.Now;
            Time = DateTime.Now.TimeOfDay;
            Tags = new ObservableCollection<TagDTO>();
            GetDropdowns();
        }

        async void GetDropdowns()
        {
            accounts = (await accountService.GetAccountsWithBalanceAsync()).ToList();
            categories = (await categoryService.GetCategoriesAsync()).ToList();
        }

        void SetTransfer()
        {
            TransferDate = Date;
            TransferTime = Time;
            TransferRate = 1.ToString();
            TransferAmount = Amount;
        }

        public async Task AddTagAsync(Guid tagId)
        {
            var tag = await this.tagService.GetTagAsync(tagId);
            if (Tags.All(x => x.Id != tag.Id))
            {
                this.Tags.Add(tag);
                TagListHeight = (tags.Count * 40) + (tags.Count * 10);
            }
        }

        public async Task RemoveTagFromListAsync(Guid tagId)
        {
            this.Tags.Remove(this.Tags.FirstOrDefault(x => x.ExternalId == tagId));
            TagListHeight = (tags.Count * 40) + (tags.Count * 10);
        }

        public async Task AddTransacionAsync()
        {
            if(IsItTransfer)
            {
               await AddTransferAsync();
            }
            else
            {
                var date = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds, DateTimeKind.Local).ToUniversalTime();
                var amount = decimal.Parse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                if(Type == TransactionType.Expense && amount > 0)
                {
                    amount *= (-1);
                }
                else if(Type == TransactionType.Income && amount <0)
                {
                    amount *= (-1);
                }
                var transaction = new AddTransaction
                {
                    Name = Name,
                    Comment = Comment,
                    Date = date,
                    Amount = amount,
                    CategoryId = SelectedCategoryIndex >= 0  ? Categories[SelectedCategoryIndex].Id : (int?)null,
                    AccountId = Accounts[selectedAccountIndex].Id,
                    Type = Type.ToExtendedTransactionType(),
                    Tags = Tags.Select(x => x.Id).ToList(),
                    Longitude = Longitude,
                    Latitude = Latitude,
                };

                using (var scope = App.Container.BeginLifetimeScope())
                {
                    await commandDispatcher.DispatchAsync(transaction, scope);
                }
                
            }
        }

        private async Task AddTransferAsync()
        {
            var date = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds, DateTimeKind.Local).ToUniversalTime();
            var transferDate = new DateTime(TransferDate.Year, TransferDate.Month, TransferDate.Day, TransferTime.Hours, TransferTime.Minutes, TransferTime.Seconds, TransferTime.Milliseconds, DateTimeKind.Local).ToUniversalTime();

            
            var amount = decimal.Parse(Amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            if (amount > 0)
            {
                amount *= (-1);
            }

            var transferAmount = decimal.Parse(TransferAmount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            if (transferAmount < 0)
            {
                transferAmount *= (-1);
            }

            var addTransactionCommand = new AddTransaction
            {
                Name = Name,
                Comment = Comment,
                Date = date,
                Amount = amount,
                CategoryId = SelectedCategoryIndex > 0 ? Categories[SelectedCategoryIndex].Id : (int?)null,
                AccountId = Accounts[selectedAccountIndex].Id,
                TransferDate = transferDate,
                TransferAmount = transferAmount,
                Rate = decimal.Parse(TransferRate.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                TransferAccountId = Accounts[selectedTransferAccountIndex].Id,
                Type = ExtendedTransactionType.Transfer,
                Tags = Tags.Select(x => x.Id).ToList(),
                Longitude = Longitude,
                Latitude = Latitude,
            };

            using (var scope = App.Container.BeginLifetimeScope())
            {
                await commandDispatcher.DispatchAsync(addTransactionCommand, scope);
            }
        }

    }
}
