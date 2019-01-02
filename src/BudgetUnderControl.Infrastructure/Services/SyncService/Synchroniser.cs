﻿using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        private readonly ITagRepository tagRepository;
        private readonly ITransactionService transactionService;
        private readonly GeneralSettings settings;
        private Dictionary<Guid, int> _tags;

        public Synchroniser(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICurrencyRepository currencyRepository,
            ICategoryRepository categoryRepository,
            IAccountGroupRepository accountGroupRepository,
            IUserRepository userRepository,
            ISynchronizationRepository synchronizationRepository,
            IUserIdentityContext userIdentityContext,
            ITagRepository tagRepository,
            ITransactionService transactionService,
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
            this.tagRepository = tagRepository;
            this.transactionService = transactionService;
        }

        public async Task SynchroniseAsync(SyncRequest syncRequest)
        {
            await this.UpdateLastSyncDateAsync(syncRequest);
            await this.UpdateUsersAsync(syncRequest.Users);
            await this.UpdateTagsAsync(syncRequest.Tags);
            await this.UpdateCategoriesAsync(syncRequest.Categories);
            await this.UpdateAccountGroupsAsync(syncRequest.AccountGroups);
            await this.UpdateAccountsAsync(syncRequest.Accounts);
            _tags = (await this.tagRepository.GetAsync()).ToDictionary(x => x.ExternalId, x => x.Id);
            await this.UpdateTransactionsAsync(syncRequest.Transactions);
            await this.UpdateTransfersAsync(syncRequest.Transfers);
        }

        private async Task UpdateLastSyncDateAsync(SyncRequest syncRequest)
        {
            var userId = (await this.userRepository.GetFirstUserAsync()).Id;
            var syncObject = await this.synchronizationRepository.GetSynchronizationAsync(syncRequest.Component, syncRequest.ComponentId, userId);// syncRequest.UserId)

            if (syncObject != null)
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

            var transactionsToUpdate = new List<Domain.Transaction>();
            var transactionsToAdd = new List<Domain.Transaction>();

            const int packageSize = 200;
            var categories = (await this.categoryRepository.GetCategoriesAsync()).GroupBy(x => x.ExternalId).Select(x => x.FirstOrDefault()).ToList();
            var dictCategories = categories
                .ToDictionary(c => c.ExternalId, c => c.Id);

            var accounts = (await this.accountRepository.GetAccountsAsync()).Distinct().ToList();
            var dictAccounts = accounts
                .ToDictionary(c => c.ExternalId, c => c.Id);

            while (transactions.Any())
            {
                var package = this.GetPartOfTransactions(transactions, packageSize);
                transactions = transactions.Skip(packageSize);

                foreach (var transaction in package)
                {
                    int? categoryId = transaction.CategoryExternalId.HasValue ? dictCategories.ContainsKey(transaction.CategoryExternalId.Value) ? dictCategories[transaction.CategoryExternalId.Value]: (int?)null : (int?)null;
                    var accountId = dictAccounts[transaction.AccountExternalId.Value];
                    var transactionToUpdate = await this.transactionRepository.GetTransactionAsync(transaction.ExternalId.Value);

                    if (transactionToUpdate != null)
                    {
                        if (transactionToUpdate.ModifiedOn < transaction.ModifiedOn)
                        {
                            transactionToUpdate.Edit(accountId, transaction.Type, transaction.Amount, transaction.Date, transaction.Name, transaction.Comment, this.userIdentityContext.UserId, transaction.IsDeleted, categoryId);
                            transactionToUpdate.SetCreatedOn(transaction.CreatedOn);
                            transactionToUpdate.SetModifiedOn(transaction.ModifiedOn);
                            transactionsToUpdate.Add(transactionToUpdate);
                            await this.DealWithTransactionToTagsAsync(transactionToUpdate.Id, transaction.Tags);
                        }
                    }
                    else
                    {
                        var transactionToAdd = Domain.Transaction.Create(accountId, transaction.Type, transaction.Amount, transaction.Date, transaction.Name, transaction.Comment, this.userIdentityContext.UserId, false, categoryId, transaction.ExternalId);
                        transactionToAdd.SetCreatedOn(transaction.CreatedOn);
                        transactionToAdd.SetModifiedOn(transaction.ModifiedOn);
                        transactionsToAdd.Add(transactionToAdd);
                    }
                }

                if (transactionsToUpdate.Any())
                {
                    await this.transactionRepository.UpdateAsync(transactionsToUpdate);
                    transactionsToUpdate.Clear();
                }

                if (transactionsToAdd.Any())
                {
                    await this.transactionRepository.AddTransactionsAsync(transactionsToAdd);
                    foreach (var item in transactionsToAdd)
                    {
                        var tags = package.Where(x => x.ExternalId == item.ExternalId).Select(x => x.Tags).FirstOrDefault();
                        await this.DealWithTransactionToTagsAsync(item.Id, tags);
                    }
                    transactionsToAdd.Clear();
                }
            } 
           
        }

        private async Task DealWithTransactionToTagsAsync(int transactionId, List<TagSyncDTO> tagsToSync)
        {
            var tagIds = tagsToSync.Select(x => _tags[x.ExternalId]).ToList(); 
            var tags2Transactions = await this.tagRepository.GetTagToTransactionsAsync(transactionId);
            var tags2Add = tagIds.Except(tags2Transactions.Select(t => t.TagId));
            var tags2Remove = tags2Transactions.Select(t => t.TagId).Except(tagIds);
            var tags2Transactions2Remove = tags2Transactions.Where(t => tags2Remove.Contains(t.TagId));
            //add new
            if(tags2Add.Any())
            {
                await transactionService.CreateTagsToTransaction(tags2Add, transactionId);
            }
            
            //delete removed
            if(tags2Transactions2Remove.Any())
            {
                await this.tagRepository.RemoveAsync(tags2Transactions2Remove);
            }
            
        }

        private IEnumerable<TransactionSyncDTO> GetPartOfTransactions(IEnumerable<TransactionSyncDTO> transactions, int packageSize)
        {
            return transactions.Take(packageSize);
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
                    if(transferToUpdate.ModifiedOn < transfer.ModifiedOn)
                    {
                        transferToUpdate.SetRate(transfer.Rate);
                        transferToUpdate.Delete(transfer.IsDeleted);
                        transferToUpdate.SetModifiedOn(transfer.ModifiedOn);
                        await this.transactionRepository.UpdateTransferAsync(transferToUpdate);
                    }
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

            var userId = (await this.userRepository.GetFirstUserAsync()).Id;
            foreach (var tag in tags)
            {
               
                var tagToUpdate = await this.tagRepository.GetAsync(tag.ExternalId);
                if (tagToUpdate != null)
                {
                    if (tagToUpdate.ModifiedOn < tag.ModifiedOn)
                    {
                        tagToUpdate.Edit(tag.Name, userId, tag.IsDeleted);
                        tagToUpdate.Delete(tag.IsDeleted);
                        tagToUpdate.SetModifiedOn(tag.ModifiedOn);
                        await this.tagRepository.UpdateAsync(tagToUpdate);
                    }

                }
                else
                {
                    var tagToAdd = Tag.Create(tag.Name, userId, tag.IsDeleted ,tag.ExternalId);
                    tagToAdd.Delete(tag.IsDeleted);
                    tagToAdd.SetModifiedOn(tag.ModifiedOn);
                    await this.tagRepository.AddAsync(tagToAdd);
                }
            }
        }

        private async Task UpdateCategoriesAsync(IEnumerable<CategorySyncDTO> categories)
        {
            if (categories == null || !categories.Any())
            {
                return;
            }

            var userId = (await this.userRepository.GetFirstUserAsync()).Id;

            foreach (var category in categories)
            {
               
                var categoryToUpdate = await this.categoryRepository.GetCategoryAsync(category.ExternalId);
                if (categoryToUpdate != null)
                {
                    if(categoryToUpdate.ModifiedOn < category.ModifiedOn)
                    {
                        categoryToUpdate.Edit(category.Name, userId);
                        categoryToUpdate.Delete(category.IsDeleted);
                        categoryToUpdate.SetModifiedOn(category.ModifiedOn);
                        await this.categoryRepository.UpdateAsync(categoryToUpdate);
                    }
                    
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

            accounts = accounts.OrderBy(a => a.ParentAccountId);

            foreach (var account in accounts)
            {
                var parentAccountId = account.ParentAccountExternalId.HasValue ? (await this.accountRepository.GetAccountAsync(account.ParentAccountExternalId.Value)).Id : (int?)null;
                var accountGroupId = (await this.accountGroupRepository.GetAccountGroupAsync(account.AccountGroupExternalId)).Id;
                var userId = this.userIdentityContext.UserId;
                var accountToUpdate = await this.accountRepository.GetAccountAsync(account.ExternalId.Value);
                if (accountToUpdate != null)
                {
                    if(accountToUpdate.ModifiedOn < account.ModifiedOn)
                    {
                        accountToUpdate.Edit(account.Name, account.CurrencyId, accountGroupId, account.IsIncludedToTotal, account.Comment, account.Order, account.Type, parentAccountId, !account.IsDeleted, userId);
                        accountToUpdate.Delete(account.IsDeleted);
                        accountToUpdate.SetModifiedOn(account.ModifiedOn);
                        await this.accountRepository.UpdateAsync(accountToUpdate);
                    }
                }
                else
                {
                    var accountToAdd = Account.Create(account.Name, account.CurrencyId, accountGroupId, account.IsIncludedToTotal, account.Comment, account.Order, account.Type, parentAccountId, !account.IsDeleted, userId, account.ExternalId);
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
                if (accountGroupToUpdate != null )
                {
                    if(accountGroupToUpdate.ModifiedOn < accountGroup.ModifiedOn)
                    {
                        accountGroupToUpdate.Edit(accountGroup.Name, userId);
                        accountGroupToUpdate.Delete(accountGroup.IsDeleted);
                        accountGroupToUpdate.SetModifiedOn(accountGroup.ModifiedOn);
                        await this.accountGroupRepository.UpdateAsync(accountGroupToUpdate);
                    }
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
