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
    /// <summary>
    /// Viewmodel a kezdőképernyő funkcióinak megvalósítására
    /// </summary>
    public class EntryScreenViewModel : ViewModelBase
    {
        /// <summary>
        /// A navigációkor meghívódó függvény felüldefiniálása, ellenőrzi az internetkapcsolatot
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Elnavigál a filmek képernyőjére
        /// </summary>
        public void NavigateToMovies()
        {
            NavigationService.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Elnavigál a sorozatok képernyőjére
        /// </summary>
        public void NavigateToSeries()
        {
            NavigationService.Navigate(typeof(SeriesMainPage));
        }
    }
}
