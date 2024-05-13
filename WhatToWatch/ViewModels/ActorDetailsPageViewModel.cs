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
    /// <summary>
    /// Viewmodel színészek részletes adatainak megjelenítésére
    /// </summary>
    public class ActorDetailsPageViewModel : ViewModelBase
    {
        private Actor _actor;
        private BitmapImage _profilepicture;
        private ActorCast _credits;
        private ActorCast _seriesCredits;

        /// <summary>
        /// A színész részletes adatai
        /// </summary>
        public Actor Actor
        {
            get { return _actor; }
            set { Set(ref _actor, value); }
        }

        /// <summary>
        /// A színész profilképe
        /// </summary>
        public BitmapImage ProfilePicture
        {
            get { return _profilepicture; }
            set { Set(ref _profilepicture, value);}
        }

        /// <summary>
        /// Filmek, melyben a színész közreműködött
        /// </summary>
        public ActorCast Credits
        {
            get { return _credits;}
            set { Set(ref _credits, value); }
        }

        /// <summary>
        /// Sorozatok, melyben a színész közreműködött
        /// </summary>
        public ActorCast SeriesCredits
        {
            get { return _seriesCredits; }
            set { Set(ref _seriesCredits, value); }
        }

        /// <summary>
        /// Apiservice a hálózati hívások lebonyolítására
        /// </summary>
        private ApiService apiService = new ApiService("Assets/apiKey.txt");

        /// <summary>
        /// A navigációkor meghívódó függvény felüldefiniálása, letölti a megfelelő adatokat
        /// </summary>
        /// <param name="parameter">A színész azonosítója</param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Elnavigál a megfelelő film részletes adataira
        /// </summary>
        /// <param name="movieId">A film azonosítója</param>
        public void NavigateToDetails(int movieId)
        {
            NavigationService.Navigate(typeof(DetailsPage), movieId);
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
