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

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var service = new ApiService();

            try
            {
                var popularMovies = await service.GetPopularMovieListAsync();
                foreach (var movie in popularMovies.results)
                {
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
