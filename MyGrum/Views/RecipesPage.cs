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
    public partial class RecipesPage : ContentPage
    {
        public RecipesPage()
        {
            Title = "Рецепты";
            Label label = new Label { Text = "Добрый вечер!" };
            StackLayout st = new StackLayout { Children = { label } };
            Content = st;
        }
    }
}