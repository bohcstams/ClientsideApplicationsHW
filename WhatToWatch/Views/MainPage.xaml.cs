using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using WhatToWatch.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WhatToWatch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void Movie_Clicked_Navigate_To_Details(object sender, ItemClickEventArgs e)
        {
            var movieHandler = (Movie)e.ClickedItem;
            ViewModel.NavigateToDetails(movieHandler.id);
        }

        private void Movie_Search(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                var searchString = SearchBar.Text;
                ViewModel.MovieSearch(searchString);
                GoBackButton.IsEnabled = true;
            }  
        }

        private void Back_To_Main_Page(object sender, RoutedEventArgs e)
        {
            GoBackButton.IsEnabled=false;
            ViewModel.BackToMainPage();
        }
    }
}
