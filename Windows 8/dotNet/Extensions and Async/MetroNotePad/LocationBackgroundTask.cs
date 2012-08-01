using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;

namespace MetroNotePad
{
    public  class LocationBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                await GetPosition();
            }
            finally
            {
                deferral.Complete();
            }
        }

        private async Task GetPosition()
        {
            var _geolocator = new Geolocator();
            var _cts = new CancellationTokenSource();

            //CancellationToken token = _cts.Token;

            // Carry out the operation
            Geoposition pos = await _geolocator.GetGeopositionAsync();//.AsTask(token);

            WriteLocationData(pos);
        }

        private void WriteLocationData(Geoposition pos)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["Latitude"] = pos.Coordinate.Latitude.ToString();
            settings.Values["Longitude"] = pos.Coordinate.Longitude.ToString();
            settings.Values["Accuracy"] = pos.Coordinate.Accuracy.ToString();
        }

    }
}
