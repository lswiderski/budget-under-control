using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using BudgetUnderControl.Domain;
using Autofac;
using BudgetUnderControl.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BudgetUnderControl
{
    public class App : Application
    {
        static Context context;
        static IContainer Container;

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
            MainPage = new BudgetUnderControl.Views.MasterPage();
        }

        public static Context Context
        {
            get
            {
                if (context == null)
                {
                    context = new Context(DependencyService.Get<IFileHelper>().GetLocalFilePath("buc.db3"));
                }
                return context;
            }
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
            // Initialize Autofac builder
            var builder = new ContainerBuilder();

            // Register services
            // builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            //builder.RegisterInstance(new ApplePlatform()).As<IPlatform>();
            // TODO add your services or load modules

            builder.RegisterType<Context>();
            App.Container = builder.Build();
        }
    }
}

