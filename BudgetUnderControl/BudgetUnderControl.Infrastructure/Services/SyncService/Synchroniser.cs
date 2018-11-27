using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services
{
    public class Synchroniser : ISynchroniser
    {

        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ICurrencyRepository currencyRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAccountGroupRepository accountGroupRepository;
        private readonly IUserRepository userRepository;
        private readonly ISynchronizationRepository synchronizationRepository;
        private readonly IUserIdentityContext userIdentityContext;
        private readonly GeneralSettings settings;

        public Synchroniser(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICurrencyRepository currencyRepository,
            ICategoryRepository categoryRepository,
            IAccountGroupRepository accountGroupRepository,
            IUserRepository userRepository,
            ISynchronizationRepository synchronizationRepository,
            IUserIdentityContext userIdentityContext,
            GeneralSettings settings)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.currencyRepository = currencyRepository;
            this.categoryRepository = categoryRepository;
            this.accountGroupRepository = accountGroupRepository;
            this.userRepository = userRepository;
            this.synchronizationRepository = synchronizationRepository;
            this.userIdentityContext = userIdentityContext;
            this.settings = settings;
        }

        public async Task SynchroniseAsync(SyncRequest syncRequest)
        {
            await this.UpdateLastSyncDateAsync(syncRequest);
            await this.UpdateUsersAsync(syncRequest.Users);
            await this.UpdateTagsAsync(syncRequest.Tags);
            await this.UpdateCategoriesAsync(syncRequest.Categories);
            await this.UpdateAccountGroupsAsync(syncRequest.AccountGroups);
            await this.UpdateAccountsAsync(syncRequest.Accounts);
            await this.UpdateTransactionsAsync(syncRequest.Transactions);
            await this.UpdateTransfersAsync(syncRequest.Transfers);
        }

        private async Task UpdateLastSyncDateAsync(SyncRequest syncRequest)
        {
            var userId = (await this.userRepository.GetFirstUserAsync()).Id;
            var syncObject = await this.synchronizationRepository.GetSynchronizationAsync(syncRequest.Component, syncRequest.ComponentId, userId);// syncRequest.UserId)

            if(syncObject != null)
            {
                syncObject.LastSyncAt = DateTime.UtcNow;
                await this.synchronizationRepository.UpdateAsync(syncObject);
            }
            else
            {
                syncObject = new Synchronization
                {
                    LastSyncAt = DateTime.UtcNow,
                    Component = syncRequest.Component,
                    ComponentId = syncRequest.ComponentId,
                    UserId = userId,
                };

                await this.synchronizationRepository.AddSynchronizationAsync(syncObject);
            }
        }

        private async Task UpdateTransactionsAsync(IEnumerable<TransactionSyncDTO> transactions)
        {
            if (transactions == null || !transactions.Any())
            {
                return;
            }

            foreach (var transaction in transactions)
            {

                int? categoryId = transaction.CategoryExternalId.HasValue ? (await this.categoryRepository.GetCategoryAsync(transaction.CategoryExternalId.Value)).Id : (int?)null;
                var accountId = (await this.accountRepository.GetAccountAsync(transaction.AccountExternalId.Value)).Id;
                var transactionToUpdate = await this.transactionRepository.GetTransactionAsync(transaction.ExternalId.Value);
                if (transactionToUpdate != null)
                {
                    transactionToUpdate.Edit(accountId, transaction.Type, transaction.Amount, transaction.Date, transaction.Name, transaction.Comment, this.userIdentityContext.UserId, transaction.IsDeleted, categoryId);
                    transactionToUpdate.SetCreatedOn(transaction.CreatedOn);
                    transactionToUpdate.SetModifiedOn(transaction.ModifiedOn);
                    await this.transactionRepository.UpdateAsync(transactionToUpdate);
                }
                else
                {
                    var transactionToAdd = Transaction.Create(accountId, transaction.Type, transaction.Amount, transaction.Date, transaction.Name, transaction.Comment, this.userIdentityContext.UserId, false, categoryId, transaction.ExternalId);
                    transactionToAdd.SetCreatedOn(transaction.CreatedOn);
                    transactionToAdd.SetModifiedOn(transaction.ModifiedOn);
                    await this.transactionRepository.AddTransactionAsync(transactionToAdd);
                }
            }
        }

        private async Task UpdateTransfersAsync(IEnumerable<TransferSyncDTO> transfers)
        {
            if (transfers == null || !transfers.Any())
            {
                return;
            }

            foreach (var transfer in transfers)
            {
                var transferToUpdate = await this.transactionRepository.GetTransferAsync(transfer.ExternalId.Value);
                if (transferToUpdate != null)
                {
                    transferToUpdate.SetRate(transfer.Rate);
                    transferToUpdate.Delete(transfer.IsDeleted);
                    transferToUpdate.SetModifiedOn(transfer.ModifiedOn);
                    
                    
                    await this.transactionRepository.UpdateTransferAsync(transferToUpdate);
                }
                else
                {
                    var toTransferId = (await this.transactionRepository.GetTransactionAsync(transfer.ToTransactionExternalId)).Id;
                    var fromTransferId = (await this.transactionRepository.GetTransactionAsync(transfer.FromTransactionExternalId)).Id;
                    var transferToAdd = Transfer.Create(toTransferId, fromTransferId, transfer.Rate, transfer.ExternalId);
                    transferToAdd.Delete(transfer.IsDeleted);
                    transferToAdd.SetModifiedOn(transfer.ModifiedOn);
                    await this.transactionRepository.AddTransferAsync(transferToAdd);
                }
            }
        }

        private async Task UpdateUsersAsync(IEnumerable<UserSyncDTO> users)
        {
            if (users == null || !users.Any())
            {
                return;
            }

            foreach (var user in users)
            {
               //TODO
            }
        }

        private async Task UpdateTagsAsync(IEnumerable<TagSyncDTO> tags)
        {
            if (tags == null || !tags.Any())
            {
                return;
            }

            foreach (var tag in tags)
            {
                //TODO
            }
        }

        private async Task UpdateCategoriesAsync(IEnumerable<CategorySyncDTO> categories)
        {
            if (categories == null || !categories.Any())
            {
                return;
            }

            foreach (var category in categories)
            {
                var userId = (await this.userRepository.GetFirstUserAsync()).Id;
                var categoryToUpdate = await this.categoryRepository.GetCategoryAsync(category.ExternalId);
                if (categoryToUpdate != null)
                {
                    categoryToUpdate.Edit(category.Name, userId);
                    categoryToUpdate.Delete(category.IsDeleted);
                    categoryToUpdate.SetModifiedOn(category.ModifiedOn);
                    await this.categoryRepository.UpdateAsync(categoryToUpdate);
                }
                else
                {
                    var categoryToAdd = Category.Create(category.Name, userId, category.ExternalId);
                    categoryToAdd.Delete(category.IsDeleted);
                    categoryToAdd.SetModifiedOn(category.ModifiedOn);
                    await this.categoryRepository.AddCategoryAsync(categoryToAdd);
                }
            }
        }

        private async Task UpdateAccountsAsync(IEnumerable<AccountSyncDTO> accounts)
        {
            if (accounts == null || !accounts.Any())
            {
                return;
            }

            foreach (var account in accounts)
            {
                var parentAccountId = account.ParentAccountExternalId.HasValue ? (await this.accountRepository.GetAccountAsync(account.ParentAccountExternalId.Value)).Id : (int?)null;
                var accountGroupId = (await this.accountGroupRepository.GetAccountGroupAsync(account.AccountGroupExternalId)).Id;
                var userId = (await this.userRepository.GetFirstUserAsync()).Id;
                var accountToUpdate = await this.accountRepository.GetAccountAsync(account.ExternalId.Value);
                if (accountToUpdate != null)
                {
                    accountToUpdate.Edit(account.Name, account.CurrencyId, accountGroupId, account.IsIncludedToTotal, account.Comment, account.Order, account.Type, parentAccountId, account.IsDeleted, userId);
                    accountToUpdate.Delete(account.IsDeleted);
                    accountToUpdate.SetModifiedOn(account.ModifiedOn);
                    await this.accountRepository.UpdateAsync(accountToUpdate);
                }
                else
                {
                    var accountToAdd = Account.Create(account.Name, account.CurrencyId, accountGroupId, account.IsIncludedToTotal, account.Comment, account.Order, account.Type, parentAccountId, account.IsDeleted, userId, account.ExternalId);
                    accountToAdd.Delete(account.IsDeleted);
                    accountToAdd.SetModifiedOn(account.ModifiedOn);
                    await this.accountRepository.AddAccountAsync(accountToAdd);
                }
            }
        }

        private async Task UpdateAccountGroupsAsync(IEnumerable<AccountGroupSyncDTO> accountGroups)
        {
            if (accountGroups == null || !accountGroups.Any())
            {
                return;
            }

            foreach (var accountGroup in accountGroups)
            {
                var userId = (await this.userRepository.GetFirstUserAsync()).Id;
                var accountGroupToUpdate = await this.accountGroupRepository.GetAccountGroupAsync(accountGroup.ExternalId);
                if (accountGroupToUpdate != null)
                {
                    accountGroupToUpdate.Edit(accountGroup.Name, userId);
                    accountGroupToUpdate.Delete(accountGroup.IsDeleted);
                    accountGroupToUpdate.SetModifiedOn(accountGroup.ModifiedOn);
                    await this.accountGroupRepository.UpdateAsync(accountGroupToUpdate);
                }
                else
                {
                    var accounGroupToAdd = AccountGroup.Create(accountGroup.Name, userId, accountGroup.ExternalId);
                    accounGroupToAdd.Delete(accountGroup.IsDeleted);
                    accounGroupToAdd.SetModifiedOn(accountGroup.ModifiedOn);
                    await this.accountGroupRepository.AddAccountGroupAsync(accounGroupToAdd);
                }
            }
        }
    }
}
