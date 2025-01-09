using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System;
using System.Threading.Tasks;

namespace DndManager
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        const int RequestLocationId = 0;
        const int RequestBluetoothId = 1;
        const int RequestWifiId = 2;
        const int RequestNearbyWifiDevicesId = 3;

        readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.ChangeWifiState,
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.BluetoothAdvertise,
            Manifest.Permission.BluetoothConnect,
            Manifest.Permission.BluetoothScan,
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.ChangeWifiMulticastState,
            Manifest.Permission.NearbyWifiDevices
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GetLocationPermission();
            }

            if ((int)Build.VERSION.SdkInt >= 31)
            {
                GetBluetoothPermission();
            }

            GetWifiPermission();
            GetNearbyWifiDevicesPermission();
        }

        void GetLocationPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.ChangeWifiState) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestLocationId);
            }
        }

        void GetBluetoothPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothAdvertise) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothScan) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestBluetoothId);
            }
        }

        void GetWifiPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessWifiState) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.ChangeWifiMulticastState) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestWifiId);
            }
        }

        void GetNearbyWifiDevicesPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.NearbyWifiDevices) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestNearbyWifiDevicesId);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Location permission granted
                }
                else
                {
                    // Location permission denied
                }
            }
            else if (requestCode == RequestBluetoothId)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Bluetooth permission granted
                }
                else
                {
                    // Bluetooth permission denied
                }
            }
            else if (requestCode == RequestWifiId)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Wi-Fi permission granted
                }
                else
                {
                    // Wi-Fi permission denied
                }
            }
            else if (requestCode == RequestNearbyWifiDevicesId)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Nearby Wi-Fi Devices permission granted
                }
                else
                {
                    // Nearby Wi-Fi Devices permission denied
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task RequestPermissionsAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                // Notify user that permission is required
                return;
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                GetBluetoothPermission();
            }
            else
            {
                // Handle the case for devices with API level < 31
                // Notify user that Bluetooth advertising is not supported
                Console.WriteLine("Bluetooth advertising is not supported on this device.");
            }

            GetWifiPermission();
            GetNearbyWifiDevicesPermission();
        }
    }
}