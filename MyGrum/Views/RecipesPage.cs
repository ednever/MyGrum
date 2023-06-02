using MyGrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        string[] fileNames = { "Soogiajad.txt", "Retseptid.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        List<Soogiajad> soogiajad = new List<Soogiajad>();
        List<Retseptid> retseptid = new List<Retseptid>();
        List<Retseptid> retseptidUhesSoogiajas = new List<Retseptid>();

        Grid grid;
        List<Image> images = new List<Image>();
        TapGestureRecognizer tap = new TapGestureRecognizer();

        bool isPageInModeSoogiajad;
        bool isFirstLoad = true;
        int soogiaegID;

        public RecipesPage(string pealkiri, bool isPageInModeSoogiajad, int soogiaegID)
        {
            Title = pealkiri;
            this.isPageInModeSoogiajad = isPageInModeSoogiajad;
            this.soogiaegID = soogiaegID;

            Label label = new Label { Text = "Добрый вечер!" };
            StackLayout st = new StackLayout { Children = { label } };
            Content = st;

            tap.Tapped += Tap_Tapped;
        }
        async void Tap_Tapped(object sender, EventArgs e)
        {
            //Frame frm = (Frame)sender;

            //if (isPageInModeSoogiajad)
            //{
            //    if (grid.Children.Last() == frm)
            //    {
            //        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, frm.TabIndex, frm.TabIndex + 1));
            //    }
            //    else
            //    {
            //        katID = frm.TabIndex;
            //        await Navigation.PushAsync(new GroceryPage(images[frm.TabIndex].AutomationId, false, frm.TabIndex + 1));
            //    }
            //}
            //else
            //{
            //    if (grid.Children.Last() == frm)
            //    {
            //        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, katID, katID));
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, katID, katID));
            //    }
            //}
        }
    }
}