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
        int[] ints = new int[6] { 0, 1, 2, 3, 4, 5 };
        public GroceryPage()
        {
            Title = "Продукты";
            StackLayout st = new StackLayout();
            for (int i = 0; i < ints.Length / 3; i++) //ряды
            {
                StackLayout st1 = new StackLayout { Orientation = StackOrientation.Horizontal};
                for (int j = 0; j < 3; j++) //колонки
                {
                    Label label = new Label { Text = ints[j].ToString() };
                    st1.Children.Add(label);
                }
                st.Children.Add(st1);
            }
            
            

            


            
            Content = st;
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