using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace Tracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnGetLocation.Clicked += BtnGetLocation_Clicked;
        }

        private async void BtnGetLocation_Clicked(object sender, System.EventArgs e)
        {
            await RetreiveLocation();
        }

        private async Task RetreiveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 100;

            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
            {
                var position = await locator.GetLastKnownLocationAsync();

                if (position == null)
                {
                    //got a cahched position, so let's use it.
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                }
                txtLat.Text = "Latitude: " + position.Latitude.ToString();
                txtLong.Text = "Longitude: " + position.Longitude.ToString();

                var geocoder = new Geocoder();
                var locationAddress = await locator.GetAddressesForPositionAsync(position);

                foreach(var item in locationAddress){
                    if (item.CountryName != null)
                    {
                        txtProvince.Text = "Province: " + item.CountryName;
                        txtCity.Text = "City: " + item.AdminArea;
                        txtDistrict.Text = "District: " + item.SubAdminArea;
                        txtStreet.Text = "Street: " + item.Thoroughfare;
                        txtAddress.Text = "Address: " + item.SubThoroughfare;
                        break;
                    }
                }
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude)
                                 , Distance.FromMiles(1)));
            }
        }
    }
}
