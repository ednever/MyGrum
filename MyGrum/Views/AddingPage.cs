using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingPage : ContentPage
    {
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Image image;
        Entry entry;
        bool test;
        public AddingPage(bool kvst)
        {
            this.test = kvst;
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            if (test)
                Title = "Добавление категории";          
            else
                Title = "Добавление товара";

            image = new Image 
            { 
                Source = ImageSource.FromFile("plus.png"),
                Aspect = Aspect.AspectFit
            };
            Frame frame = new Frame 
            {
                BorderColor = Color.Black,
                WidthRequest = 200,
                HeightRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,10,0,0),
                Content = image 
            };
            frame.GestureRecognizers.Add(tap);
        
            Label label = new Label { Text = "Название", FontSize = 25, FontAttributes = FontAttributes.Bold };
            entry = new Entry { Placeholder = "Введите текст" };
            Button button = new Button { Text = "Сохранить" };


            StackLayout st2 = new StackLayout { };
            StackLayout st1 = new StackLayout 
            { 
                HorizontalOptions = LayoutOptions.End,
                Children = { button } 
            };
            StackLayout st = new StackLayout { Children = { frame, label, entry, st1 } };
            Content = st;
        }

        async void Tap_Tapped(object sender, EventArgs e)
        {
            var pickResult = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Выберите изображение"
            });

            if (pickResult != null)
            {
                image.Source = ImageSource.FromFile(pickResult.FullPath);
                image.Margin = -20;                
            }            
        }

        void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (File.Exists(Path.Combine(folderPath, fileNames[0])))
            {
                File.AppendAllText(Path.Combine(folderPath, fileNames[0]), "\n" + "" + "," + entry.Text + "," + ""); //Доработать
            }
        }
    }
}

/**
 * Порядковый номер вместо кавычек
 * Название картинки из её полного путя вместо кавычек
 * Другая версия страницы для товаров
 */