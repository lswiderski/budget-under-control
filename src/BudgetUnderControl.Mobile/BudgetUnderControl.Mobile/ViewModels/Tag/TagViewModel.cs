using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public class TagViewModel : ITagViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ITagService tagService;

        ICollection<TagDTO> tags;
        public ICollection<TagDTO> Tags
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
                }

            }
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        private bool isDeleted;
        public bool IsDeleted
        {
            get => isDeleted;
            set
            {
                if (isDeleted != value)
                {
                    isDeleted = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDeleted)));
                }
            }
        }

        private ICommandDispatcher commandDispatcher { get; set; }
        private Guid externalId;
        public TagViewModel(ITagService tagService, ICommandDispatcher commandDispatcher)
        {
            this.tagService = tagService;
            this.commandDispatcher = commandDispatcher;
        }

        public async Task LoadTagsAsync()
        {
            Tags = await tagService.GetTagsAsync();
        }

        public async Task LoadActiveTagsAsync()
        {
            Tags = await tagService.GetActiveTagsAsync();
        }


        public async Task LoadTagAsync(Guid tagId)
        {
            externalId = tagId;
            SelectedTag = await tagService.GetTagAsync(tagId);
            Name = SelectedTag.Name;
            IsDeleted = SelectedTag.IsDeleted;
        }

        public async Task AddTagAsync()
        {

            var command = new AddTag
            {
                Name = Name,
            };

            using (var scope = App.Container.BeginLifetimeScope())
            {
                await commandDispatcher.DispatchAsync(command, scope);
            }
        }

        public async Task EditTagAsync()
        {
            var command = new EditTag
            {
                Name = Name,
                ExternalId = externalId,
                IsDeleted = IsDeleted
            };

            using (var scope = App.Container.BeginLifetimeScope())
            {
                await commandDispatcher.DispatchAsync(command, scope);
            }
        }
    }
}
