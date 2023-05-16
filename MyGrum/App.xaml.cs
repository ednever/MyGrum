using MyGrum.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGrum
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Create a TabbedPage and set its Children collection
            //var tabbedPage = new TabbedPage { Children = { new GroceryPage(), new ListPage(), new RecipesPage() } };
            //TabBar tabBar = new TabBar();





            //Shell shell= new Shell();
            //shell.






            //tabBar
            //tabbedPage.Children.Add(tabBar);
            //MainPage= tabbedPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
