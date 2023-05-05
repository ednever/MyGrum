using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroceryPage : ContentPage
    {
        int[] ints = { 1, 2, 3, 4, 5, 6 };
        public GroceryPage()
        {
            Title = "Категории";
            StackLayout st = new StackLayout();
            for (int i = 0; i < 4; i += 3)
            {
                StackLayout st1 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                for (int j = 0; j < 3; j++)
                {
                    Label label = new Label { Text = ints[i + j].ToString(), FontSize = 20, Margin = 30 };

                    Frame frame = new Frame
                    {
                        BorderColor = Color.Black,
                        BackgroundColor = Color.Transparent,
                        CornerRadius = 15,
                        Content = label
                    };
                    TapGestureRecognizer tap = new TapGestureRecognizer();
                    tap.Tapped += Tap_Tapped;
                    frame.GestureRecognizers.Add(tap);
                    st1.Children.Add(frame);
                }

                st.Children.Add(st1);
            }
            Content = st;
        }
        public GroceryPage(int[] ints)
        {
            this.ints = ints;
            Title = "Товары";

            //Content = st;
        }

        public async void Tap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GroceryPage(ints));
        }
    }
    public class Vizualization
    {
        public Vizualization()
        {
            
        }

    }
}
//Неделя на выполнение - до 06.05
//План действий:
//  1. Создание базы данных
//  2. Заполнение базы данных
//  3. Написание кода
//  4. Тестировка
//  5. Оформление
//  6. Тестировка

/*
 * Сделать расстояние между боксами одинаковое
 * Заменить цифры на картинки
 * Переделать страницу в общий класс
 */