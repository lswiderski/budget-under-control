using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Model.Services;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class AddTransactionCommand
    {
        public int AccountId { get; set; }
        public int? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public string Comment { get; set; }
        public ExtendedTransactionType Type { get; set; }

        public int TransferAccountId { get; set; }
        public DateTime TransferDate { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal Rate { get; set; }
    }

    public class AddTransactionValidator : AbstractValidator<AddTransactionCommand>
    {
        public AddTransactionValidator(ICategoryService categoryService, IAccountService accountService)
        {
            RuleFor(t => t.Amount).NotEmpty();
            RuleFor(t => t.Date).NotEmpty();
            RuleFor(t => t.Name).NotEmpty().Length(1, 100);
            RuleFor(t => t.Comment).Length(0, 1000);

            RuleFor(t => t.TransferAmount).NotEmpty().When(t => t.Type == ExtendedTransactionType.Transfer);
            RuleFor(t => t.TransferDate).NotEmpty().When(t => t.Type == ExtendedTransactionType.Transfer);
            RuleFor(t => t.Rate).NotEmpty().When(t => t.Type == ExtendedTransactionType.Transfer);

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

            RuleFor(t => t.TransferAccountId).NotEmpty().When(t => t.Type == ExtendedTransactionType.Transfer).CustomAsync(async (id, context, cancel) =>
            {
                if ((context.InstanceToValidate as AddTransactionCommand).Type == ExtendedTransactionType.Transfer)
                {
                    var isValid = await accountService.IsValidAsync(id);
                    if (!isValid)
                    {
                        context.AddFailure("This user do not own that Account");
                    }
                }
            });
        }

    }
}
