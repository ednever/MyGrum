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
        List<Label> labels = new List<Label>();
        List<CheckBox> boxes = new List<CheckBox>();
        List<bool> bools = new List<bool>();
        StackLayout st;
        public ListPage()
        {
            Title = "Список";

            st = new StackLayout { Margin = new Thickness(20, 20, 0, 0) };
            Button button = new Button { Text = "Очистить список", Margin = new Thickness(0, 0, 20, 20) };
            button.Clicked += Button_Clicked;
            StackLayout st1 = new StackLayout { Children = { button }, HorizontalOptions = LayoutOptions.End };
            StackLayout st2 = new StackLayout { Children = { st, st1 } };
            ScrollView scrollView = new ScrollView { Content = st2 };
            Content = scrollView;
        }
        void Button_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            st.Children.Clear();
            bools.Clear();
        }
        void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.IsChecked)
            {
                bools[box.TabIndex] = true;
                labels[box.TabIndex].TextDecorations = TextDecorations.Strikethrough;
                labels[box.TabIndex].TextColor = Color.Gray;
            }
            else
            {
                bools[box.TabIndex] = false;
                labels[box.TabIndex].TextDecorations = TextDecorations.None;
                labels[box.TabIndex].TextColor = Color.Black;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            st.Children.Clear();
            labels.Clear();
            boxes.Clear();

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

            for (int i = 1; i < tooted.Count + 1; i++)
            {
                if (Preferences.ContainsKey(i.ToString()))
                {                    
                    Label label = new Label 
                    { 
                        TextColor = Color.Black, 
                        Text = Preferences.Get(i.ToString(), "Нет данных"), 
                        Margin = new Thickness(0, 5, 0, 0) 
                    };
                    labels.Add(label);
                    
                    CheckBox checkBox = new CheckBox { TabIndex = i - 1, AutomationId = label.Text };
                    
                    checkBox.CheckedChanged += CheckBox_CheckedChanged;
                    boxes.Add(checkBox);
                    bools.Add(false);


                    StackLayout st1 = new StackLayout 
                    {
                        Children = { checkBox, label }, 
                        Orientation = StackOrientation.Horizontal 
                    };
                    st.Children.Add(st1);
                }
            }
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].IsChecked = bools[i];
            }
        }       
        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    //foreach (CheckBox box in boxes)
        //    //{
        //    //    bools.Add(box.IsChecked);
        //    //}
        //}
    }  
}
//Если приложение закрывается, то отметка флажков забывается