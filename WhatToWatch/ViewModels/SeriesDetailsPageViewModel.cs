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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace WhatToWatch.ViewModels
{
    public class SeriesDetailsPageViewModel : ViewModelBase
    {
        private Series _series;
        private BitmapImage _poster;

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

        public ObservableCollection<Season> Seasons { get; set; } =
            new ObservableCollection<Season>();

        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        { 
            Series = (Series)parameter;
            try
            {   
                Poster = await apiService.GetPosterAsync(Series.poster_path);
                Series = await apiService.GetSeriesDetailsAsync(Series.id);
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

    }
}
