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
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        List<Kategooriad> kategooriad = new List<Kategooriad>();
        List<Tooted> tooted = new List<Tooted>();

        Grid grid;
        List<Label> labels = new List<Label>();
        TapGestureRecognizer tap = new TapGestureRecognizer();

        public GroceryPage(string pealkiri, bool kvst)
        {
            Title = pealkiri;

            tap.Tapped += Tap_Tapped;
            FileOutput(kvst);

            grid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions = new RowDefinitionCollection { new RowDefinition() },
                ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition(), new ColumnDefinition(), new ColumnDefinition() },
                Margin = 10
            };
            if (kvst)
            {
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
            }
            else
            {
                for (int i = 0; i < tooted.Count; i++)
                {
                    int row = i / 3;
                    int column = i % 3;

                    Label label = new Label
                    {
                        Text = tooted[i].Toote,
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
            }
            
            ScrollView scrollView = new ScrollView { Content = grid };
            Content = scrollView;
        }
        public async void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frm = (Frame)sender;
            if (grid.Children.Last() == frm)
            {
                await Navigation.PushAsync(new AddingPage());
            }
            else
            {                
                await Navigation.PushAsync(new GroceryPage(labels[frm.TabIndex].Text, false));
            }            
        }
        public void FileOutput(bool kvst)
        {
            //Создание файлов
            //File.WriteAllText(Path.Combine(folderPath, fileNames[0]), "1,Овощи,vegetables.png"); //Категория
            //File.WriteAllText(Path.Combine(folderPath, fileNames[1]), "1,Картофель,potato.png,1"); //Товар
            if (kvst)
            {
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
            else
            {
                if (String.IsNullOrEmpty(fileNames[1])) return;
                if (fileNames[1] != null)
                {
                    String[] Andmed = File.ReadAllLines(Path.Combine(folderPath, fileNames[1]));
                    for (int i = 0; i < Andmed.Length; i++)
                    {
                        var columns = Andmed[i].Split(',');
                        Tooted toote = new Tooted(int.Parse(columns[0]), columns[1], columns[2], int.Parse(columns[3]));
                        tooted.Add(toote);
                    }
                }
                tooted.Add(new Tooted(0, "+", "test", 0));
            }
        }
    }
}

/*
 * Заменить цифры на картинки
 * Переделать страницу в общий класс
 */