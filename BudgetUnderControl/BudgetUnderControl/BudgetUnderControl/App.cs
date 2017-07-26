using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using BudgetUnderControl.Domain;
using Autofac;
using BudgetUnderControl.Views;
using BudgetUnderControl.Common;
using BudgetUnderControl.Model;
using Microsoft.Practices.ServiceLocation;
using Autofac.Extras.CommonServiceLocator;
using BudgetUnderControl.ViewModel;

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
            builder.RegisterInstance(new ContextConfig() { DbName = Settings.DB_NAME, DbPath = dbPath }).As<IContextConfig>();
            //builder.RegisterInstance(new Context(new ContextConfig() { DbName = Settings.DB_NAME, DbPath = dbPath }));
            builder.RegisterType<BaseModel>().As<IBaseModel>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyModel>().As<ICurrencyModel>().SingleInstance();
            builder.RegisterType<AccountModel>().As<IAccountModel>().SingleInstance();
            builder.RegisterType<AccountGroupModel>().As<IAccountGroupModel>().SingleInstance();
            builder.RegisterType<TransactionModel>().As<ITransactionModel>().SingleInstance();
            builder.RegisterType<CategoryModel>().As<ICategoryModel>().SingleInstance();
            builder.RegisterType<SyncService>().As<ISyncService>().SingleInstance();
            builder.RegisterInstance<IFileHelper>(DependencyService.Get<IFileHelper>());

            builder.RegisterType<EditAccountViewModel>().As<IEditAccountViewModel>().InstancePerDependency();
            builder.RegisterType<AddAccountViewModel>().As<IAddAccountViewModel>().InstancePerDependency();
            builder.RegisterType<AccountsViewModel>().As<IAccountsViewModel>().InstancePerDependency();
            builder.RegisterType<AccountDetailsViewModel>().As<IAccountDetailsViewModel>().InstancePerDependency();
            builder.RegisterType<TransactionsViewModel>().As<ITransactionsViewModel>().InstancePerDependency();
            builder.RegisterType<AddTransactionViewModel>().As<IAddTransactionViewModel>().InstancePerDependency();
            builder.RegisterType<EditTransactionViewModel>().As<IEditTransactionViewModel>().InstancePerDependency();
            builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>().InstancePerDependency();
            



            App.Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(App.Container));

        }
    }
}

