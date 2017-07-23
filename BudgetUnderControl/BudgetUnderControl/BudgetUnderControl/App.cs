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
using System.ComponentModel;

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
            //Resources = new ResourceDictionary();
            //Resources.Add("primaryGreen", Color.FromHex("91CA47"));
            //Resources.Add("primaryDarkGreen", Color.FromHex("6FA22E"));
            //
            //var nav = new NavigationPage(new StartScreen());
            //nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
            //nav.BarTextColor = Color.White;

            //MainPage = nav;

            MainPage = MasterPage = new BudgetUnderControl.Views.MasterPage();
        }

        protected override void OnStart()
        {

            AutoFacInit();
            //Debug.WriteLine("OnStart");

            //// always re-set when the app starts
            //// users expect this (usually)
            ////			Properties ["ResumeAtTodoId"] = "";
            //if (Properties.ContainsKey("ResumeAtTodoId"))
            //{
            //	var rati = Properties["ResumeAtTodoId"].ToString();
            //	Debug.WriteLine("   rati=" + rati);
            //	if (!String.IsNullOrEmpty(rati))
            //	{
            //		Debug.WriteLine("   rati=" + rati);
            //		ResumeAtTodoId = int.Parse(rati);

            //		if (ResumeAtTodoId >= 0)
            //		{
            //			var todoPage = new TodoItemPage();
            //			todoPage.BindingContext = await Database.GetItemAsync(ResumeAtTodoId);
            //			await MainPage.Navigation.PushAsync(todoPage, false); // no animation
            //		}
            //	}
            //}
        }

        protected override void OnSleep()
        {
            //Debug.WriteLine("OnSleep saving ResumeAtTodoId = " + ResumeAtTodoId);
            //// the app should keep updating this value, to
            //// keep the "state" in case of a sleep/resume
            //Properties["ResumeAtTodoId"] = ResumeAtTodoId;
        }

        protected override void OnResume()
        {
            //Debug.WriteLine("OnResume");
            //if (Properties.ContainsKey("ResumeAtTodoId"))
            //{
            //	var rati = Properties["ResumeAtTodoId"].ToString();
            //	Debug.WriteLine("   rati=" + rati);
            //	if (!String.IsNullOrEmpty(rati))
            //	{
            //		Debug.WriteLine("   rati=" + rati);
            //		ResumeAtTodoId = int.Parse(rati);

            //		if (ResumeAtTodoId >= 0)
            //		{
            //			var todoPage = new TodoItemPage();
            //			todoPage.BindingContext = await Database.GetItemAsync(ResumeAtTodoId);
            //			await MainPage.Navigation.PushAsync(todoPage, false); // no animation
            //		}
            //	}
            //}
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

            builder.RegisterType<EditAccountViewModel>().As<IEditAccountViewModel>().InstancePerDependency();
            builder.RegisterType<AddAccountViewModel>().As<IAddAccountViewModel>().InstancePerDependency();
            builder.RegisterType<AccountsViewModel>().As<IAccountsViewModel>().InstancePerDependency();
            builder.RegisterType<AccountDetailsViewModel>().As<IAccountDetailsViewModel>().InstancePerDependency();
            builder.RegisterType<TransactionsViewModel>().As<ITransactionsViewModel>().InstancePerDependency();
            builder.RegisterType<AddTransactionViewModel>().As<IAddTransactionViewModel>().InstancePerDependency();
            builder.RegisterType<EditTransactionViewModel>().As<IEditTransactionViewModel>().InstancePerDependency();
            

            App.Container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(App.Container));

        }
    }
}

