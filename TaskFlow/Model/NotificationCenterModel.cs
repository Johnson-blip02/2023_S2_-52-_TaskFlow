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
        public NotificationType Type { get; set; } = NotificationType.Default;
        public int TaskId { get; set; }

        public Notification() { }

        public static class NotificationBuilderHelper
        {
            public static List<TimeSpan> ReminderLength
            {
                get
                {
                    List<TimeSpan> list = new List<TimeSpan>();
                    for (int i = 0; i < 24; i++)
                    {
                        list.Add(new TimeSpan(0, i * 30, 0));
                    }
                    return list;
                }
            }

            public static Notification CreateTodoNotifcation(TodoItem todo, TimeSpan? reminderLength)
            {
                //TODO: Implement default reminder length
                if(reminderLength == null)
                    reminderLength = new TimeSpan(1, 0, 0);

                Notification builder = new Notification()
                {
                    Request = new NotificationRequest()
                    {
                        NotificationId = todo.Id + 100,
                        Title = todo.Title,
                        Description = todo.Description,
                        Android =
                        {
                            ChannelId = "todo_notify_before"
                        },
                        Schedule =
                        {
                            NotifyTime = todo.DueDate - reminderLength,
                        }
                        
                    },
                    Type = NotificationType.Task,
                    TaskId = todo.Id,
                };

                return builder;
            }
        }
    }

    public enum NotificationType
    {
        Default = 1,
        Task = 2,
        Schedule = 3,
        Pomodoro = 4
    }
}
