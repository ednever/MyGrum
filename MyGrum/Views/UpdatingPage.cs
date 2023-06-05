using System;
using System.IO;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.Generic;
using MyGrum.Models;

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
            Button buttonSave = new Button { Text = "Сохранить", Margin = new Thickness(0, 20, 20, 0) };
            Button buttonDelete = new Button { Text = "Удалить", Margin = new Thickness(0, 20, 5, 0) };
            buttonSave.Clicked += Button_Clicked;
            buttonDelete.Clicked += Button_Clicked;

            StackLayout st1 = new StackLayout 
            { 
                Orientation = StackOrientation.Horizontal, 
                HorizontalOptions = LayoutOptions.End, 
                Children = { buttonDelete, buttonSave } 
            };
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
            Button btn = (Button)sender;
            if (btn.Text == "Удалить") 
            {
                bool result = await DisplayAlert("Уведомление", "Вы действительно желаете удалить?", "Нет", "Да");

                if (result)
                {
                    return;
                }
                else
                {
                    string textToFile = num.ToString() + "," + entry.Text + "," + Path.GetFileName(image.Source.ToString());
                    int fileNumber = 0;

                    if (isPageRecipesPageOrGroceryPage && isPageInModeClassOrSubclass)
                    {
                        fileNumber = 2;
                    }
                    else if (isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                    {
                        fileNumber = 3;
                        textToFile += "," + classID.ToString() + "," + "...,...";
                    }
                    else if (!isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                    {
                        fileNumber = 1;
                        textToFile += "," + classID.ToString();
                    }

                    string[] lines = File.ReadAllLines(Path.Combine(folderPath, fileNames[fileNumber]));
                    
                    List<string> abiLinesList = new List<string>();
                    foreach (var item in lines)
                    {
                        abiLinesList.Add(item);
                    }

                    for (int i = 0; i < abiLinesList.Count; i++)
                    {
                        var columns = abiLinesList[i].Split(',');
                        if (!isPageInModeClassOrSubclass)
                        {
                            if (columns[0] == num.ToString() && columns[3] == classID.ToString())
                                abiLinesList.Remove(textToFile);                           
                        }
                        else
                        {
                            if (columns[0] == num.ToString())
                            {
                                abiLinesList.Remove(textToFile);

                                if (isPageRecipesPageOrGroceryPage)
                                {
                                    string[] linesRetseptid = File.ReadAllLines(Path.Combine(folderPath, fileNames[3]));
                                    List<string> abiLinesRetseptidList = new List<string>();
                                    foreach (var item in linesRetseptid)
                                    {
                                        abiLinesRetseptidList.Add(item);
                                    }
                                    for (int j = 0; j < abiLinesRetseptidList.Count; j++)
                                    {
                                        var columnsRetseptid = abiLinesRetseptidList[j].Split(',');
                                        if (columnsRetseptid[3] == num.ToString())
                                            abiLinesRetseptidList.Remove(abiLinesRetseptidList[j]);
                                    }
                                    string[] newlinesRetseptid = abiLinesRetseptidList.ToArray();
                                    File.WriteAllLines(Path.Combine(folderPath, fileNames[3]), newlinesRetseptid);
                                }
                                else
                                {
                                    string[] linesRetseptid = File.ReadAllLines(Path.Combine(folderPath, fileNames[1]));
                                    List<string> abiLinesRetseptidList = new List<string>();
                                    foreach (var item in linesRetseptid)
                                    {
                                        abiLinesRetseptidList.Add(item);
                                    }
                                    for (int j = 0; j < abiLinesRetseptidList.Count; j++)
                                    {
                                        var columnsRetseptid = abiLinesRetseptidList[j].Split(',');
                                        if (columnsRetseptid[3] == num.ToString())
                                            abiLinesRetseptidList.Remove(abiLinesRetseptidList[j]);
                                    }
                                    string[] newlinesRetseptid = abiLinesRetseptidList.ToArray();
                                    File.WriteAllLines(Path.Combine(folderPath, fileNames[1]), newlinesRetseptid);
                                }
                            }                                                          
                        }
                    }
                    string[] newlines = abiLinesList.ToArray();

                    string testqa = ""; //Проверка
                    foreach (var item in newlines)
                    {
                        testqa += "\n" + item;
                    }
                    await DisplayAlert("Ошибка", testqa, "Ок");

                    File.WriteAllLines(Path.Combine(folderPath, fileNames[fileNumber]), newlines);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(entry.Text))
                {
                    await DisplayAlert("Ошибка", "Заполните все поля!", "Ок");
                }
                else
                {
                    int fileNumber = 0;
                    string textToFile = num.ToString() + "," + entry.Text + "," + Path.GetFileName(image.Source.ToString());
                    
                    if (isPageRecipesPageOrGroceryPage && isPageInModeClassOrSubclass)
                    {
                        fileNumber = 2;
                    }
                    else if (isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                    {
                        fileNumber = 3;
                        textToFile += "," + classID.ToString() + "," + "...,..."; //подумать можно
                    }
                    else if (!isPageRecipesPageOrGroceryPage && !isPageInModeClassOrSubclass)
                    {
                        fileNumber = 1;
                        textToFile += "," + classID.ToString();
                    }

                    string[] lines = File.ReadAllLines(Path.Combine(folderPath, fileNames[fileNumber]));
                    if (!isPageInModeClassOrSubclass)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var columns = lines[i].Split(',');
                            if (columns[0] == num.ToString() && columns[3] == classID.ToString())
                            {
                                lines[i] = textToFile;
                            }
                        }
                    }
                    else
                    {
                        lines[num - 1] = textToFile;
                    }

                    string testqa = ""; //Проверка
                    foreach (var item in lines)
                    {
                        testqa += "\n" + item;
                    }
                    await DisplayAlert("Ошибка", testqa, "Ок");

                    if (File.Exists(Path.Combine(folderPath, fileNames[fileNumber])))
                        File.WriteAllLines(Path.Combine(folderPath, fileNames[fileNumber]), lines);

                    await Navigation.PopAsync();
                }
            }           
        }
    }
}
/*
 * Обновлять рецепт с его описанием
 */