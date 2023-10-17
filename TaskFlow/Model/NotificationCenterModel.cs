using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using SQLiteNetExtensions.Extensions;
using TaskFlow.View;
using TaskFlow.ViewModel;

namespace TaskFlow.Model
{
    /// <summary>
    /// Class for scheduling and getting all scheduled notifications. Notifications are stored in the database
    /// so they can be restored when the app is closed.
    /// </summary>
    public class NotificationCenterModel : Database<Notification>
    {
        protected override void CreateTableAsync()
        {
            dbConn.CreateTable<Notification>();
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
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
        
        /// <summary>
        /// Restores all notifications from notifications stored in the database. Should only be
        /// called when the app is opened or when the app starts after a phone reboot.
        /// </summary>
        /// <returns></returns>
        public int RestoreNotifcations()
        {
            if (LocalNotificationCenter.Current.GetPendingNotificationList != null)
                return 0;

            int count = 0;

            foreach (var notification in GetData())
            {
                if (notification.NotifyTime < DateTime.Now) //Remove all old notifications from the database
                    Delete(notification);
                else if (ScheduleNotification(notification))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Schedule a notification
        /// </summary>
        /// <param name="notification">Notification to schedule</param>
        /// <returns></returns>
        public bool ScheduleNotification(Notification notification)
        {     
            Insert(notification);

            NotificationRequest request = Notification.NotificationRequestGenerator.GetNotification(notification);
            LocalNotificationCenter.Current.Show(request);

            return true;
        }

        /// <summary>
        /// Handler rum when a notification button is tapped. Actions are based of the returned ActionID 
        /// of the event.
        /// </summary>
        /// <param name="e"></param>
        private async void Current_NotificationActionTapped(NotificationActionEventArgs e)
        {
            switch (e.ActionId)
            {
                case 100: //Dismiss Button
                    LocalNotificationCenter.Current.Cancel(e.Request.NotificationId);
                    break;
                                   
                case 101: //View Task Button
                    //If the app isnt open, we need to open the todo page because otherwise we cannot get values
                    if(!App.Current.MainPage.IsLoaded)
                        await App.Current.MainPage.Navigation.PushAsync(new ToDoPage(new ToDoViewModel()));

                    App.Current.MainPage.Loaded += (z, o) => //Now that we have a page open we can now open the popup
                    {
                        TodoItem item = App.TodoModel.GetData().Find(x => x.Id == int.Parse(e.Request.ReturningData));
                        ToDoPage.VM.SetSelectedItem(item);
                        ((ToDoPage)ToDoPage.page).OpenPopup();
                    };

                    break;
            }
        }
    }
}
