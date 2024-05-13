using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WhatToWatch.Services;
using WhatToWatch.Views;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    public class EntryScreenViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var checker = new ConnectionService();
            if (!checker.IsConnected())
            {
                checker.ShowErrorMessage("Kérjük ellenőrizze internetkapcsolatát!");
            }
            await base.OnNavigatedToAsync(parameter, mode, state);        
        }
        
        public void NavigateToMovies()
        {
            NavigationService.Navigate(typeof(MainPage));
        }

        public void NavigateToSeries()
        {
            NavigationService.Navigate(typeof(SeriesMainPage));
        }
    }
}
