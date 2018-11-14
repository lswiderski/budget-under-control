using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Repositories;
using BudgetUnderControl.Infrastructure.Services;
using BudgetUnderControl.Infrastructure.Services.UserService;
using BudgetUnderControl.Model;
using BudgetUnderControl.Model.Services;
using CommonServiceLocator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BudgetUnderControl.API
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);


            services.AddEntityFrameworkSqlServer()
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<Context>();

            var dbPath = Settings.DB_SQLite_NAME;

            // Initialize Autofac builder
            var builder = new ContainerBuilder();
            //var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME, DbPath = dbPath, Application = ApplicationType.SqlServerMigrations, DbPassword= "Qwerty!1", DbUser="buc" };
            var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME, DbPath = dbPath, Application = ApplicationType.Web, DbPassword= "Qwerty!1", DbUser="buc" };
            // Register services
            builder.RegisterInstance(contextConfig).As<IContextConfig>();
            builder.RegisterType<WebContextFacade>().As<IContextFacade>().InstancePerLifetimeScope();
            builder.RegisterInstance(new Context(contextConfig));
            builder.RegisterType<BaseModel>().As<IBaseModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountGroupService>().As<IAccountGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AccountGroupRepository>().As<IAccountGroupRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SyncService>().As<ISyncService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            builder.Register<Func<IUserIdentityContext>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return () =>
                {
                    var service = context.Resolve<IUserService>();
                    var identity = service.CreateUserIdentityContext();
                    return identity;
                };
            });

            builder.Register<IUserIdentityContext>(c =>
            {
                return c.Resolve<Func<IUserIdentityContext>>()();
            });

            builder.Populate(services);
           // builder.RegisterModule(new ContainerModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
