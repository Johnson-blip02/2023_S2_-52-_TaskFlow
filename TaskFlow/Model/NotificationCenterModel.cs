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

        public async Task<int> RestoreNotifcations()
        {
            if(LocalNotificationCenter.Current.GetPendingNotificationList != null)
            {
                return 0;
            }

            var notifications = GetData();
            int count = 0;

            foreach (var notification in notifications)
            {
                bool success = await LocalNotificationCenter.Current.Show(notification.Request);
                if (success)
                    count++;
            }

            return count;
        }

        public bool ScheduleNotification(Notification notification)
        {
            foreach(var list in GetData())
            {
                if(list.Request.NotificationId == notification.Request.NotificationId)
                {
                    return false;
                }
            }

            Insert(notification);
            LocalNotificationCenter.Current.Show(notification.Request);

            return true;
        }
    }

    public class Notification
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public NotificationRequest Request { get; set; }
        public NotificationType Type { get; set; }
        public int TaskId { get; set; }

        public Notification() { }

        public Notification(NotificationRequest request, NotificationType type, int taskId)
        {
            Request = request;
            Type = type;
            TaskId = taskId;
        }
    }

    public enum NotificationType
    {
        Task,
        Schedule,
        Pomodoro
    }
}
