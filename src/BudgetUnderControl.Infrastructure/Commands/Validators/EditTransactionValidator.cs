using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.ApiInfrastructure.Commands.Validators
{
    public class EditTransactionValidator : AbstractValidator<EditTransaction>
    {
        public EditTransactionValidator(ICategoryService categoryService, IAccountService accountService)
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.ExternalId).NotEmpty();
            RuleFor(t => t.Amount).NotEmpty();
            RuleFor(t => t.Date).NotEmpty();
            RuleFor(t => t.Name).NotEmpty().Length(1, 100);
            RuleFor(t => t.Comment).Length(0, 1000);

            RuleFor(t => t.TransferAmount).NotEmpty().When(t => t.ExtendedType == ExtendedTransactionType.Transfer);
            RuleFor(t => t.TransferDate).NotEmpty().When(t => t.ExtendedType == ExtendedTransactionType.Transfer);
            RuleFor(t => t.Rate).NotEmpty().When(t => t.ExtendedType == ExtendedTransactionType.Transfer);

            RuleFor(t => t.CategoryId).NotNull().CustomAsync(async (id, context, cancel) =>
            {
                if (id.HasValue)
                {
                    var isValid = await categoryService.IsValidAsync(id.Value);

                    if (!isValid)
                    {
                        context.AddFailure("This user do not own that Category");
                    }
                }


            });

            RuleFor(t => t.AccountId).NotEmpty().CustomAsync(async (id, context, cancel) =>
            {
                var isValid = await accountService.IsValidAsync(id);

                if (!isValid)
                {
                    context.AddFailure("This user do not own that Account");
                }
            });
            RuleFor(t => t.TransferTransactionId).NotEmpty().When(t => t.ExtendedType == ExtendedTransactionType.Transfer);
            RuleFor(t => t.TransferId).NotEmpty().When(t => t.ExtendedType == ExtendedTransactionType.Transfer);
            RuleFor(t => t.TransferAccountId).NotEmpty().When(t => t.ExtendedType == ExtendedTransactionType.Transfer).CustomAsync(async (id, context, cancel) =>
            {
                if ((context.InstanceToValidate as EditTransaction).ExtendedType == ExtendedTransactionType.Transfer && id.HasValue)
                {
                    var isValid = await accountService.IsValidAsync(id.Value);
                    if (!isValid)
                    {
                        context.AddFailure("This user do not own that Account");
                    }
                }
            });
        }

    }
}
