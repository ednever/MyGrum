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
        string[] fileNames = { "Kategooriad.txt", "Tooted.txt" };
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        List<Tooted> tooted = new List<Tooted>();
        StackLayout st;
        public ListPage()
        {
            Title = "Список";

            st = new StackLayout { Margin = new Thickness(20, 20, 0, 0) };
            StackLayout st1 = new StackLayout { 
                Children = { new Button { Text = "Очистить список", Margin = new Thickness(0,0,20,20) } },
                HorizontalOptions = LayoutOptions.End
            };
            StackLayout st2 = new StackLayout { Children = { st, st1 } };
            ScrollView scrollView = new ScrollView { Content = st2 };
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
                    StackLayout st1 = new StackLayout 
                    { 
                        Children = { new CheckBox(), new Label { Text = Preferences.Get(i.ToString(), "Pole andmed"), Margin = new Thickness(0,5,0,0) } }, 
                        Orientation = StackOrientation.Horizontal 
                    };
                    st.Children.Add(st1);

                    //label.Text += "\n" + Preferences.Get(i.ToString(), "Pole andmed");
                    Preferences.Clear();
                }
            }
        }
    }  
}