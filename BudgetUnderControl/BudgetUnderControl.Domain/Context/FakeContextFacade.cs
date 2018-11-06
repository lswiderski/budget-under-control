using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BudgetUnderControl.Domain
{
    public class FakeContextFacade : IContextFacade
    {
        public DbSet<Account> Accounts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<AccountGroup> AccountGroup { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<AccountSnapshot> AccountSnapshot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<Category> Categories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<Currency> Currencies { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<ExchangeRate> ExchangeRates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<File> Files { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<Icon> Icons { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<Tag> Tags { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<TagToTransaction> TagsToTransactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<Transaction> Transactions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<Transfer> Transfers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public EntityEntry Remove(object entity)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
