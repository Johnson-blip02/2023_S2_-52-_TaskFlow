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

            NotificationRequest request = Notification.NotificationBuilderHelper.NotificationRequestGenerator.GetNotification(notification);
            LocalNotificationCenter.Current.Show(request);

            return true;
        }
    }

    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set;}
        public DateTime NotifyTime { get; set;}
        public NotificationType Type { get; set; } = NotificationType.Default;
        public int NotificationId { get; set; }

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
                if (reminderLength == null)
                    reminderLength = new TimeSpan(1, 0, 0);

                Notification builder = new Notification()
                {
                    Type = NotificationType.TaskBefore,
                    TaskId = todo.Id,
                    NotificationId = todo.Id + 100,
                    NotifyTime = todo.DueDate - (TimeSpan)reminderLength,
                    Description = todo.Description,
                    Title = todo.Title,
                };

                return builder;
            }
            public static class NotificationRequestGenerator
            {
                public static NotificationRequest GetNotification(Notification notif)
                {
                    NotificationRequest request = null;

                    switch(notif.Type)
                    {
                        case NotificationType.Default:
                            request = GetDefaultNotification(notif);
                            break;
                        case NotificationType.TaskBefore:
                            request = GetTaskBeforeNotification(notif);
                            break;
                        case NotificationType.TaskAfter:
                            request = GetTaskAfterNotification(notif);
                            break;
                        case NotificationType.Schedule:
                            request = GetScheduleNotification(notif);
                            break;
                        case NotificationType.Pomodoro:
                            request = GetPomodoroNotification(notif);
                            break;
                    }

                    return request;
                }

                private static NotificationRequest GetDefaultNotification(Notification notif)
                {
                    NotificationRequest request = new NotificationRequest
                    {
                        Title = notif.Title,
                        Description = notif.Description,
                        NotificationId = notif.NotificationId,
                        Android =
                        {
                            ChannelId = "general_notify"
                        },
                        Schedule =
                        {
                            NotifyTime = notif.NotifyTime
                        }

                    };
                    return request;
                }

                private static NotificationRequest GetTaskBeforeNotification(Notification notif)
                {
                    NotificationRequest request = new NotificationRequest
                    {
                        Title = notif.Title,
                        Description = notif.Description,
                        NotificationId = notif.NotificationId,
                        Android =
                        {
                            ChannelId = "todo_notify_before"
                        },
                        Schedule =
                        {
                            NotifyTime = notif.NotifyTime
                        }

                    };
                    return request;
                }

                private static NotificationRequest GetTaskAfterNotification(Notification notif)
                {
                    NotificationRequest request = new NotificationRequest
                    {
                        Title = notif.Title,
                        Description = notif.Description,
                        NotificationId = notif.NotificationId,
                        Android =
                        {
                            ChannelId = "todo_notify_after"
                        },
                        Schedule =
                        {
                            NotifyTime = notif.NotifyTime
                        }

                    };
                    return request;
                }

                private static NotificationRequest GetScheduleNotification(Notification notif)
                {
                    NotificationRequest request = new NotificationRequest
                    {
                        Title = notif.Title,
                        Description = notif.Description,
                        NotificationId = notif.NotificationId,
                        Android =
                        {
                            ChannelId = "general_notify"
                        },
                        Schedule =
                        {
                            NotifyTime = notif.NotifyTime
                        }

                    };
                    return request;
                }

                private static NotificationRequest GetPomodoroNotification(Notification notif)
                {
                    NotificationRequest request = new NotificationRequest
                    {
                        Title = notif.Title,
                        Description = notif.Description,
                        NotificationId = notif.NotificationId,
                        Android =
                        {
                            ChannelId = "pomodoro_notify"
                        },
                        Schedule =
                        {
                            NotifyTime = notif.NotifyTime
                        }

                    };
                    return request;
                }
            }
        }
    }


    public enum NotificationType
    {
        Default = 1,
        TaskBefore = 2,
        TaskAfter = 3,
        Schedule = 4,
        Pomodoro = 5
    }
}
