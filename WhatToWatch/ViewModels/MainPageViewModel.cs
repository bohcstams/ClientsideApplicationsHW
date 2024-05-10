using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Template10.Mvvm;
using WhatToWatch.Models;
using WhatToWatch.Services;
using WhatToWatch.Views;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public ObservableCollection<MovieGroup> MovieGroups { get; set; } =
            new ObservableCollection<MovieGroup>();

        public ObservableCollection<MovieGroup> MovieGroupsCache { get; set; } =
            new ObservableCollection<MovieGroup>();



        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {

            try
            {
                await GetPopularMoviesAsync();
                await GetNowPlayingMoviesAsync();
                await GetTopRatedMoviesAsync();
                await GetUpcomingMoviesAsync();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async Task GetPopularMoviesAsync()
        {
            var popularMovies = await apiService.GetPopularMovieListAsync();
            await GetPostersForMovieListAsync(popularMovies);
            AddMovieListToGroups("Népszerű filmek", popularMovies);
        }

        private async Task GetNowPlayingMoviesAsync()
        {
            var nowPlayingMovies = await apiService.GetNowPlayingMoviesAsync();
            await GetPostersForMovieListAsync(nowPlayingMovies);
            AddMovieListToGroups("Jelenleg a mozikban", nowPlayingMovies);

        }

        private async Task GetTopRatedMoviesAsync()
        {
            var topRatedMovies = await apiService.GetTopRatedMoviesAsync();
            await GetPostersForMovieListAsync(topRatedMovies);
            AddMovieListToGroups("Legjobbra értékelt filmek", topRatedMovies);
        }

        private async Task GetUpcomingMoviesAsync()
        {
            var upcomingMovies = await apiService.GetUpcomingMoviesAsync();
            await GetPostersForMovieListAsync(upcomingMovies);
            AddMovieListToGroups("Újdonságok", upcomingMovies);
        }

        private async Task GetPostersForMovieListAsync(MovieList movieList)
        {
            foreach(var movie in movieList.results)
            {
                var image = await apiService.GetMoviePosterAsync(movie.poster_path);
                if (image != null)
                {
                    movie.poster = image;
                }
            }
        }

        private void AddMovieListToGroups(string title, MovieList movies)
        {
            MovieGroups.Add(new MovieGroup
            {
                Title = title,
                Movies = movies.results
            });
        }

        public async void MovieSearch(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchResult = await apiService.GetMovieSearchResultAsync(searchString);
                await GetPostersForMovieListAsync(searchResult);
                foreach(var group in MovieGroups)
                {
                    MovieGroupsCache.Add(group);
                }
                MovieGroups.Clear();
                AddMovieListToGroups("Eredmények", searchResult);
            }
        }

        public void BackToMainPage()
        {
            MovieGroups.Clear();
            foreach (var group in MovieGroupsCache)
            {
                MovieGroups.Add(group);
            }
            MovieGroupsCache.Clear();
        }


        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
        }
    }
}
