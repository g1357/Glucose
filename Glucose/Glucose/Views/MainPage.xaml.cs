﻿using System;

using Glucose.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Glucose.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
