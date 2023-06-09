﻿using System;
using System.IO;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.Generic;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingPage : ContentPage
    {
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt", "Soogiajad.txt", "Retseptid.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Image image;
        Entry entry;
        bool isPageInModeClassOrSubclass, isPageRecipesPageOrGroceryPage;
        int num, classID;

        public AddingPage(bool isPageInModeClassOrSubclass, int num, int classID, bool isPageRecipesPageOrGroceryPage)
        {
            this.isPageInModeClassOrSubclass = isPageInModeClassOrSubclass;
            this.num = num;
            this.classID = classID;
            this.isPageRecipesPageOrGroceryPage = isPageRecipesPageOrGroceryPage;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            if (isPageRecipesPageOrGroceryPage)
            {
                if (isPageInModeClassOrSubclass)
                    Title = "Добавление приёма пищи";
                else
                    Title = "Добавление рецепта";
            }
            else
            {
                if (isPageInModeClassOrSubclass)
                    Title = "Добавление категории";
                else
                    Title = "Добавление товара";
            }


            image = new Image { Source = ImageSource.FromFile("plus.png") };
            Frame frame = new Frame 
            {
                BorderColor = Color.Black,
                WidthRequest = 200,
                HeightRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,20,0,0),
                Content = image
            };
            frame.GestureRecognizers.Add(tap);
        
            Label label = new Label 
            {
                TextColor = Color.Black,
                Text = "Название", 
                FontSize = 25, 
                FontAttributes = FontAttributes.Bold, 
                Margin = new Thickness(20, 20, 0, 0) 
            };
            entry = new Entry 
            {
                TextColor = Color.Black,
                PlaceholderColor = Color.Black,
                Placeholder = "Введите текст",               
                Margin = new Thickness(20, 0, 20, 0), 
                MaxLength = 20 
            };
            Button button = new Button { Text = "Сохранить", Margin = new Thickness(0,20,20,0) };
            button.Clicked += Button_Clicked;

            StackLayout st1 = new StackLayout { HorizontalOptions = LayoutOptions.End, Children = { button } };
            StackLayout st = new StackLayout { Children = { frame, label, entry, st1 } };
            Content = st;
        }
        async void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frm = (Frame)sender;
            var pickResultTask = FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images });

            await pickResultTask.ContinueWith(t =>
            {
                var pickResult = t.Result;

                if (pickResult != null)
                {
                    // Сохранение изображения во внутреннее хранилище приложения
                    var imagePath = Path.Combine(FileSystem.AppDataDirectory, pickResult.FileName);

                    using (var stream = pickResult.OpenReadAsync().Result)
                    {
                        using (var fileStream = File.OpenWrite(imagePath))
                        {
                            stream.CopyToAsync(fileStream).Wait();
                        }
                    }

                    // Отображение изображения в элементе управления Image
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        image.Source = ImageSource.FromFile(imagePath);
                        image.Margin = -19;
                        image.Aspect = Aspect.AspectFill;
                    });
                }
            });
        }
        async void Button_Clicked(object sender, EventArgs e)
        {            
            if (string.IsNullOrWhiteSpace(entry.Text) || image.Aspect == Aspect.AspectFit)
            {
                await DisplayAlert("Ошибка", "Заполните все поля!", "Ок");
            }
            else
            {
                string textToFile = (num + 1).ToString() + "," + entry.Text + "," + Path.GetFileName(image.Source.ToString());
                int fileNumber = 0;

                if (isPageRecipesPageOrGroceryPage && isPageInModeClassOrSubclass)
                {
                    fileNumber = 2;
                }
                else if (isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                {
                    fileNumber = 3;
                    textToFile += "," + classID.ToString() + "," + "...,..."; 
                }
                else if(!isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                {
                    fileNumber = 1;
                    textToFile += "," + classID.ToString();
                }

                string[] lines = File.ReadAllLines(Path.Combine(folderPath, fileNames[fileNumber]));
                List<string> test = new List<string>();
                foreach (var item in lines)
                {
                    test.Add(item);
                }
                test.Add(textToFile);
                string[] newlines = test.ToArray();

                //string proverka = ""; //test
                //foreach (var item in newlines)
                //{
                //    proverka += "\n" + item.ToString();
                //}
                //await DisplayAlert("TEST",proverka,"OK");

                if (File.Exists(Path.Combine(folderPath, fileNames[fileNumber])))
                    File.WriteAllLines(Path.Combine(folderPath, fileNames[fileNumber]), newlines);

                await Navigation.PopAsync();
            }
        }
    }
}