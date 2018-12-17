using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Infrastructure.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
   public class AddAccount : ICommand
    {
        public int CurrencyId { get; set; }
        public int AccountGroupId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public string Comment { get; set; }
        public AccountType Type { get; set; }
        public int? ParentAccountId { get; set; }
        public int Order { get; set; }
        public Guid ExternalId { get; set; }

        public AddAccount()
        {
            this.ExternalId = Guid.NewGuid();
        }
    }

    public class AddAccountValidator : AbstractValidator<AddAccount>
    {
        public AddAccountValidator(ICurrencyService currencyService, IAccountGroupService accountGroupService , IAccountService accountService)
        {
            RuleFor(t => t.AccountGroupId).NotEmpty();
            RuleFor(t => t.Name).NotEmpty().Length(1, 100);

            RuleFor(t => t.CurrencyId).NotEmpty().CustomAsync(async (id, context, cancel) =>
            {
                    var isValid = await currencyService.IsValidAsync(id);
                    if (!isValid)
                    {
                        context.AddFailure("That Currency is not Exist");
                    }
            });
            RuleFor(t => t.ParentAccountId).CustomAsync(async (id, context, cancel) =>
            {
                if (id.HasValue)
                {
                    var isValid = await accountService.IsValidAsync(id.Value);
                    if (!isValid)
                    {
                        context.AddFailure("This user do not own that Account");
                    }
                }
            });

            RuleFor(t => t.AccountGroupId).NotEmpty().CustomAsync(async (id, context, cancel) =>
            {
                    var isValid = await accountGroupService.IsValidAsync(id);
                    if (!isValid)
                    {
                        context.AddFailure("This user do not own that Account Group");
                    }
                
            });
        }
    }
}
