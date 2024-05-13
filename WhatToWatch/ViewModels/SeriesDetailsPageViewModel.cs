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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    public class SeriesDetailsPageViewModel : ViewModelBase
    {
        private Series _series;
        private BitmapImage _poster;
        private Credits _seriesCast;
        private SeriesList _relatedSeries;

        public Series Series
        { 
            get { return _series; }
            set { Set(ref _series, value); }
        }

        public BitmapImage Poster
        {
            get { return _poster; }
            set { Set(ref _poster, value);}
        }

        public Credits SeriesCast
        {
            get { return _seriesCast; }
            set { Set(ref _seriesCast, value); }
        }

        public SeriesList RelatedSeries
        {
            get { return _relatedSeries; }
            set { Set(ref _relatedSeries, value); }
        }

        public ObservableCollection<Season> Seasons { get; set; } =
            new ObservableCollection<Season>();

        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        { 
            var seriesId = (int)parameter;
            try
            {
                Series = await apiService.GetSeriesDetailsAsync(seriesId);
                Poster = await apiService.GetPosterAsync(Series.poster_path);
                SeriesCast = await apiService.GetSeriesCastAsync(seriesId);
                RelatedSeries = await apiService.GetRecommendedSeriesAsync(seriesId);
                foreach (Season season in Series.seasons)
                {
                    Seasons.Add(season);
                }
            }catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await base.OnNavigatedToAsync (parameter, mode, state);
        }

        public void NavigateToActorDetails(int actorId)
        {
            NavigationService.Navigate(typeof(ActorDetailsPage), actorId);
        }

        public void NavigateToSeriesDetails(int seriesId)
        {
            NavigationService.Navigate(typeof(SeriesDeatilsPage), seriesId);
        }

    }
}
