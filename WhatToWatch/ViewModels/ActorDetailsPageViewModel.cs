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

        private ApiService apiService = new ApiService();

        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        { 
            var actorId = (int)parameter;
            try
            {
                Actor = await apiService.GetActorDetailsAsync(actorId);
                ProfilePicture = await apiService.GetMoviePosterAsync(Actor.profile_path);
                Credits = await apiService.GetActorCastAsync(actorId);

            }catch(Exception ex) { 
            }

            await base.OnNavigatedToAsync (parameter, mode, state);
        }

        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
        }
    }
}
