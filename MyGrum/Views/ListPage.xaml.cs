﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGrum.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            Title = "Список";
            Label label = new Label { Text = "Добрый день!" };
            StackLayout st = new StackLayout { Children = { label } };
            Content = st;
        }
    }
}