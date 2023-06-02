using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;
using MyGrum.Models;
using System.Threading.Tasks;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingPage : ContentPage
    {
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt", "Soogiajad.txt", "Retseptid.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Image image;
        Entry entry;
        bool isPageInModeClassOrSubclass;
        bool isPageRecipesPageOrGroceryPage;
        string newImageName;
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


            image = new Image { Source = ImageSource.FromFile("plus.png"), Aspect = Aspect.AspectFit };
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
        
            Label label = new Label { Text = "Название", FontSize = 25, FontAttributes = FontAttributes.Bold, Margin = new Thickness(20, 20, 0, 0) };
            entry = new Entry { Placeholder = "Введите текст", Margin = new Thickness(20, 0, 20, 0) };
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
                        newImageName = pickResult.FileName;
                        image.Source = ImageSource.FromFile(imagePath);
                        image.Margin = -19;
                        image.Aspect = Aspect.AspectFill;
                    });
                }
            });
        }
        async void Button_Clicked(object sender, EventArgs e)
        {
            string polpo;
            if (string.IsNullOrWhiteSpace(entry.Text) || image.Aspect == Aspect.AspectFit)
            {
                await DisplayAlert("Ошибка", "Заполните все поля!", "Ок");
            }
            else
            {
                //if (isPageRecipesPageOrGroceryPage)
                //{
                //    if (isPageInModeClassOrSubclass)
                //    {
                //        if (File.Exists(Path.Combine(folderPath, fileNames[2]))) //Приём пищи
                //        {
                //            File.AppendAllText(Path.Combine(folderPath, fileNames[2]), "\n" + num.ToString() + "," + entry.Text + "," + newImageName); //число,название,картинка
                //        }
                //    }
                //    else
                //    {
                //        if (File.Exists(Path.Combine(folderPath, fileNames[3]))) //Рецепт
                //        {
                //            File.AppendAllText(Path.Combine(folderPath, fileNames[3]), "\n" + num.ToString() + "," + entry.Text + "," + newImageName + "," + classID + "," + "..."); //число,название,картинка,класс,описание                 
                //        }
                //    }
                //}
                //else 
                //{                    
                //    if (isPageInModeClassOrSubclass)
                //    {
                //        if (File.Exists(Path.Combine(folderPath, fileNames[0]))) //Категория
                //        {
                //            File.AppendAllText(Path.Combine(folderPath, fileNames[0]), "\n" + num.ToString() + "," + entry.Text + "," + newImageName); //число,название,картинка
                //        }
                //    }
                //    else
                //    {
                //        if (File.Exists(Path.Combine(folderPath, fileNames[1]))) //Товар
                //        {
                //            File.AppendAllText(Path.Combine(folderPath, fileNames[1]), "\n" + num.ToString() + "," + entry.Text + "," + newImageName + "," + classID); //число,название,картинка,категория                 
                //        }
                //    }

                //}

                if (isPageRecipesPageOrGroceryPage)
                {
                    if (isPageInModeClassOrSubclass)
                    {
                        polpo = "\n" + num.ToString() + "," + entry.Text + "," + newImageName;
                    }
                    else
                    {
                        polpo = "\n" + num.ToString() + "," + entry.Text + "," + newImageName + "," + classID + "," + "...";
                    }
                }
                else
                {
                    if (isPageInModeClassOrSubclass)
                    {
                        polpo = "\n" + num.ToString() + "," + entry.Text + "," + newImageName;
                    }
                    else
                    {
                        polpo = "\n" + num.ToString() + "," + entry.Text + "," + newImageName + "," + classID;
                    }
                }
                for (int i = 0; i < fileNames.Length; i++)
                {
                    if (File.Exists(Path.Combine(folderPath, fileNames[i])))
                    {
                        File.AppendAllText(Path.Combine(folderPath, fileNames[i]), polpo);
                    }
                }
                await Navigation.PopAsync();
            }
        }
    }
}