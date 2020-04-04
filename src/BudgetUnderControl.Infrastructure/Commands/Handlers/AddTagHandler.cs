using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.CommonInfrastructure.Commands
{
    public class AddTagHandler : ICommandHandler<AddTag>
    {
        private readonly ITagService tagService;
        public AddTagHandler(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public async Task HandleAsync(AddTag command)
        {
            await this.tagService.AddTagAsync(command);

        }
    }
}