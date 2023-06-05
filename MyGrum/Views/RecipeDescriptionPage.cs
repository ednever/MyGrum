using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeDescriptionPage : ContentPage
    {
        List<string> texts = new List<string> { "Необходимые ингредиенты", "Рецепт приготовления" };        
        List<Label> labels = new List<Label>();
        List<AutoResizingEditor> editors = new List<AutoResizingEditor>();
        List<Button> buttons = new List<Button>();
        int ID;

        public RecipeDescriptionPage(int ID, string pealkiri, ImageSource imageSource, string nimekiri, string kirjeldus)
        {
            this.ID = ID;
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;
            texts.Add(nimekiri);
            texts.Add(kirjeldus);

            Title = pealkiri;

            Frame frame = new Frame 
            {
                BorderColor = Color.Black,
                CornerRadius = 30,
                HeightRequest = 300,
                Content = new Image { Source = imageSource, Aspect = Aspect.AspectFill, Margin = -19 }
            };

            StackLayout st = new StackLayout { Children = { frame }, Margin = 20 };
            for (int i = 0; i < 2; i++)
            {
                Label pealkiriLabel = new Label
                {
                    Text = texts[i],
                    FontSize = 25,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                Label sisuLabel = new Label 
                {
                    Margin = new Thickness(0, 20, 0, 20),
                    FontSize = 16,
                    GestureRecognizers = { tap }
                };
                labels.Add(sisuLabel);

                AutoResizingEditor editor = new AutoResizingEditor { IsVisible = false };
                editors.Add(editor);

                Button button = new Button { Text = "Сохранить", Margin = new Thickness(0, 0, 20, 20), IsVisible = false };               
                button.Clicked += Button_Clicked;
                buttons.Add(button);

                StackLayout st1 = new StackLayout { HorizontalOptions = LayoutOptions.End, Children = { button } };
                StackLayout st2 = new StackLayout { Children = { pealkiriLabel, sisuLabel, editor, st1 } };
                st.Children.Add(st2);

                labels[i].Text = "";
                var columns = texts[i + 2].Split('#');
                foreach (string column in columns)
                {
                    labels[i].Text += column + "\n";
                }
                labels[i].Text = labels[i].Text.Substring(0, labels[i].Text.LastIndexOf("\n"));
            }
            
            ScrollView scrollView = new ScrollView { Content = st };
            Content = scrollView;
        }
        void Tap_Tapped(object sender, EventArgs e)
        {           
            Label lbl = (Label)sender;
            lbl.IsVisible = false;

            for (int i = 0; i < 2; i++)
            {
                if (lbl == labels[i])
                {
                    editors[i].Text = lbl.Text;
                    editors[i].IsVisible = true;
                    editors[i].Focus();
                    buttons[i].IsVisible = true;
                }
            }                      
        }
        void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;            
            btn.IsVisible = false;
            for (int i = 0; i < 2; i++)
            {
                if (btn == buttons[i])
                {
                    editors[i].IsVisible = false;
                    labels[i].Text = editors[i].Text;
                    labels[i].IsVisible = true;
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            string[] lines = File.ReadAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Retseptid.txt"));
            for (int i = 0; i < lines.Length; i++)
            {
                var columns = lines[i].Split(',');
                if (columns[0] == ID.ToString())
                {
                    List<string> sonadList = new List<string> { "", "" };
                    for (int j = 0; j < sonadList.Count; j++) 
                    {
                        var sonad = labels[j].Text.Split('\n');
                        foreach (string sona in sonad)
                        {
                            sonadList[j] += sona + "#";
                        }
                        sonadList[j] = sonadList[j].Substring(0, sonadList[j].Length - 1);
                        lines[i] = columns[0] + "," + columns[1] + "," + columns[2] + "," + columns[3] + "," + sonadList[0] + "," + sonadList[1];
                    }                                        
                }
            }
            File.WriteAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Retseptid.txt"), lines);

            //texts.Clear();
            //labels.Clear();
            //editors.Clear();
            //buttons.Clear();
        }
    }
}