using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using MyGrum.Models;
using System.IO;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        Label label;
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        List<Tooted> tooted = new List<Tooted>();
        Grid grid;
        public ListPage()
        {
            Title = "Список";
            label = new Label();

            grid = new Grid
            {
                //VerticalOptions = LayoutOptions.Start,
                ColumnDefinitions = new ColumnDefinitionCollection 
                { 
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }, 
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                Margin = 10
            };
            ScrollView scrollView = new ScrollView { Content = grid };
            Content = scrollView;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


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

            for (int i = 0; i < tooted.Count; i++)
            {
                if (Preferences.ContainsKey(i.ToString()))
                {
                   

                    grid.Children.Add(new CheckBox(), 0, i);
                    grid.Children.Add(new Label { Text = Preferences.Get(i.ToString(), "Pole andmed") }, 1, i);
                    //label.Text += "\n" + Preferences.Get(i.ToString(), "Pole andmed");
                    //Preferences.Clear();
                }
            }

            
            //Preferences.
                     
        }
    }  
}
//дочинить отображение списка