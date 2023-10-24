using Android.App;
using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Platforms.Android
{
    [BroadcastReceiver(Label = "BootReceiver", DirectBootAware = true, Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //lauch the application on reboot as a service to reinitalize the notifications and priority recalculation etc.
            if (intent.Action == "android.intent.action.BOOT_COMPLETED")
            {
                Toast.MakeText(context, "starting app in background", ToastLength.Long).Show();
                Intent new_intent = new Intent(context, typeof(MainActivity));
                new_intent.SetFlags(ActivityFlags.NewTask);
                context.StartService(new_intent); //Start it as a service
            }
        }
    }
}
