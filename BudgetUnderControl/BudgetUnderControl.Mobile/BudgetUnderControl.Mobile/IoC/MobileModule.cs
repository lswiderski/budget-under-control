using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.IoC
{
    public class MobileModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(Settings.DB_SQLite_NAME);
            builder.RegisterInstance(new ContextConfig() { DbName = Settings.DB_SQLite_NAME, DbPath = dbPath, Application = ApplicationType.Mobile }).As<IContextConfig>();
            builder.RegisterInstance<IFileHelper>(DependencyService.Get<IFileHelper>());
            builder.RegisterType<ContextFacade>().As<IContextFacade>().InstancePerLifetimeScope();
            builder.RegisterType<EditAccountViewModel>().As<IEditAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddAccountViewModel>().As<IAddAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountsViewModel>().As<IAccountsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountDetailsViewModel>().As<IAccountDetailsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionsViewModel>().As<ITransactionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddTransactionViewModel>().As<IAddTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<EditTransactionViewModel>().As<IEditTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<OverviewViewModel>().As<IOverviewViewModel>().InstancePerLifetimeScope();
        }
    }
}
