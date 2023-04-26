using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroceryPage : ContentPage
    {
        public GroceryPage()
        {
            Label label = new Label { Text = "Tere hommikust!" };
            Content = label;
        }
    }
}