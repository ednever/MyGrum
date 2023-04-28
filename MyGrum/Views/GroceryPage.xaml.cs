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
            Title = "Продукты";
            Label label = new Label { Text = "Доброе утро!" };
            Content = label;
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