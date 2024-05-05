using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using WhatToWatch.Models;
using WhatToWatch.Services;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Movie> movies { get; set; } =
            new ObservableCollection<Movie>();

        private ApiService apiService = new ApiService();

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {

            try
            {
                var popularMovies = await apiService.GetPopularMovieListAsync();
                foreach (var movie in popularMovies.results)
                {
                    var image = await apiService.GetMoviePosterAsync(movie.poster_path);
                    if (image != null)
                    {
                        movie.poster = image;
                    }
                    movies.Add(movie);
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
            await base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
