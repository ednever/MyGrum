using System;
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
        Image imageView;
        public AddingPage(bool kvst)
        {
            if (kvst)
                Title = "Добавление категории";          
            else
                Title = "Добавление товара";

            Button selectImageButton = new Button
            {
                Text = "Выбрать изображение"
            };

            imageView = new Image
            {
                Aspect = Aspect.AspectFit
            };

            selectImageButton.Clicked += Button_Clicked;

            Label label = new Label { Text= "Название", FontSize = 40 };



            Entry entry = new Entry
            {
                Placeholder = "Введите текст"
            };

            Button button = new Button { Text = "Сохранить" };

            // Добавьте кнопку и ImageView в вашу пользовательскую разметку или контейнер
            // Например, в StackLayout:
            StackLayout layout = new StackLayout { Children = { imageView, selectImageButton, label, entry } };
            Content= layout;
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "Выбрать изображение")
            {
                var pickResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Выберите изображение"
                });

                if (pickResult != null)
                {
                    // Получите путь к выбранному изображению
                    string imagePath = pickResult.FullPath;

                    // Установите источник изображения для ImageView
                    imageView.Source = ImageSource.FromFile(imagePath);
                }
            }
            else
            {

            }
        }
    }
}