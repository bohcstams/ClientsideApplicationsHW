using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Template10.Mvvm;
using WhatToWatch.Models;
using WhatToWatch.Services;
using WhatToWatch.Views;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    /// <summary>
    /// Viewmodel filmek listájának megjelenítésére
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {

        /// <summary>
        /// Csopotosított filmek listája
        /// </summary>
        public ObservableCollection<MovieGroup> MovieGroups { get; set; } =
            new ObservableCollection<MovieGroup>();

        /// <summary>
        /// Csoportosított mentett filmek listája
        /// </summary>
        public ObservableCollection<MovieGroup> MovieGroupsCache { get; set; } =
            new ObservableCollection<MovieGroup>();


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
                await GetPopularMoviesAsync();
                await GetNowPlayingMoviesAsync();
                await GetTopRatedMoviesAsync();
                await GetUpcomingMoviesAsync();
            }catch (Exception ex)
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
        /// Letölti a népszerű filmeket és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetPopularMoviesAsync()
        {
            var popularMovies = await apiService.GetPopularMoviesAsync();
            AddMovieListToGroups("Népszerű filmek", popularMovies);
        }

        /// <summary>
        /// Letölti a most játszott filmeket és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetNowPlayingMoviesAsync()
        {
            var nowPlayingMovies = await apiService.GetNowPlayingMoviesAsync();
            AddMovieListToGroups("Jelenleg a mozikban", nowPlayingMovies);

        }

        /// <summary>
        /// Letölti a legjobbra értékelt filmeket és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetTopRatedMoviesAsync()
        {
            var topRatedMovies = await apiService.GetTopRatedMoviesAsync();
            AddMovieListToGroups("Legjobbra értékelt filmek", topRatedMovies);
        }

        /// <summary>
        /// Letölti a újdonságnak számító filmeket és hozzáadja a listához
        /// </summary>
        /// <returns></returns>
        private async Task GetUpcomingMoviesAsync()
        {
            var upcomingMovies = await apiService.GetUpcomingMoviesAsync();

            AddMovieListToGroups("Újdonságok", upcomingMovies);
        }

        /// <summary>
        /// A megadott névvel hozzáadja a filmek listáját a csoportosított listához
        /// </summary>
        /// <param name="title">A csoport neve</param>
        /// <param name="movies">A filmek listjája</param>
        private void AddMovieListToGroups(string title, MovieList movies)
        {
            MovieGroups.Add(new MovieGroup
            {
                Title = title,
                Movies = movies.results
            });
        }

        /// <summary>
        /// Lebonyolít egy keresést a filmek között
        /// </summary>
        /// <param name="searchString">A keresett film címe</param>
        public async void MovieSearch(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchResult = await apiService.GetMovieSearchResultAsync(searchString);
                if(MovieGroupsCache.Count != 0)
                {
                    MovieGroups.Clear();
                    AddMovieListToGroups("Eredmények", searchResult);
                }
                else
                {
                    foreach (var group in MovieGroups)
                    {
                        MovieGroupsCache.Add(group);
                    }
                    MovieGroups.Clear();
                    AddMovieListToGroups("Eredmények", searchResult);
                }
            }
        }

        /// <summary>
        /// Visszatölti az ajánló képernyőhöz tartozó adatokat
        /// </summary>
        public void BackToMainPage()
        {
            MovieGroups.Clear();
            foreach (var group in MovieGroupsCache)
            {
                MovieGroups.Add(group);
            }
            MovieGroupsCache.Clear();
        }


        /// <summary>
        /// Elnavigál a megfelelő film részletes adataira
        /// </summary>
        /// <param name="movieId">A film azonosítója</param>
        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
        }
    }
}
