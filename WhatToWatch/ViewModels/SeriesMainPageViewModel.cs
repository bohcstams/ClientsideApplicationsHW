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
    public class SeriesMainPageViewModel : ViewModelBase
    {
        public ObservableCollection<SeriesGroup> SeriesGroups { get; set; } =
            new ObservableCollection<SeriesGroup>();

        public ObservableCollection<SeriesGroup> SeriesGroupsCache { get; set; } =
            new ObservableCollection<SeriesGroup>();

        private ApiService apiService = new ApiService("Assets/apiKey.txt");


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
                Debug.WriteLine(ex.Message);
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        public async void SeriesSearch(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchResult = await apiService.GetSeriesSearchResultAsync(searchString);
                foreach(var group in SeriesGroups)
                {
                    SeriesGroupsCache.Add(group);
                }
                SeriesGroups.Clear();
                AddSeriesListToGroups("Eredmények", searchResult);
            }
        }

        public void BackToMainPage()
        {
            SeriesGroups.Clear();
            foreach(var group in SeriesGroupsCache)
            {
                SeriesGroups.Add(group);
            }
            SeriesGroupsCache.Clear();
        }

        private async Task GetPopularSeriesAsync()
        {
            var popularSeries = await apiService.GetPopularSeriesAsync();
            AddSeriesListToGroups("Népszerű sorozatok",  popularSeries);
        }

        private async Task GetOnTheAirSeriesAsync()
        {
            var onAirSeries = await apiService.GetOnTheAirSeriesAsync();
            AddSeriesListToGroups("Közvetített sorozatok", onAirSeries);
        }

        private async Task GetAiringTodaySeriesAsync()
        {
            var airingTodaySeries = await apiService.GetAiringTodaySeriesAsync();
            AddSeriesListToGroups("Ma a TV-ben", airingTodaySeries);
        }

        private async Task GetTopRatedSeriesAsync()
        {
            var topRatedSeries = await apiService.GetTopRatedSeriesAsync();
            AddSeriesListToGroups("Legjobbra értékelt", topRatedSeries);
        }

        private void AddSeriesListToGroups(string title, SeriesList series)
        {
            SeriesGroups.Add(new SeriesGroup
            {
                Title = title,
                Series = series.results
            });
        }

        public void NavigateToDetails(Series series)
        {
            NavigationService.Navigate(typeof(SeriesDeatilsPage), series.id);
        }
    }
}
