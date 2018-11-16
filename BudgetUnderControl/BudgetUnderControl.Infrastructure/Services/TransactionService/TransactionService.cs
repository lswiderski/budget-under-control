using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Services;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Common.Extensions;
using FluentValidation;

namespace BudgetUnderControl.Model.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IUserRepository userRepository;
        private readonly IValidator<AddTransaction> addTransactionValidator;

        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, IValidator<AddTransaction> addTransactionValidator)
        {
            this.transactionRepository = transactionRepository;
            this.userRepository = userRepository;
            this.addTransactionValidator = addTransactionValidator;
        }

        public async Task<ICollection<TransactionListItemDTO>> GetTransactionsAsync(TransactionsFilter filter = null)
        {
            var transactions = await this.transactionRepository.GetTransactionsAsync(filter);
            var dtos = transactions.Select(t => new TransactionListItemDTO
            {
                AccountId = t.AccountId,
                Date = t.Date,
                Id = t.Id,
                Value = t.Amount,
                Account = t.Account.Name,
                ValueWithCurrency = t.Amount + t.Account.Currency.Symbol,
                Type = t.Type,
                Name = t.Name,
                CurrencyCode = t.Account.Currency.Code,
                IsTransfer = t.IsTransfer
            }).ToList();

            return dtos;
        }

        public async Task<ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>>> GetGroupedTransactionsAsync(TransactionsFilter filter = null)
        {
            var transactions = await this.transactionRepository.GetTransactionsAsync(filter);

            var dtos = transactions.Select(t => new TransactionListItemDTO
            {
                AccountId = t.AccountId,
                Date = t.Date,
                Id = t.Id,
                Value = t.Amount,
                Account = t.Account.Name,
                ValueWithCurrency = t.Amount + t.Account.Currency.Symbol,
                Type = t.Type,
                Name = t.Name,
                CurrencyCode = t.Account.Currency.Code,
                IsTransfer = t.IsTransfer,
                ExternalId = t.ExternalId.ToString(),
            }).OrderByDescending(x => x.Date)
                                .GroupBy(x => x.Date.ToString("d MMM yyyy"))
                                .Select(x => new ObservableGroupCollection<string, TransactionListItemDTO>(x))
                                .ToList();

            return new ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>>(dtos);
        }

        public async Task AddTransactionAsync(AddTransaction command)
        {
            var results = addTransactionValidator.Validate(command);
            if(!results.IsValid)
            {
                throw new ArgumentException(results.ToString("~"));
            }

            var user = await userRepository.GetFirstUserAsync();

            if(command.Type == ExtendedTransactionType.Transfer)
            {
                if (command.Amount > 0)
                {
                    command.Amount *= (-1);
                }
                if (command.TransferAmount < 0)
                {
                    command.TransferAmount *= (-1);
                }
                var transactionExpense = Transaction.Create(command.AccountId, TransactionType.Expense, command.Amount, command.Date, command.Name, command.Comment, user.Id, command.CategoryId);
                await transactionRepository.AddTransactionAsync(transactionExpense);

                var transactionIncome = Transaction.Create(command.TransferAccountId, TransactionType.Income, command.TransferAmount, command.TransferDate, command.Name, command.Comment, user.Id, command.CategoryId);
                await transactionRepository.AddTransactionAsync(transactionIncome);

                var transfer = Transfer.Create(transactionExpense.Id, transactionIncome.Id, command.Rate);
                await transactionRepository.AddTransferAsync(transfer);
            }
            else
            {
                var type = command.Type.ToTransactionType();
                if (type == TransactionType.Expense && command.Amount > 0)
                {
                    command.Amount *= (-1);
                }
                else if (type == TransactionType.Income && command.Amount < 0)
                {
                    command.Amount *= (-1);
                }
                var transaction = Transaction.Create(command.AccountId, type, command.Amount, command.Date, command.Name, command.Comment, user.Id, command.CategoryId);

                await this.transactionRepository.AddTransactionAsync(transaction);
            }
        }

        public async Task EditTransactionAsync(EditTransactionDTO arg)
        {
            var firstTransaction = await this.transactionRepository.GetTransactionAsync(arg.Id);
            Transaction secondTransaction = null;

            await transactionRepository.UpdateAsync(firstTransaction);

            var transfer = await transactionRepository.GetTransferAsync(arg.Id); 

            if (transfer != null)
            {
                var secondTRansactionId = transfer.ToTransactionId != arg.Id ? transfer.ToTransactionId : transfer.FromTransactionId;
                secondTransaction = await this.transactionRepository.GetTransactionAsync(transfer.ToTransactionId);
            }

            //remove transfer, no more transfer
            if (arg.ExtendedType != ExtendedTransactionType.Transfer
                && transfer != null && secondTransaction != null)
            {
                await this.transactionRepository.RemoveTransferAsync(transfer);
                await this.transactionRepository.RemoveTransactionAsync(secondTransaction);
                firstTransaction.Edit(arg.AccountId, arg.Type, arg.Amount, arg.Date, arg.Name, arg.Comment, arg.CategoryId);
                await this.transactionRepository.UpdateAsync(firstTransaction);
            }
            //new Transfer, no transfer before
            else if (arg.ExtendedType == ExtendedTransactionType.Transfer
                && transfer == null && secondTransaction == null)
            {
                firstTransaction.Edit(arg.AccountId, TransactionType.Expense, arg.Amount, arg.Date, arg.Name, arg.Comment, arg.CategoryId);
                await this.transactionRepository.UpdateAsync(firstTransaction);

                var user = await userRepository.GetFirstUserAsync();

                var transactionIncome = Transaction.Create(arg.TransferAccountId.Value, TransactionType.Income, arg.TransferAmount.Value, arg.TransferDate.Value, arg.Name, arg.Comment, user.Id, arg.CategoryId);
                await transactionRepository.AddTransactionAsync(transactionIncome);

                var newTransfer = Transfer.Create(firstTransaction.Id, transactionIncome.Id, arg.Rate.Value);
                await transactionRepository.AddTransferAsync(newTransfer);

            }

            //edit transfer
            else if (arg.ExtendedType == Common.Enums.ExtendedTransactionType.Transfer
                && transfer != null && secondTransaction != null)
            {
                firstTransaction.Edit(arg.AccountId, TransactionType.Expense, arg.Amount, arg.Date, arg.Name, arg.Comment, arg.CategoryId);
                await this.transactionRepository.UpdateAsync(firstTransaction);

                secondTransaction.Edit(arg.TransferAccountId.Value, TransactionType.Income, arg.TransferAmount.Value, arg.TransferDate.Value, arg.Name, arg.Comment, arg.CategoryId);
                await this.transactionRepository.UpdateAsync(firstTransaction);

                transfer.SetRate(arg.Rate.Value);
                await transactionRepository.UpdateTransferAsync(transfer);
            }
            //just edit 1 transaction, no transfer before
            else if (arg.ExtendedType != Common.Enums.ExtendedTransactionType.Transfer
                && transfer == null && secondTransaction == null)
            {
                decimal amount = 0;

                if (arg.Type == TransactionType.Expense && arg.Amount > 0)
                {
                    amount = arg.Amount * (-1);
                }
                else if (arg.Type == TransactionType.Income && arg.Amount < 0)
                {
                    amount = arg.Amount * (-1);
                }
                else
                {
                    amount = arg.Amount;
                }

                firstTransaction.Edit(arg.AccountId, arg.Type, amount, arg.Date, arg.Name, arg.Comment, arg.CategoryId);
                await this.transactionRepository.UpdateAsync(firstTransaction);
            }
        }

        public async Task RemoveTransactionAsync(int transactionId)
        {
            var firstTransaction = await this.transactionRepository.GetTransactionAsync(transactionId);
            Transaction secondTransaction = null;
            var transfer = await this.transactionRepository.GetTransferAsync(transactionId);

            if (transfer != null)
            {
                var secondTRansactionId = transfer.ToTransactionId != transactionId ? transfer.ToTransactionId : transfer.FromTransactionId;
                secondTransaction = await this.transactionRepository.GetTransactionAsync(secondTRansactionId);
                await transactionRepository.RemoveTransferAsync(transfer);
                await transactionRepository.RemoveTransactionAsync(secondTransaction);
            }
            await transactionRepository.RemoveTransactionAsync(firstTransaction);
        }

        public async Task<EditTransactionDTO> GetEditTransactionAsync(int id)
        {
            var t = await this.transactionRepository.GetTransactionAsync(id);

            var transaction = new EditTransactionDTO
            {
                AccountId = t.AccountId,
                Amount = t.Amount,
                CategoryId = t.CategoryId,
                Comment = t.Comment,
                Date = t.Date,
                Id = t.Id,
                Name = t.Name,
                Type = t.Type,
            };

            transaction.ExtendedType = transaction.Type == TransactionType.Income ? ExtendedTransactionType.Income : ExtendedTransactionType.Expense;

            var transfer = await this.transactionRepository.GetTransferAsync(transaction.Id);

            if (transfer != null)
            {
                int transferedTransactionId;
                if (transaction.Id == transfer.FromTransactionId)
                {
                    transferedTransactionId = transfer.ToTransactionId;
                }
                else
                {
                    transferedTransactionId = transfer.FromTransactionId;
                }
                var transferedTransaction = await this.transactionRepository.GetTransactionAsync(transferedTransactionId);

                transaction.Rate = transfer.Rate;
                transaction.TransferId = transfer.Id;

                if (transaction.Id == transfer.FromTransactionId)
                {
                    transaction.TransferAmount = transferedTransaction.Amount;
                    transaction.TransferDate = transferedTransaction.Date;
                    transaction.TransferTransactionId = transferedTransaction.Id;
                    transaction.TransferAccountId = transferedTransaction.AccountId;

                }
                else
                {
                    transaction.TransferAmount = transaction.Amount;
                    transaction.Amount = transferedTransaction.Amount;

                    transaction.TransferDate = transaction.Date;
                    transaction.Date = transferedTransaction.Date;

                    transaction.TransferTransactionId = transaction.Id;
                    transaction.Id = transferedTransaction.Id;


                    transaction.TransferAccountId = transaction.AccountId;
                    transaction.AccountId = transferedTransaction.AccountId;
                }

                transaction.ExtendedType = ExtendedTransactionType.Transfer;

            }

            return transaction;
        }
    }
}
