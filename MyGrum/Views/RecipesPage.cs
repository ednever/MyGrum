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
    public partial class RecipesPage : ContentPage
    {
        string[] fileNames = { "Soogiajad.txt", "Retseptid.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        List<Soogiajad> soogiajad = new List<Soogiajad>();
        List<Retseptid> retseptid = new List<Retseptid>();
        List<Retseptid> retseptidUhesSoogiajas = new List<Retseptid>();

        Grid grid;
        List<Image> images = new List<Image>();
        TapGestureRecognizer tap = new TapGestureRecognizer();
        PinchGestureRecognizer pinch = new PinchGestureRecognizer();

        DateTime lastTapTime = DateTime.MinValue;
        const int DoubleTapMilliseconds = 300;

        bool isPageInModeSoogiajad;
        bool isFirstLoad = true;
        int soogiaegID;

        public RecipesPage(string pealkiri, bool isPageInModeSoogiajad, int soogiaegID)
        {
            Title = pealkiri;
            this.isPageInModeSoogiajad = isPageInModeSoogiajad;
            this.soogiaegID = soogiaegID;

            tap.Tapped += Tap_Tapped;
            pinch.PinchUpdated += OnPinchUpdated;
        }
        async void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frm = (Frame)sender;

            if (isPageInModeSoogiajad)
            {
                if (grid.Children.Last() == frm)
                {
                    if (frm.TabIndex == 0)
                        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, 0, 0, true));
                    else
                        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, soogiajad[frm.TabIndex - 1].SoogiaegID, 0, true));
                }
                else
                {
                    soogiaegID = frm.TabIndex;
                    await Navigation.PushAsync(new RecipesPage(images[frm.TabIndex].AutomationId, false, soogiajad[frm.TabIndex].SoogiaegID));
                }
            }
            else
            {
                if (grid.Children.Last() == frm)
                {
                    if (frm.TabIndex == 0)
                        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, 0, soogiaegID, true));                    
                    else
                        await Navigation.PushAsync(new AddingPage(isPageInModeSoogiajad, retseptid[frm.TabIndex - 1].RetseptID, soogiaegID, true));
                }
                else
                {
                    await Navigation.PushAsync(new RecipeDescriptionPage(retseptid[frm.TabIndex].RetseptID, retseptid[frm.TabIndex].Retsept, images[frm.TabIndex].Source, retseptid[frm.TabIndex].Nimekiri, retseptid[frm.TabIndex].Kirjeldus));
                }
            }
        }
        void FileOutput(bool isPageInModeSoogiajad)
        {
            //Очистка файлов
            //File.Delete(Path.Combine(folderPath, fileNames[0]));
            //File.Delete(Path.Combine(folderPath, fileNames[1]));

            if (!File.Exists(Path.Combine(folderPath, fileNames[0])))
                File.WriteAllText(Path.Combine(folderPath, fileNames[0]), "1,Завтрак,breakfast.jpg"); //Приём пищи
            

            if (!File.Exists(Path.Combine(folderPath, fileNames[1])))
                File.WriteAllText(Path.Combine(folderPath, fileNames[1]), "1,Овсянка,oatmeal.jpg,1,Овсяные хлопья - 200 г#Изюм - 1 ст. л.#Орехи - 1 ст. л.#Корица - добавить по вкусу#Банан - 1 шт#Мёд - 1 ст. л.#Горячая вода - 100 мл,..."); //Рецепт
            

            if (isPageInModeSoogiajad)
            {
                if (String.IsNullOrEmpty(fileNames[0])) return;
                if (fileNames[0] != null)
                {
                    String[] Andmed = File.ReadAllLines(Path.Combine(folderPath, fileNames[0]));
                    for (int i = 0; i < Andmed.Length; i++)
                    {
                        var columns = Andmed[i].Split(',');
                        Soogiajad soogiaeg = new Soogiajad(int.Parse(columns[0]), columns[1], columns[2]);
                        soogiajad.Add(soogiaeg);
                    }
                }
                soogiajad.Add(new Soogiajad(0, "+", "plus.png"));
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
                        Retseptid retsept = new Retseptid(int.Parse(columns[0]), columns[1], columns[2], int.Parse(columns[3]), columns[4], columns[5]);
                        retseptid.Add(retsept);
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
                soogiajad.Clear();
                retseptid.Clear();
                retseptidUhesSoogiajas.Clear();

                ProrisovkaStranitsi();
            }
        }
        void ProrisovkaStranitsi()
        {
            FileOutput(isPageInModeSoogiajad);

            grid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions = new RowDefinitionCollection { new RowDefinition() },
                ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition(), new ColumnDefinition() },
                Margin = 20
            };

            if (isPageInModeSoogiajad)
            {
                for (int i = 0; i < soogiajad.Count; i++)
                {
                    int row = i / 2;
                    int column = i % 2;

                    Image image = new Image
                    {
                        AutomationId = soogiajad[i].Soogiaeg,
                        Source = ImageSource.FromFile(Path.Combine(FileSystem.AppDataDirectory, soogiajad[i].Pilt)),
                        Aspect = Aspect.AspectFill,
                        Margin = -19
                    };
                    images.Add(image);

                    Frame frame = new Frame
                    {
                        TabIndex = i,
                        BorderColor = Color.Black,
                        CornerRadius = 30,
                        HeightRequest = 140,
                        Margin = new Thickness(5,10,5,10),
                        Content = image,
                        GestureRecognizers = { tap, pinch }
                    };

                    grid.Children.Add(frame, column, row);
                }
            }
            else
            {
                foreach (var retsept in retseptid)
                {
                    if (retsept.SoogiaegID == soogiaegID)
                    {
                        retseptidUhesSoogiajas.Add(retsept);
                    }
                }
                retseptidUhesSoogiajas.Add(new Retseptid(0, "+", "plus.png", 0, "+", "+"));

                for (int i = 0; i < retseptidUhesSoogiajas.Count; i++)
                {
                    int row = i / 2;
                    int column = i % 2;

                    Image image = new Image
                    {
                        AutomationId = retseptidUhesSoogiajas[i].Retsept,
                        Source = ImageSource.FromFile(Path.Combine(FileSystem.AppDataDirectory, retseptidUhesSoogiajas[i].Pilt)),
                        Aspect = Aspect.AspectFill,
                        Margin = -19
                    };
                    images.Add(image);

                    Frame frame = new Frame
                    {
                        TabIndex = i,
                        BorderColor = Color.Black,
                        CornerRadius = 30,
                        HeightRequest = 140,
                        Margin = new Thickness(5, 10, 5, 10),
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
                    if (isPageInModeSoogiajad)
                        await Navigation.PushAsync(new UpdatingPage(soogiajad[frm.TabIndex].Soogiaeg, images[frm.TabIndex].Source, isPageInModeSoogiajad, soogiajad[frm.TabIndex].SoogiaegID, 0, true, null, null));                   
                    else
                        await Navigation.PushAsync(new UpdatingPage(retseptidUhesSoogiajas[frm.TabIndex].Retsept, images[frm.TabIndex].Source, isPageInModeSoogiajad, retseptidUhesSoogiajas[frm.TabIndex].RetseptID, soogiaegID, true, retseptidUhesSoogiajas[frm.TabIndex].Nimekiri, retseptidUhesSoogiajas[frm.TabIndex].Kirjeldus));                   
                }
                lastTapTime = DateTime.Now;
            }
        }
    }
}
/*
 * У картинки должна быть чёрная немного прозрачная линия с названием картинки
 */