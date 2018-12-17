using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
namespace BudgetUnderControl.Migrations.AndroidRunner
{
    class Program : IDesignTimeDbContextFactory<Context>
    {
        //used only for creating migrations for mobile version of project
        static void Main(string[] args)
        {
            Program p = new Program();
            using (Context context = p.CreateDbContext(null))
            {

            }
            Console.WriteLine("Hello World!");
        }

        public Context CreateDbContext(string[] args)
        {

            var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLite_NAME, Application = ApplicationType.SQLiteMigrations, ConnectionString = "Filename="};
            var connectionString = contextConfig.ConnectionString;


            DbContextOptionsBuilder<Context> optionsBuilder = new DbContextOptionsBuilder<Context>()
                .UseSqlite(connectionString, options => options.MigrationsAssembly("BudgetUnderControl.Migrations.SQLite"));

            return new Context(optionsBuilder.Options, contextConfig);
        }
    }
}
