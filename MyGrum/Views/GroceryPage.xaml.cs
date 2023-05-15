using MyGrum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
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
        Grid grid;
        List<Label> labels = new List<Label>();
        
        public GroceryPage()
        {
            Title = "Категории";

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            FileOutput();

            grid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions = new RowDefinitionCollection { new RowDefinition() },
                ColumnDefinitions = new ColumnDefinitionCollection{ new ColumnDefinition(), new ColumnDefinition(), new ColumnDefinition() },
                Margin = 10
            };

            for (int i = 0; i < kategooriad.Count; i++)
            {
                int row = i / 3;
                int column = i % 3;

                Label label = new Label
                {                    
                    Text = kategooriad[i].Kategooria,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 20
                };
                labels.Add(label);

                Frame frame = new Frame
                {                   
                    BorderColor = Color.Black,
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 15,
                    WidthRequest = 120,
                    HeightRequest = 80,
                    Content = label
                };                
                frame.GestureRecognizers.Add(tap);

                grid.Children.Add(frame, column, row);
            }
            ScrollView scrollView = new ScrollView { Content = grid };
            Content = scrollView;
        }
        public GroceryPage(string zxc)
        {
            //this.zxc = zxc;
            Title = "Товары";
            Title = zxc;

            //Content = st;
        }
        public async void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frm = (Frame)sender;
            if (grid.Children.Last() == frm)
            {
                await Navigation.PushAsync(new ListPage());
            }
            else
            {                
                await Navigation.PushAsync(new GroceryPage(labels[frm.TabIndex].Text));
            }            
        }

        public void FileOutput()
        {
            //Создание файлов
            //File.WriteAllText(Path.Combine(folderPath, fileNames[0]), "1,Овощи,vegetables.png"); //Категория
            //File.WriteAllText(Path.Combine(folderPath, fileNames[1]), "1,Картофель,potato.png,1"); //Товар

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
            kategooriad.Add(new Kategooriad(0, "+", "test"));
        }
    }
}

/*
 * Заменить цифры на картинки
 * Переделать страницу в общий класс
 */