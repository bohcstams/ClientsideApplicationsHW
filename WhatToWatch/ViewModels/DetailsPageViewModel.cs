using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Viewmodel filmek részletes adatainak megjelenítésére
    /// </summary>
    public class DetailsPageViewModel : ViewModelBase
    {

        private Movie _movie;
        private Credits _moviecast;
        private BitmapImage _poster;
        private MovieList _relatedMovies;

        /// <summary>
        /// A film részletes adatai
        /// </summary>
        public Movie Movie
        {
            get { return _movie; }
            set
            {Set(ref _movie, value);}
        }

        /// <summary>
        /// A film posztere
        /// </summary>
        public BitmapImage Poster
        {
            get { return _poster; }
            set { Set(ref _poster, value); }
        }

        /// <summary>
        /// A film stáblistája
        /// </summary>
        public Credits MovieCast {
            get { return _moviecast; }
            set
            { Set(ref _moviecast, value); }
        }

        /// <summary>
        /// A filmhez kapcsolódó filmek listája
        /// </summary>
        public MovieList RelatedMovies
        {
            get { return _relatedMovies; }
            set { Set(ref _relatedMovies, value); }
        }

        /// <summary>
        /// Apiservice a hálózati hívások lebonyolítására
        /// </summary>
        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        /// <summary>
        /// A navigációkor meghívódó függvény felüldefiniálása, letölti a megfelelő adatokat
        /// </summary>
        /// <param name="parameter">A film azonosítója</param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var movieId = (int)parameter;
            try
            {
                Movie = await apiService.GetMovieDetailsAsync(movieId);
                Movie.poster = await apiService.GetPosterAsync(Movie.poster_path);
                Poster = Movie.poster;
                MovieCast = await apiService.GetMovieCastAsync(movieId);
                RelatedMovies = await apiService.GetRelatedMoviesAsync(movieId);

            }catch(Exception ex)
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
        /// <param name="movieId">A színész azonosítója</param>
        public void NavigateToDetails(int actorId)
        {
            NavigationService.Navigate(typeof(ActorDetailsPage), actorId);
        }

        /// <summary>
        /// Elnavigál a megfelelő film részletes adataira
        /// </summary>
        /// <param name="movieId">A film azonosítója</param>
        public void NavigateToMovieDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
        }
    }
}
