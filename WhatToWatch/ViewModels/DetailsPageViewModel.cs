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
    public class DetailsPageViewModel : ViewModelBase
    {

        private Movie _movie;
        private MovieCast _moviecast;
        private BitmapImage _poster;
        private MovieList _relatedMovies;

        public Movie Movie
        {
            get { return _movie; }
            set
            {Set(ref _movie, value);}
        }

        public BitmapImage Poster
        {
            get { return _poster; }
            set { Set(ref _poster, value); }
        }

        public MovieCast MovieCast {
            get { return _moviecast; }
            set
            { Set(ref _moviecast, value); }
        }

        public MovieList RelatedMovies
        {
            get { return _relatedMovies; }
            set { Set(ref _relatedMovies, value); }
        }

        private ApiService apiService = new ApiService("Assets/apiKey.txt");

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
                Debug.WriteLine(ex.Message);
            }
            
            if(Movie == null || MovieCast == null)
            {
                //TODO: Throw up error message as dialog
            }

            await base.OnNavigatedToAsync (parameter, mode, state);
        }

        public void NavigateToDetails(int actorId)
        {
            NavigationService.Navigate(typeof(ActorDetailsPage), actorId);
        }

        public void NavigateToMovieDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
        }
    }
}
