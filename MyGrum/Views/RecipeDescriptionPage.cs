using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeDescriptionPage : ContentPage
    {
        public RecipeDescriptionPage(string pealkiri, ImageSource imageSource)
        {
            Title = pealkiri;
            Image image = new Image
            {
                Source = imageSource,
                Aspect = Aspect.AspectFill,
                Margin = -19
            };
            Frame frame = new Frame 
            {
                BorderColor = Color.Black,
                CornerRadius = 30,
                HeightRequest = 300,
                Content = image
            };
            Label pealkiri2_1 = new Label { 
                Text = "Необходимые ингредиенты", 
                FontSize = 25, 
                FontAttributes = FontAttributes.Bold, 
                Margin = new Thickness(0, 20, 0, 0) 
            };
            Label vajalikudTooted = new Label
            {
                Margin = new Thickness(0, 20, 0, 0),
                FontSize = 16,               
            };
            Label pealkiri2_2 = new Label
            {
                Text = "Рецепт приготовления",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 20, 0, 0)
            };
            Label retseptiValmistamine = new Label
            {
                Margin = new Thickness(0, 20, 0, 0),
                FontSize = 16,
            };
            StackLayout st = new StackLayout { Children = { frame, pealkiri2_1, vajalikudTooted, pealkiri2_2, retseptiValmistamine }, Margin = 20 };
            ScrollView scrollView = new ScrollView { Content = st };
            Content = scrollView;
        }
    }
}
/*
 * Изменить класс рецепты добавив новое поле - Необходимые ингредиенты
 * При нажатии на vajalikudTooted и retseptiValmistamine появляется возможность редактировать в нём текст
 * При изменении рецепта записывать данные в файл
 */