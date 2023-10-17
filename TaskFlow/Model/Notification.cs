using SQLite;
using Plugin.LocalNotification;

namespace TaskFlow.Model
{
    /// <summary>
    /// Class for storing notification information. Contains helper sub-classes for
    /// generating notifications which will be used by the notification center.
    /// </summary>
    /// <see cref="NotificationCenterModel"/>
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
            /// <summary>
            /// Generates a list of possible timespans that a task reminder can be set to.
            /// </summary>
            [Obsolete("Method is not used")]
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

            /// <summary>
            /// Offsets for the notification ids to prevent conflicts with other notifications.
            /// </summary>
            private enum NotificationDivider
            {
                System = 0,
                General = 100,
                Pomodoro = 250,
                TaskBefore = 300,
                TaskStart = 400,
                TaskAfter = 500,
            }

            /// <summary>
            /// Creates a notifcation for a task reminder.
            /// </summary>
            /// <param name="todo">Todo Object</param>
            /// <param name="reminderLength">Timespan before the beginning of the notification to show a reminder</param>
            /// <returns>Generated Notification</returns>
            public static Notification CreatePreTodoNotifcation(TodoItem todo, TimeSpan? reminderLength)
            {
                if (reminderLength == null)
                    reminderLength = new TimeSpan(0, 15, 0);

                Notification builder = new Notification()
                {
                    Type = NotificationType.TaskBefore,
                    TaskId = todo.Id,
                    NotificationId = todo.Id + (int)NotificationDivider.TaskBefore,
                    NotifyTime = todo.DueDate - (TimeSpan)reminderLength,
                    Description = todo.Description,
                    Title = todo.Title,
                };

                return builder;
            }

            /// <summary>
            /// Creates a notifcation for task when a task starts
            /// </summary>
            /// <param name="todo">Todo Object</param>
            /// <returns>Generated Notification</returns>
            public static Notification CreateStartTodoNotifcation(TodoItem todo)
            {
                Notification builder = new Notification()
                {
                    Type = NotificationType.TaskStart,
                    TaskId = todo.Id,
                    NotificationId = todo.Id + (int)NotificationDivider.TaskStart,
                    NotifyTime = todo.DueDate,
                    Description = todo.Description,
                    Title = todo.Title,
                };

                return builder;
            }

            /// <summary>
            /// Creates a notifcation for task when a task ends
            /// </summary>
            /// <param name="todo">Todo Object</param>
            /// <returns>Generated Notification</returns>
            public static Notification CreateTodoEndNotifcation(TodoItem todo)
            {
                TimeSpan notify = new TimeSpan(0, 5, 0);

                if(todo.TimeBlock/3 < notify)
                    notify = todo.TimeBlock / 3;

                Notification builder = new Notification()
                {
                    Type = NotificationType.TaskAfter,
                    TaskId = todo.Id,
                    NotificationId = todo.Id + (int)NotificationDivider.TaskAfter,
                    NotifyTime = todo.DueDate + (todo.TimeBlock - notify),
                    Description = todo.Description,
                    Title = todo.Title,
                };

                return builder;
            }
        }

        /// <summary>
        /// Helper class for generating notification requests based on the type of notification.
        /// </summary>
        public static class NotificationRequestGenerator
        {
            /// <summary>
            /// Creates a notification request based on the type of notification.
            /// </summary>
            /// <param name="notification">Notification Object</param>
            /// <returns></returns>
            public static NotificationRequest GetNotification(Notification notification)
            {
                NotificationRequest request = null;

                switch(notification.Type)
                {
                    case NotificationType.TaskBefore:
                        request = GetTaskBeforeNotification(notification);
                        break;
                    case NotificationType.TaskAfter:
                        request = GetTaskAfterNotification(notification);
                        break;
                    case NotificationType.Schedule:
                        request = GetScheduleNotification(notification);
                        break;
                    case NotificationType.Pomodoro:
                        request = GetPomodoroNotification(notification);
                        break;
                    case NotificationType.TaskStart:
                        request = GetTaskStartNotification(notification);
                        break;
                    default:
                        request = GetDefaultNotification(notification);
                        break;
                }

                return request;
            }

            #region NotificationRequest Generators
            /// <summary>
            /// Notifications without a type should be assigned to a default type. Returns a generic notification
            /// request.
            /// </summary>
            /// <param name="notification">Notification object</param>
            /// <returns></returns>
            private static NotificationRequest GetDefaultNotification(Notification notification)
            {
                NotificationRequest request = new NotificationRequest
                {
                    Title = notification.Title,
                    Description = notification.Description,
                    NotificationId = notification.NotificationId,
                    Android =
                    {
                        ChannelId = "general_notify"
                    },
                    Schedule =
                    {
                        NotifyTime = notification.NotifyTime
                    }

                };
                return request;
            }

            /// <summary>
            /// Notifications which will display before a task starts ie the task reminder. Returns a notification request
            /// for task related notificatons.
            /// </summary>
            /// <param name="notification">Notification Object</param>
            /// <returns>Notification Request</returns>
            private static NotificationRequest GetTaskBeforeNotification(Notification notification)
            {
                NotificationRequest request = new NotificationRequest
                {
                    Title = notification.Title + " Starting Soon!",
                    Description = notification.Description,
                    NotificationId = notification.NotificationId,
                    Android =
                    {
                        ChannelId = "todo_notify_before",
                    },
                    Schedule =
                    {
                        NotifyTime = notification.NotifyTime
                    },
                    CategoryType = NotificationCategoryType.Event,
                    ReturningData = notification.TaskId.ToString(),
                };
                return request;
            }

            /// <summary>
            /// Notifications which will display when a task starts. Returns a notification request
            /// for task related notificatons.
            /// </summary>
            /// <param name="notification">Notification Object</param>
            /// <returns>Notification Request</returns>
            private static NotificationRequest GetTaskStartNotification(Notification notification)
            {
                NotificationRequest request = new NotificationRequest
                {
                    Title = notification.Title + " Has Started!",
                    Description = notification.Description,
                    NotificationId = notification.NotificationId,
                    Android =
                    {
                        ChannelId = "todo_notify_before",
                        AutoCancel = false,
                    },
                    Schedule =
                    {
                        NotifyTime = notification.NotifyTime
                    },
                    CategoryType = NotificationCategoryType.Event,
                    ReturningData = notification.TaskId.ToString(),
                    
                };
                return request;
            }

            /// <summary>
            /// Notifications which will display after a task ends / reaches its deadline.
            /// Returns a notification request for task related notificatons.
            /// </summary>
            /// <param name="notification">Notification Object</param>
            /// <returns>Notification Request</returns>
            private static NotificationRequest GetTaskAfterNotification(Notification notification)
            {
                NotificationRequest request = new NotificationRequest
                {
                    Title = notification.Title + " Ending Soon!",
                    Description = notification.Description,
                    NotificationId = notification.NotificationId,
                    Android =
                    {
                        ChannelId = "todo_notify_after"
                    },
                    Schedule =
                    {
                        NotifyTime = notification.NotifyTime
                    },
                    CategoryType = NotificationCategoryType.Event,
                    ReturningData = notification.TaskId.ToString(),
                };
                return request;
            }

            /// <summary>
            /// WIP notifications for schedule based notifications.
            /// </summary>
            /// <param name="notification">Notification Object</param>
            /// <returns>Notification Request</returns>
            private static NotificationRequest GetScheduleNotification(Notification notification)
            {
                NotificationRequest request = new NotificationRequest
                {
                    Title = notification.Title,
                    Description = notification.Description,
                    NotificationId = notification.NotificationId,
                    Android =
                    {
                        ChannelId = "general_notify"
                    },
                    Schedule =
                    {
                        NotifyTime = notification.NotifyTime
                    },             
                };
                return request;
            }

            /// <summary>
            /// WIP notification for pomodoro based notifications.
            /// </summary>
            /// <param name="notification">Notification Object</param>
            /// <returns>Notification Requset</returns>
            private static NotificationRequest GetPomodoroNotification(Notification notification)
            {
                NotificationRequest request = new NotificationRequest
                {
                    Title = notification.Title,
                    Description = notification.Description,
                    NotificationId = notification.NotificationId,
                    Android =
                    {
                        ChannelId = "pomodoro_notify"
                    },
                    Schedule =
                    {
                        NotifyTime = notification.NotifyTime
                    }

                };
                return request;
            }
            #endregion
        }
    }

    /// <summary>
    /// Types to describe what the notification is for.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// For all generic notifications
        /// </summary>
        Default = 1,
        /// <summary>
        /// Notifcation Reminders for tasks
        /// </summary>
        TaskBefore = 2,
        /// <summary>
        /// Notification for when a task starts
        /// </summary>
        TaskStart = 3,
        /// <summary>
        /// Notification for when a task ends
        /// </summary>
        TaskAfter = 4,
        /// <summary>
        /// Notification for schedule based notifications
        /// </summary>
        Schedule = 5,
        /// <summary>
        /// Notifications for pomodoro based notifications
        /// </summary>
        Pomodoro = 6
    }
}
