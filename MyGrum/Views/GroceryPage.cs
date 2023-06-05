using MyGrum.Models;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroceryPage : ContentPage
    {
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        List<Kategooriad> kategooriad = new List<Kategooriad>();
        List<Tooted> tooted = new List<Tooted>();
        List<Tooted> tootedUheKategooriaga = new List<Tooted>();

        Grid grid;
        List<Image> images = new List<Image>();
        TapGestureRecognizer tap = new TapGestureRecognizer();
        PinchGestureRecognizer pinch = new PinchGestureRecognizer();

        DateTime lastTapTime = DateTime.MinValue;
        const int DoubleTapMilliseconds = 300;

        bool test;
        bool isFirstLoad = true;
        int katID;

        public GroceryPage(string pealkiri, bool kvst, int katID)
        {
            Title = pealkiri;
            this.test = kvst;
            this.katID = katID;

            tap.Tapped += Tap_Tapped;
            pinch.PinchUpdated += OnPinchUpdated;
        }
        async void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frm = (Frame)sender;

            if (test)
            {
                if (grid.Children.Last() == frm)
                {
                    if (frm.TabIndex == 0)
                        await Navigation.PushAsync(new AddingPage(test, 0, 0, false));                    
                    else
                        await Navigation.PushAsync(new AddingPage(test, kategooriad[frm.TabIndex - 1].KategooriaID, 0, false));                                        
                }
                else
                {
                    katID = frm.TabIndex;
                    await Navigation.PushAsync(new GroceryPage(images[frm.TabIndex].AutomationId, false, kategooriad[frm.TabIndex].KategooriaID));
                }              
            }
            else
            {
                if (grid.Children.Last() == frm)
                {
                    if (frm.TabIndex == 0)
                        await Navigation.PushAsync(new AddingPage(test, 0, katID, false));
                    else
                        await Navigation.PushAsync(new AddingPage(test, tooted[frm.TabIndex - 1].TooteID, katID, false));
                }
                else
                {
                    if (frm.Opacity == 1)
                    {
                        frm.Opacity = 0.5;
                        Preferences.Set(frm.TabIndex.ToString(), images[frm.TabIndex].AutomationId);
                    }
                    else
                    {
                        frm.Opacity = 1;
                        Preferences.Remove(frm.TabIndex.ToString());
                    }
                }
            }           
        }
        void FileOutput(bool kvst)
        {
            //Очистка файлов
            //File.Delete(Path.Combine(folderPath, fileNames[0]));
            //File.Delete(Path.Combine(folderPath, fileNames[1]));

            if (!File.Exists(Path.Combine(folderPath, fileNames[0])))
                File.WriteAllText(Path.Combine(folderPath, fileNames[0]), "1,Овощи,vegetables.png"); //Категория
            

            if (!File.Exists(Path.Combine(folderPath, fileNames[1])))
                File.WriteAllText(Path.Combine(folderPath, fileNames[1]), "1,Картофель,potato.png,1"); //Товар
            

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
                kategooriad.Add(new Kategooriad(0, "+", "plus.png"));
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
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (isFirstLoad)
            {
                ProrisovkaStranitsi();
                isFirstLoad = false;
            }
            else 
            {
                grid.Children.Clear();
                images.Clear();
                kategooriad.Clear();
                tooted.Clear();
                tootedUheKategooriaga.Clear();

                ProrisovkaStranitsi();
            }
        }
        void ProrisovkaStranitsi()
        {
            FileOutput(test);
            grid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions = new RowDefinitionCollection { new RowDefinition() },
                ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition(), new ColumnDefinition(), new ColumnDefinition() },
                Margin = 10
            };
            
            if (test)
            {
                for (int i = 0; i < kategooriad.Count; i++)
                {
                    int row = i / 3;
                    int column = i % 3;

                    Image image = new Image
                    {
                        AutomationId = kategooriad[i].Kategooria,
                        Source = ImageSource.FromFile(Path.Combine(FileSystem.AppDataDirectory, kategooriad[i].Pilt)),
                        Aspect = Aspect.AspectFill,
                        Margin = -19
                    };
                    images.Add(image);

                    Frame frame = new Frame
                    {
                        TabIndex = i,
                        BorderColor = Color.Black,
                        CornerRadius = 15,
                        HeightRequest = 80,
                        Content = image,
                        GestureRecognizers = { tap, pinch }
                    };

                    grid.Children.Add(frame, column, row);
                }
            }
            else
            {
                foreach (var toote in tooted)
                {
                    if (toote.KategooriaID == katID)
                    {
                        tootedUheKategooriaga.Add(toote);
                    }
                }
                tootedUheKategooriaga.Add(new Tooted(0, "+", "plus.png", 0));

                for (int i = 0; i < tootedUheKategooriaga.Count; i++)
                {
                    int row = i / 3;
                    int column = i % 3;

                    Image image = new Image
                    {
                        AutomationId = tootedUheKategooriaga[i].Toote,
                        Source = ImageSource.FromFile(Path.Combine(FileSystem.AppDataDirectory, tootedUheKategooriaga[i].Pilt)),
                        Aspect = Aspect.AspectFill,
                        Margin = -19
                    };
                    images.Add(image);

                    Frame frame = new Frame
                    {
                        TabIndex = i,
                        BorderColor = Color.Black,
                        CornerRadius = 15,
                        HeightRequest = 80,
                        Content = image,
                        GestureRecognizers = { tap, pinch }
                    };

                    grid.Children.Add(frame, column, row);
                }
            }

            images.Last().Margin = 0;

            ScrollView scrollView = new ScrollView { Content = grid };
            Content = scrollView;
        }
        async void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            Frame frm = (Frame)sender;
            if (e.Status == GestureStatus.Completed)
            {
                if (grid.Children.Last() == frm)
                    return;
                
                if (DateTime.Now - lastTapTime < TimeSpan.FromMilliseconds(DoubleTapMilliseconds))
                {
                    if (test)
                        await Navigation.PushAsync(new UpdatingPage(kategooriad[frm.TabIndex].Kategooria, images[frm.TabIndex].Source, test, kategooriad[frm.TabIndex].KategooriaID, 0, false));                    
                    else
                        await Navigation.PushAsync(new UpdatingPage(tooted[frm.TabIndex].Toote, images[frm.TabIndex].Source, test, tooted[frm.TabIndex].TooteID, katID, false));                                        
                }                                   
                lastTapTime = DateTime.Now;
            }
        }
    }
}

/*
 * Переделать страницу в общий класс
 * Чтобы товар добавился в список необходимо посмотреть список
 * Одновременно в список можно добавить только 1 товар
 */