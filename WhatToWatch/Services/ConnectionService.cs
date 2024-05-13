using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

namespace WhatToWatch.Services
{
    public class ConnectionService
    {
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

        public async void ShowErrorMessage(string errorMessage)
        {
            await new MessageDialog(errorMessage).ShowAsync();
        }
    }

}
