using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToWatch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SeriesMainPage : Page
    {
        public SeriesMainPage()
        {
            this.InitializeComponent();
        }

        private void Series_Clicked_Navigate_To_Details(object sender, ItemClickEventArgs e)
        {
          
        }

        private void Back_To_Main_Page(object sender, RoutedEventArgs e)
        {
            GoBackButton.IsEnabled = false;
            ViewModel.BackToMainPage();
        }

        private void Series_Search(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var searchString = SearchBar.Text;
                ViewModel.SeriesSearch(searchString);
                GoBackButton.IsEnabled = true;
            }
        }
    }
}
