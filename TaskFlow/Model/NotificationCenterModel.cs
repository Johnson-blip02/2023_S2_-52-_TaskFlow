using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotification;
using Android.App;
using Plugin.LocalNotification.AndroidOption;
using SQLiteNetExtensions.Extensions.TextBlob;
using System.Text.Json;
using SQLiteNetExtensions.Attributes;
using System.Runtime.Serialization.Formatters.Binary;

namespace TaskFlow.Model
{
    public class NotificationCenterModel : Database<Notification>
    {
        protected override void CreateTableAsync()
        {
            dbConn.CreateTable<Notification>();
        }

        protected override List<Notification> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<Notification>();
            }
            catch
            {
                return null;
            }
        }

        public int RestoreNotifcations()
        {
            if (LocalNotificationCenter.Current.GetPendingNotificationList != null)
            {
                return 0;
            }

            var notifications = GetData();
            int count = 0;

            foreach (var notification in notifications)
            {
                bool success = ScheduleNotification(notification);
                if (success)
                    count++;
            }

            return count;
        }

        public bool ScheduleNotification(Notification notification)
        {
            foreach(var list in GetData())
            {
                if(list.NotificationId == notification.NotificationId)
                {
                    return false;
                }
            }
            Insert(notification);

            NotificationRequest request = Notification.NotificationRequestGenerator.GetNotification(notification);
            LocalNotificationCenter.Current.Show(request);

            return true;
        }
    }
}
