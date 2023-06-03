using MyGrum.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyGrum
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            string[,] pealkirjad = { {"Продукты", "page1.png"}, {"Список", "page2.png"}, {"Рецепты", "page3.png"} };
            List<ContentPage> pages = new List<ContentPage> { new GroceryPage("Категории", true, 0), new ListPage(), new RecipesPage("Приёмы пищи", true, 0) };

            TabBar tabBar = new TabBar();

            for (int i = 0; i < pealkirjad.Length / 2; i++)
            {
                tabBar.Items.Add(new ShellContent { Title = pealkirjad[i, i * 0], Icon = pealkirjad[i, i * 0 + 1], Content = pages[i] });
            }

            Items.Add(tabBar);
        }
    }
}
/*
 * Приложение некорректно работает с ночной темой на устройстве
 * Добавить кнопку как пользоваться приложением, если тестировка пройдёт безуспешно
 * Добавить функции удаления данных
 * Доделать изменение данных
 */