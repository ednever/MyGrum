using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using Xamarin.Forms.Xaml;
using System;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Classes : ContentPage
    {
        Image imageControl;
        string imagePath;

        public Classes() 
        {
            imageControl = new Image { Source = ImageSource.FromFile(Path.Combine(FileSystem.AppDataDirectory, "IMG_20230523_104101.jpg" )) };

            //SelectImageFromDevice();

            Content = imageControl;           
        }
        async public void SelectImageFromDevice()
        {
            var pickResultTask = FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Выберите изображение"
            });

            await pickResultTask.ContinueWith(t =>
            {
                var pickResult = t.Result;

                if (pickResult != null)
                {
                    // Сохранение изображения во внутреннее хранилище приложения
                    imagePath = Path.Combine(FileSystem.AppDataDirectory, pickResult.FileName);

                    using (var stream = pickResult.OpenReadAsync().Result)
                    {
                        using (var fileStream = File.OpenWrite(imagePath))
                        {
                            stream.CopyToAsync(fileStream).Wait();
                        }
                    }

                    //imageControl.Source = ImageSource.FromFile(imagePath);

                    // Отображение изображения в элементе управления Image
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var imageSource = ImageSource.FromFile(imagePath);
                        imageControl.Source = imageSource;
                    });
                }
            });
        }
    }
}