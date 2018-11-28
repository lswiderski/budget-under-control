using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Infrastructure.Settings;
using BudgetUnderControl.Mobile.Extensions;
using BudgetUnderControl.Mobile.Services;
using BudgetUnderControl.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.IoC
{
    public class MobileModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var settings = this.GetSettings<GeneralSettings>("generalsettings.json");
            builder.RegisterInstance(settings)
              .SingleInstance();

            var dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(settings.DbName);
           
            builder.RegisterInstance(new ContextConfig() { DbName = settings.DbName, DbPath = dbPath, Application = ApplicationType.Mobile , ConnectionString = settings.ConnectionString }).As<IContextConfig>();
            builder.RegisterInstance<IFileHelper>(DependencyService.Get<IFileHelper>());
            builder.RegisterType<ContextFacade>().As<IContextFacade>().SingleInstance();
            builder.RegisterType<EditAccountViewModel>().As<IEditAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddAccountViewModel>().As<IAddAccountViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountsViewModel>().As<IAccountsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AccountDetailsViewModel>().As<IAccountDetailsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionsViewModel>().As<ITransactionsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<AddTransactionViewModel>().As<IAddTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<EditTransactionViewModel>().As<IEditTransactionViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<OverviewViewModel>().As<IOverviewViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SyncMobileService>().As<ISyncMobileService>().InstancePerLifetimeScope();

            builder.Register(ctx => new HttpClient() { BaseAddress = new Uri(settings.ApiBaseUri) })
            .Named<HttpClient>("api")
            .SingleInstance();

        }

    }
}
