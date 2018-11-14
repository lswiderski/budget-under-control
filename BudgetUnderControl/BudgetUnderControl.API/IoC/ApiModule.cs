using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetUnderControl.API.IoC
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME,  Application = ApplicationType.SqlServerMigrations, DbPassword= "Qwerty!1", DbUser="buc" };
            var contextConfig = new ContextConfig() { DbName = Settings.DB_SQLServer_NAME, Application = ApplicationType.Web, DbPassword = "Qwerty!1", DbUser = "buc" };

            builder.RegisterInstance(contextConfig).As<IContextConfig>();
            builder.RegisterType<WebContextFacade>().As<IContextFacade>().InstancePerLifetimeScope();
        }
    }
}
