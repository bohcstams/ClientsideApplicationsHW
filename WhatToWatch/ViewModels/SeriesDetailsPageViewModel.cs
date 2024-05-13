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
    /// <summary>
    /// Viewmodel sorozatok részletes adatainak megjelenítésére
    /// </summary>
    public class SeriesDetailsPageViewModel : ViewModelBase
    {
        private Series _series;
        private BitmapImage _poster;
        private Credits _seriesCast;
        private SeriesList _relatedSeries;

        /// <summary>
        /// A sorozat részletes adatai
        /// </summary>
        public Series Series
        { 
            get { return _series; }
            set { Set(ref _series, value); }
        }


        /// <summary>
        /// A sorozat posztere
        /// </summary>
        public BitmapImage Poster
        {
            get { return _poster; }
            set { Set(ref _poster, value);}
        }

        /// <summary>
        /// A sorozat stáblistája
        /// </summary>
        public Credits SeriesCast
        {
            get { return _seriesCast; }
            set { Set(ref _seriesCast, value); }
        }

        /// <summary>
        /// A sorozathoz kapcsolódó sorozatok listája
        /// </summary>
        public SeriesList RelatedSeries
        {
            get { return _relatedSeries; }
            set { Set(ref _relatedSeries, value); }
        }

        /// <summary>
        /// A sorozat évadainak listája
        /// </summary>
        public ObservableCollection<Season> Seasons { get; set; } =
            new ObservableCollection<Season>();

        /// <summary>
        /// Apiservice a hálózati hívások lebonyolítására
        /// </summary>
        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        /// <summary>
        /// A navigációkor meghívódó függvény felüldefiniálása, letölti a megfelelő adatokat
        /// </summary>
        /// <param name="parameter">A sorozat azonosítója</param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
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
                var checker = new ConnectionService();
                if (!checker.IsConnected())
                {
                    checker.ShowErrorMessage("Kérjük ellenőrizze internetkapcsolatát!");
                }
                Debug.WriteLine(ex.Message);
            }
            await base.OnNavigatedToAsync (parameter, mode, state);
        }

        /// <summary>
        /// Elnavigál a megfelelő színész részletes adataira
        /// </summary>
        /// <param name="actorId">A színész azonosítója</param>
        public void NavigateToActorDetails(int actorId)
        {
            NavigationService.Navigate(typeof(ActorDetailsPage), actorId);
        }

        /// <summary>
        /// Elnavigál a megfelelő sorozat részletes adataira
        /// </summary>
        /// <param name="seriesId">A sorozat azonosítója</param>
        public void NavigateToSeriesDetails(int seriesId)
        {
            NavigationService.Navigate(typeof(SeriesDeatilsPage), seriesId);
        }

    }
}
