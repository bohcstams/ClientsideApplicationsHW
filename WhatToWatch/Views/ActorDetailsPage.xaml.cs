using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WhatToWatch.Models;
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
    public sealed partial class ActorDetailsPage : Page
    {
        public ActorDetailsPage()
        {
            this.InitializeComponent();
        }

        private void Movie_Clicked_Navigate_To_Details(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var movieHandler = (Movie)button.DataContext;
            ViewModel.NavigateToDetails(movieHandler.id);
        }

        private void Series_Clicked_Navigate_To_Details(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var seriesHandler = (Movie)button.DataContext;
            ViewModel.NavigateToSeriesDetails(seriesHandler.id);
        }
    }
}
