using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain
{
    public class WebContextFacade : IContextFacade
    {
        public int SaveChanges() => _context.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => _context.SaveChangesAsync(cancellationToken);

        public EntityEntry Remove(object entity) => _context.Remove(entity);

        public EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class => _context.Remove<TEntity>(entity);

        public void RemoveRange(IEnumerable<object> entities) => _context.Remove(entities);

        private readonly Context _context;

        public static ContextFacade Create(IContextConfig contextConfig) => new ContextFacade(contextConfig);

        public WebContextFacade(Context context)
        {
            _context = context;
        }

        public DbSet<Account> Accounts
        {
            get => _context.Accounts;
            set => _context.Accounts = value;
        }

        public DbSet<AccountGroup> AccountGroup
        {
            get => _context.AccountGroup;
            set => _context.AccountGroup = value;
        }
        public DbSet<AccountSnapshot> AccountSnapshot
        {
            get => _context.AccountSnapshot;
            set => _context.AccountSnapshot = value;
        }
        public DbSet<Category> Categories
        {
            get => _context.Categories;
            set => _context.Categories = value;
        }
        public DbSet<Currency> Currencies
        {
            get => _context.Currencies;
            set => _context.Currencies = value;
        }
        public DbSet<ExchangeRate> ExchangeRates
        {
            get => _context.ExchangeRates;
            set => _context.ExchangeRates = value;
        }
        public DbSet<File> Files
        {
            get => _context.Files;
            set => _context.Files = value;
        }
        public DbSet<Icon> Icons
        {
            get => _context.Icons;
            set => _context.Icons = value;
        }
        public DbSet<Tag> Tags
        {
            get => _context.Tags;
            set => _context.Tags = value;
        }
        public DbSet<TagToTransaction> TagsToTransactions
        {
            get => _context.TagsToTransactions;
            set => _context.TagsToTransactions = value;
        }
        public DbSet<Transaction> Transactions
        {
            get => _context.Transactions;
            set => _context.Transactions = value;
        }
        public DbSet<Transfer> Transfers
        {
            get => _context.Transfers;
            set => _context.Transfers = value;
        }

    }
}
