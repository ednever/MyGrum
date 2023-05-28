using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        Label label;
        public ListPage()
        {
            Title = "Список";
            label = new Label();
            StackLayout st = new StackLayout { Children = { label } };
            Content = st;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Preferences.ContainsKey("1"))
            {
                label.Text += "\n" + Preferences.Get("1", "Pole andmed");
                Preferences.Clear();                
            }           
        }
    }  
}