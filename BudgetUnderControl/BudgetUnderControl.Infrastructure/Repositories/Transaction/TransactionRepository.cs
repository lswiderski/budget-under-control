using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class TransactionRepository : BaseModel, ITransactionRepository
    {
        private readonly IAccountRepository accountRepository;
        private readonly IUserIdentityContext userIdentityContext;
    
        public TransactionRepository(IContextFacade context, IAccountRepository accountRepository,
            IUserIdentityContext userIdentityContext) : base(context)
        {
            this.accountRepository = accountRepository;
            this.userIdentityContext = userIdentityContext;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            this.Context.Transactions.Add(transaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            this.Context.Transactions.Update(transaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddTransferAsync(Transfer transfer)
        {
            this.Context.Transfers.Add(transfer);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateTransferAsync(Transfer transfer)
        {
            this.Context.Transfers.Update(transfer);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveTransactionAsync(Transaction transaction)
        {
            this.Context.Transactions.Remove(transaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveTransferAsync(Transfer transfer)
        {
            this.Context.Transfers.Remove(transfer);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveTransactionsAsync(IEnumerable<Transaction> transactions)
        {
            this.Context.Transactions.RemoveRange(transactions);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveTransfersAsync(IEnumerable<Transfer> transfers)
        {
            this.Context.Transfers.RemoveRange(transfers);
            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<Transaction>> GetTransactionsAsync(TransactionsFilter filter = null)
        {

            var query = this.Context.Transactions
                        .Include(p => p.Category)
                        .Include(p => p.Account)
                        .ThenInclude(p => p.Currency)
                        .AsQueryable();


            if (filter != null && filter.AccountsIds != null && filter.AccountsIds.Any())
            {
                var accounts = await this.accountRepository.GetSubAccountsAsync(filter.AccountsIds);
                accounts.AddRange(filter.AccountsIds);
                accounts = accounts.Distinct().ToList();

                query = query.Where(q => accounts.Contains(q.AccountId)).AsQueryable();
            }
            else
            {
                query = query.Where(q => q.Account.OwnerId == userIdentityContext.UserId).AsQueryable();
            }

            if (filter != null)
            {
                query = query.Where(q => q.Date >= filter.FromDate).AsQueryable();
                query = query.Where(q => q.Date <= filter.ToDate).AsQueryable();
            }

            var transactionsWithExtraProperty = (await (from t in query
                                                        from transferFrom in this.Context.Transfers.Where(x => x.FromTransactionId == t.Id).DefaultIfEmpty()
                                                        from transferTo in this.Context.Transfers.Where(x => x.ToTransactionId == t.Id).DefaultIfEmpty()
                                                        orderby t.Date descending
                                                        select new
                                                        {
                                                            t,
                                                            IsTransfer = transferTo != null || transferFrom != null
                                                        })
                                                        .ToListAsync());
            transactionsWithExtraProperty.ForEach(x => x.t.IsTransfer = x.IsTransfer);
            var transactions = transactionsWithExtraProperty.Select(x => x.t).ToList();
            return transactions;
        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            var transaction = await this.Context.Transactions.Where(t => t.Id == id).SingleAsync();
            return transaction;
        }

        public async Task<Transaction> GetTransactionAsync(Guid id)
        {
            var transaction = await this.Context.Transactions.Where(t => t.ExternalId == id).SingleAsync();
            return transaction;
        }

        public async Task<Transfer> GetTransferAsync(int transactionId)
        {
            var transfer = await this.Context.Transfers.Where(t => t.FromTransactionId == transactionId || t.ToTransactionId == transactionId).SingleOrDefaultAsync();
            return transfer;
        }

        public async Task<ICollection<Transfer>> GetTransfersAsync()
        {
            var transfers = await this.Context.Transfers.ToListAsync();

            return transfers;
        }
            
    }
}
