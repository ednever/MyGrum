﻿using MyGrum.Models;
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
        bool test;

        public GroceryPage(string pealkiri, bool kvst, int katID)
        {
            Title = pealkiri;
            this.test = kvst;

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

                    string imagePathhhh;
                    if (i == 0)
                    {
                        imagePathhhh = kategooriad[i].Pilt;
                    }
                    else
                    {
                        imagePathhhh = Path.Combine(FileSystem.AppDataDirectory, kategooriad[i].Pilt);
                    }
                    Image image = new Image
                    {
                        AutomationId = kategooriad[i].Kategooria,
                        Source = ImageSource.FromFile(imagePathhhh),
                        Aspect = Aspect.AspectFill,
                        Margin = -19,
                        HeightRequest = 118
                    };
                    

                    images.Add(image);

                    Frame frame = new Frame
                    {
                        TabIndex = i,
                        BorderColor = Color.Black,
                        BackgroundColor = Color.Transparent,                     
                        CornerRadius = 15,
                        WidthRequest = 120,
                        HeightRequest = 80,
                        Content = new StackLayout { Children = { image } }
                    };
                    frame.GestureRecognizers.Add(tap);

                    grid.Children.Add(frame, column, row);
                }
            }
            else
            {
                foreach (var toote in tooted)
                {
                    if (toote.KategooriaID == katID)
                    {
                        tootedUheKategooriaga.Insert(0, toote);
                    }
                }
                for (int i = 0; i < tootedUheKategooriaga.Count; i++)
                {
                    int row = i / 3;
                    int column = i % 3;

                    Image image = new Image
                    {
                        AutomationId = tootedUheKategooriaga[i].Toote,
                        Source = ImageSource.FromFile(tootedUheKategooriaga[i].Pilt),
                        Aspect = Aspect.AspectFill,
                        Margin = -19,
                        HeightRequest = 118
                    };
                    images.Add(image);

                    Frame frame = new Frame
                    {
                        BorderColor = Color.Black,
                        CornerRadius = 15,
                        WidthRequest = 120,
                        HeightRequest = 80,
                        Content = new StackLayout { Children = { image } }
                    };
                    frame.GestureRecognizers.Add(tap);

                    grid.Children.Add(frame, column, row);
                }
            }

            images.Last().HeightRequest = 80;
            images.Last().Margin = 0;

            tootedUheKategooriaga.Clear();

            ScrollView scrollView = new ScrollView { Content = grid };
            Content = scrollView;
        }
        public async void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frm = (Frame)sender;

            if (grid.Children.Last() == frm)
            {
                await Navigation.PushAsync(new AddingPage(test, frm.TabIndex, frm.TabIndex + 1));
            }
            else
            {
                if (test)
                {
                    await Navigation.PushAsync(new GroceryPage(images[frm.TabIndex].AutomationId, false, frm.TabIndex + 1));
                }
                else
                {
                    if (frm.Opacity == 1)
                    {
                        frm.Opacity = 0.5;
                    }
                    else
                    {
                        frm.Opacity = 1;
                        Preferences.Set("1", images[frm.TabIndex].AutomationId);
                    }
                }
            }
        }
        public void FileOutput(bool kvst)
        {
            //Очистка файлов
            //File.Delete(Path.Combine(folderPath, fileNames[0]));
            //File.Delete(Path.Combine(folderPath, fileNames[1]));
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
                tooted.Add(new Tooted(0, "+", "plus.png", 0));
                tootedUheKategooriaga.Add(new Tooted(0, "+", "plus.png", 0));
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //this.Content= null;
        }
    }
}

/*
 * Переделать страницу в общий класс
 * 
 * При добавлении нового товара он помещается в первую категорию - 50%
 * При обновлении страницы ничего не обновляется
 */