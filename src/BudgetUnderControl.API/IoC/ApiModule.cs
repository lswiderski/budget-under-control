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
            var settings = this.configuration.GetSettings<GeneralSettings>().InjectEnvVariables();
            Console.WriteLine("Connection string: " + settings.ConnectionString);
            Console.WriteLine("Application Type:  " + settings.ApplicationType.ToString());
            Console.WriteLine("DB Name:  " + settings.BUC_DB_Name);

            builder.RegisterInstance(settings)
                .SingleInstance();

            //var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME,  Application = ApplicationType.SqlServerMigrations, DbPassword= "Qwerty!1", DbUser="buc" };
            var contextConfig = new ContextConfig() { DbName = settings.BUC_DB_Name, Application = settings.ApplicationType, ConnectionString = settings.ConnectionString };

            builder.RegisterInstance(contextConfig).As<IContextConfig>();
            builder.RegisterType<ContextFacade>().As<IContextFacade>().SingleInstance();
            var logManager = new NLogManager();
            builder.RegisterInstance(logManager).As<ILogManager>().SingleInstance();
            builder.RegisterInstance(logManager.GetLog()).As<ILogger>();

        }
    }
}
