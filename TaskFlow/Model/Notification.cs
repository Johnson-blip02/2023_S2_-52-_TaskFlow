using SQLite;
using Plugin.LocalNotification;

namespace TaskFlow.Model
{
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

            #region NotificationRequest Generators
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
            #endregion
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
