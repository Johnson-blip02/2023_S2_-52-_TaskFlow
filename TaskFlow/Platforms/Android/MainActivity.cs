using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using TaskFlow.Platforms.Android;

namespace TaskFlow;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    //Runs when the app is started, will ask for permissions if not granted
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != Permission.Granted) //Notification permission
        {
            ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.PostNotifications }, 0);
        }
        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReceiveBootCompleted) != Permission.Granted) //Receive boot intent permisson for running after reboot.
        {
            ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.ReceiveBootCompleted }, 0);
        }

    }

}


