using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WhatToWatch.Models;
using WhatToWatch.Services;
using WhatToWatch.Views;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    /// <summary>
    /// Viewmodel sorozatok listájának megjelenjtésére
    /// </summary>
    public class SeriesMainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Csoportosított sorozatok listája
        /// </summary>
        public ObservableCollection<SeriesGroup> SeriesGroups { get; set; } =
            new ObservableCollection<SeriesGroup>();

        /// <summary>
        /// Csoportosított mentett sorozatok listája
        /// </summary>
        public ObservableCollection<SeriesGroup> SeriesGroupsCache { get; set; } =
            new ObservableCollection<SeriesGroup>();

        /// <summary>
        /// Apiservice a hálózati hívások lebonyolítására
        /// </summary>
        private ApiService apiService = new ApiService("Assets/apiKey.txt");


        /// <summary>
        /// A navigációkor meghívódó függvény felüldefiniálása, letölti a megfelelő adatokat
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            try
            {
                await GetPopularSeriesAsync();
                await GetOnTheAirSeriesAsync();
                await GetTopRatedSeriesAsync();
                await GetAiringTodaySeriesAsync();
            }catch(Exception ex)
            {
                var checker = new ConnectionService();
                if (!checker.IsConnected())
                {
                    checker.ShowErrorMessage("Kérjük ellenőrizze internetkapcsolatát!");
                }
                Debug.WriteLine(ex.Message);
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        /// <summary>
        /// Lebonyolít egy keresést a sorozatok között
        /// </summary>
        /// <param name="searchString">A keresett sorozat címe</param>
        public async void SeriesSearch(string searchString)
        {
            var checker = new ConnectionService();
            if (checker.IsConnected())
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchResult = await apiService.GetSeriesSearchResultAsync(searchString);
                    if (SeriesGroupsCache.Count != 0)
                    {
                        SeriesGroups.Clear();
                        AddSeriesListToGroups("Eredmények", searchResult);
                    }
                    else
                    {
                        foreach (var group in SeriesGroups)
                        {
                            SeriesGroupsCache.Add(group);
                        }
                        SeriesGroups.Clear();
                        AddSeriesListToGroups("Eredmények", searchResult);
                    }
                }
            }
            
        }

        /// <summary>
        /// Visszatölti az ajánló képernyőhöz tartozó adatokat
        /// </summary>
        public void BackToMainPage()
        {
            SeriesGroups.Clear();
            foreach(var group in SeriesGroupsCache)
            {
                SeriesGroups.Add(group);
            }
            SeriesGroupsCache.Clear();
        }

        /// <summary>
        /// Letölti a népszerű sorozatokat és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetPopularSeriesAsync()
        {
            var popularSeries = await apiService.GetPopularSeriesAsync();
            AddSeriesListToGroups("Népszerű sorozatok",  popularSeries);
        }

        /// <summary>
        /// Letölti a most közvetített sorozatokat és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetOnTheAirSeriesAsync()
        {
            var onAirSeries = await apiService.GetOnTheAirSeriesAsync();
            AddSeriesListToGroups("Közvetített sorozatok", onAirSeries);
        }

        /// <summary>
        /// Letölti a ma közvetített sorozatokat és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetAiringTodaySeriesAsync()
        {
            var airingTodaySeries = await apiService.GetAiringTodaySeriesAsync();
            AddSeriesListToGroups("Ma a TV-ben", airingTodaySeries);
        }

        /// <summary>
        /// Letölti a legjobbra értékelt orozatokat és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetTopRatedSeriesAsync()
        {
            var topRatedSeries = await apiService.GetTopRatedSeriesAsync();
            AddSeriesListToGroups("Legjobbra értékelt", topRatedSeries);
        }

        /// <summary>
        /// A megadott névvel hozzáadja a sorozatok listáját a csoportosított listához
        /// </summary>
        /// <param name="title">A csoport neve</param>
        /// <param name="movies">A sorozatok listjája</param>
        private void AddSeriesListToGroups(string title, SeriesList series)
        {
            SeriesGroups.Add(new SeriesGroup
            {
                Title = title,
                Series = series.results
            });
        }

        /// <summary>
        /// Elnavigál a megfelelő sorozat részletes adataira
        /// </summary>
        /// <param name="movieId">A sorozat azonosítója</param>
        public void NavigateToDetails(Series series)
        {
            NavigationService.Navigate(typeof(SeriesDeatilsPage), series.id);
        }
    }
}
