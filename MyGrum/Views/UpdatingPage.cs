using System;
using System.IO;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatingPage : ContentPage
    {
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt", "Soogiajad.txt", "Retseptid.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Image image;
        Entry entry;
        bool isPageInModeClassOrSubclass, isPageRecipesPageOrGroceryPage;
        int num, classID;

        public UpdatingPage(string pealkiri, ImageSource imageSource, bool isPageInModeClassOrSubclass, int num, int classID, bool isPageRecipesPageOrGroceryPage)
        {
            this.isPageInModeClassOrSubclass = isPageInModeClassOrSubclass;
            this.num = num;
            this.classID = classID;
            this.isPageRecipesPageOrGroceryPage = isPageRecipesPageOrGroceryPage;

            Title = pealkiri;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            image = new Image { Source = imageSource, Aspect = Aspect.AspectFill, Margin = -19 };
            Frame frame = new Frame
            {
                BorderColor = Color.Black,
                WidthRequest = 200,
                HeightRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20, 0, 0),
                Content = image
            };
            frame.GestureRecognizers.Add(tap);

            Label label = new Label 
            { 
                Text = "Название", 
                FontSize = 25, 
                FontAttributes = FontAttributes.Bold, 
                Margin = new Thickness(20, 20, 0, 0) 
            };
            entry = new Entry 
            { 
                Placeholder = "Введите текст", 
                Text = pealkiri, 
                Margin = new Thickness(20, 0, 20, 0), 
                MaxLength = 20 
            };
            Button button = new Button { Text = "Сохранить", Margin = new Thickness(0, 20, 20, 0) };
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
                    });
                }
            });
        }
        async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entry.Text))
            {
                await DisplayAlert("Ошибка", "Заполните все поля!", "Ок");
            }
            else
            {
                string textToFile = "\n" + num.ToString() + "," + entry.Text + "," + Path.GetFileName(image.Source.ToString());
                int fileNumber = 0;

                if (isPageRecipesPageOrGroceryPage && isPageInModeClassOrSubclass)
                {
                    fileNumber = 2;
                }
                else if (isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                {
                    fileNumber = 3;
                    textToFile += "," + classID.ToString() + "," + "...";
                }
                else if (!isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                {
                    fileNumber = 1;
                    textToFile += "," + classID.ToString();
                }



                string[] lines = File.ReadAllLines(Path.Combine(folderPath, fileNames[fileNumber]));
                lines[num] = textToFile;
                //File.WriteAllLines(Path.Combine(folderPath, fileNames[fileNumber]), lines);

                await Navigation.PopAsync();
            }
        }
    }
}
