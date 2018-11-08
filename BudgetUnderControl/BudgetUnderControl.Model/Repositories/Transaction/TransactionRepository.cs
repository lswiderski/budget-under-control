using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Contracts.Models;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class TransactionRepository : BaseModel, ITransactionRepository
    {
        IAccountRepository accountRepository;
        public TransactionRepository(IContextFacade context, IAccountRepository accountRepository) : base(context)
        {
            this.accountRepository = accountRepository;
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


            if (filter != null && filter.AccountId.HasValue)
            {
                var accounts = await this.accountRepository.GetSubAccountsAsync(filter.AccountId.Value);
                accounts.Add(filter.AccountId.Value);
                accounts = accounts.Distinct().ToList();

                query = query.Where(q => accounts.Contains(q.AccountId)).AsQueryable();
            }

            if (filter != null && filter.FromDate.HasValue)
            {
                query = query.Where(q => q.Date >= filter.FromDate.Value).AsQueryable();
            }

            if (filter != null && filter.ToDate.HasValue)
            {
                query = query.Where(q => q.Date <= filter.ToDate.Value).AsQueryable();
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
