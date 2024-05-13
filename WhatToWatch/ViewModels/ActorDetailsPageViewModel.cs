using System;
using System.Collections.Generic;
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
    public class ActorDetailsPageViewModel : ViewModelBase
    {
        private Actor _actor;
        private BitmapImage _profilepicture;
        private ActorCast _credits;
        private ActorCast _seriesCredits;

        public Actor Actor
        {
            get { return _actor; }
            set { Set(ref _actor, value); }
        }

        public BitmapImage ProfilePicture
        {
            get { return _profilepicture; }
            set { Set(ref _profilepicture, value);}
        }

        public ActorCast Credits
        {
            get { return _credits;}
            set { Set(ref _credits, value); }
        }

        public ActorCast SeriesCredits
        {
            get { return _seriesCredits; }
            set { Set(ref _seriesCredits, value); }
        }

        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        { 
            var actorId = (int)parameter;
            try
            {
                Actor = await apiService.GetActorDetailsAsync(actorId);
                ProfilePicture = await apiService.GetPosterAsync(Actor.profile_path);
                Credits = await apiService.GetActorCastAsync(actorId);
                SeriesCredits = await apiService.GetActorSeriesCreditsAsync(actorId);
            }catch(Exception ex) {
                var checker = new ConnectionService();
                if (!checker.IsConnected())
                {
                    checker.ShowErrorMessage("Kérjük ellenőrizze internetkapcsolatát!");
                }
            }

            await base.OnNavigatedToAsync (parameter, mode, state);
        }

        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
        }

        public void NavigateToSeriesDetails(int seriesId)
        {
            NavigationService.Navigate(typeof(SeriesDeatilsPage), seriesId);
        }
    }
}
