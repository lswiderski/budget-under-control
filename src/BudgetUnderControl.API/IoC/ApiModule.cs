using Autofac;
using BudgetUnderControl.API.Extensions;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.CommonInfrastructure.Settings;
using BudgetUnderControl.Domain;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetUnderControl.API.IoC
{
    public class ApiModule : Module
    {
        private readonly IConfiguration configuration;

        public ApiModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var settings = this.configuration.GetSettings<GeneralSettings>();
            builder.RegisterInstance(settings)
                .SingleInstance();

            //var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME,  Application = ApplicationType.SqlServerMigrations, DbPassword= "Qwerty!1", DbUser="buc" };
            var contextConfig = new ContextConfig() { DbName = settings.DbName, Application = settings.ApplicationType, ConnectionString = settings.ConnectionString };

            builder.RegisterInstance(contextConfig).As<IContextConfig>();
            builder.RegisterType<ContextFacade>().As<IContextFacade>().SingleInstance();
            var logManager = new NLogManager();
            builder.RegisterInstance(logManager).As<ILogManager>().SingleInstance();
            builder.RegisterInstance(logManager.GetLog()).As<ILogger>();

        }
    }
}
