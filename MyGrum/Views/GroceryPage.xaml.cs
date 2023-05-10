using MyGrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroceryPage : ContentPage
    {
        int[] ints = { 1, 2, 3, 4, 5, 6 };






        string[] fileNames = { "Kategooriad.txt", "Tooted.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);







        List<Kategooriad> kategooriad = new List<Kategooriad>();
        int[][] matrix = new int[3][];

        public void Test()
        {
            matrix[0] = new int[] { 1, 2, 3 };
            matrix[1] = new int[] { 1, 2, 3 };
            matrix[2] = new int[] { 1, 2, 3 };
            //matrix[3] = new int[] { 1, 2, 3 };















            //Создание файлов
            File.WriteAllText(Path.Combine(folderPath, fileNames[0]), "1,Овощи,vegetables.png"); //Категория
            File.WriteAllText(Path.Combine(folderPath, fileNames[1]), "1,Картофель,potato.png,1"); //Товар

            if (String.IsNullOrEmpty(fileNames[0])) return;
            if (fileNames[0] != null)
            {
                String[] Andmed = File.ReadAllLines(Path.Combine(folderPath, fileNames[0]));
                for (int i = 0; i < Andmed.Length; i++)
                {
                    var columns = Andmed[i].Split(',');
                    Kategooriad kategooria = new Kategooriad(int.Parse(columns[0]), columns[1], columns[2]);
                    kategooriad.Add(kategooria);
                }
            }
        }







        public GroceryPage()
        {
            Title = "Категории";

            Test();

            StackLayout st = new StackLayout();
            for (int i = 0; i < kategooriad.Count; i++)
            {
                StackLayout st1 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                Label label = new Label { Text = kategooriad[i].Kategooria, FontSize = 20, Margin = 30 };
                Frame frame = new Frame
                {
                    BorderColor = Color.Black,
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 15,
                    Content = label
                };

                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += Tap_Tapped;

                Label label2 = new Label { Text = "+", FontSize = 20, Margin = 30 };

                Frame frame2 = new Frame
                {
                    BorderColor = Color.Black,
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 15,
                    Content = label2
                };

                frame.GestureRecognizers.Add(tap);
                st1.Children.Add(frame);

                frame2.GestureRecognizers.Add(tap);
                st1.Children.Add(frame2);

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
}
//Неделя на выполнение - до 14.05
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