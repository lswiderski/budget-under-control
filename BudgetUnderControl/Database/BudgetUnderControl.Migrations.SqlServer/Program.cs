using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BudgetUnderControl.Migrations.SqlServer
{
    class Program : IDesignTimeDbContextFactory<Context>
    {
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

            var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME, Application = ApplicationType.SqlServerMigrations, DbPassword = "Qwerty!1", DbUser = "buc" };
            var connectionString = contextConfig.ConnectionString;


            DbContextOptionsBuilder<Context> optionsBuilder = new DbContextOptionsBuilder<Context>()
                .UseSqlServer(connectionString);

             return new Context(optionsBuilder.Options);
            //return new Context(contextConfig);
        }
    }


}
