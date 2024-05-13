using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

namespace WhatToWatch.Services
{
    /// <summary>
    /// Az internetkapcsolat ellenőrzéséért felelős osztály
    /// </summary>
    public class ConnectionService
    {
        /// <summary>
        /// Kapcsolat ellenőrzése
        /// </summary>
        /// <returns>Van-e internetkapcsolat</returns>
        public bool IsConnected()
        {
            var internetStatus = NetworkInformation.GetInternetConnectionProfile();
            if(internetStatus == null || internetStatus.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.None)
            {
            return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Feldob egy dialógusablakot az üzenettel
        /// </summary>
        /// <param name="errorMessage">Üzenet szövege</param>
        public async void ShowErrorMessage(string errorMessage)
        {
            await new MessageDialog(errorMessage).ShowAsync();
        }
    }

}
