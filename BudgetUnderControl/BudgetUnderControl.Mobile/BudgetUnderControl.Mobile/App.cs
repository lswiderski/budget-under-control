using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using BudgetUnderControl.Domain;
using Autofac;
using BudgetUnderControl.Views;
using BudgetUnderControl.Common;
using BudgetUnderControl.Model;
using Autofac.Extras.CommonServiceLocator;
using BudgetUnderControl.ViewModel;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Model.Services;
using CommonServiceLocator;
using BudgetUnderControl.Common.Enums;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BudgetUnderControl
{
    public class App : Application
    {
        public static IContainer Container;
        public static MasterPage MasterPage
        {
            get;
            private set;
        }

        public App()
        {
            
        }

        protected override void OnStart()
        {

            AutoFacInit();
            MainPage = MasterPage = new BudgetUnderControl.Views.MasterPage();
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
           
        }

        protected void AutoFacInit()
        {
            var dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(Settings.DB_NAME);

            // Initialize Autofac builder
            var builder = new ContainerBuilder();

            // Register services
            builder.RegisterInstance(new ContextConfig() { DbName = Settings.DB_NAME, DbPath = dbPath, Application = ApplicationType.Mobile }).As<IContextConfig>();
            builder.RegisterType<ContextFacade>().As<IContextFacade>().InstancePerLifetimeScope();
            //builder.RegisterInstance(new Context(new ContextConfig() { DbName = Settings.DB_NAME, DbPath = dbPath }));
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
            builder.RegisterType<SyncService>().As<ISyncService>().InstancePerLifetimeScope();
            builder.RegisterInstance<IFileHelper>(DependencyService.Get<IFileHelper>());

            builder.RegisterType<EditAccountViewModel>().As<IEditAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddAccountViewModel>().As<IAddAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountsViewModel>().As<IAccountsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountDetailsViewModel>().As<IAccountDetailsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionsViewModel>().As<ITransactionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddTransactionViewModel>().As<IAddTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<EditTransactionViewModel>().As<IEditTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<OverviewViewModel>().As<IOverviewViewModel>().InstancePerLifetimeScope();



            App.Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

        }
    }
}

