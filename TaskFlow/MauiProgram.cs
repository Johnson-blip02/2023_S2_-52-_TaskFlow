using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using TaskFlow.View;
using TaskFlow.ViewModel;
using System.Timers;
using Timer = System.Timers.Timer;
using TaskFlow.Model;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using TaskFlow.CustomControls;

namespace TaskFlow;

public static class MauiProgram
{
	private static Timer priorityTimer;
    public static ToDoViewModel todoVM;

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
        #region Notification Settings
            //Notifications uses Plugin.LocalNotification.
            .UseLocalNotification(config =>
			{
                //Notifcation actions (buttons) for event type notifications.
                config.AddCategory(new NotificationCategory(NotificationCategoryType.Event)
                {
                    ActionList = new HashSet<NotificationAction>(new List<NotificationAction>()
                    {
                        //Creates a dismiss button. When pressed will have an action id of 100
                        new NotificationAction(100) 
                        {
                            Title = "Dismiss",
                        },
                        //Creates a button for viewing the task. When pressed will have an action id of 101
                        new NotificationAction(101)
                        {
                            Title = "View Task",
                            Android =
                            {
                                LaunchAppWhenTapped = true,
                            }
                        }
                    })
                });
                //Android channels for notifications. An android requirement. Allows notifications bound to a
                //channel to be grouped together and have their own notification settings. Ie. control over types
                //of notifications that can toggled on or off in the app settings.
                config.AddAndroid(android =>
                {
                    android.AddChannel(new NotificationChannelRequest
                    {
                        Id = $"todo_notify_before",
                        Name = "Task Reminders",
                        Description = "Notify of upcoming tasks and when a task starts",
                    });
					android.AddChannel(new NotificationChannelRequest
                    {
                        Id = $"todo_notify_end",
                        Name = "Task End Notification",
                        Description = "Notify of a task's ending",
                    });
                    android.AddChannel(new NotificationChannelRequest
                    {
                        Id = $"pomodoro_notify",
                        Name = "Pomodoro Timer",
                        Description = "Allow pomodoro timer notifications",
                    });
                    android.AddChannel(new NotificationChannelRequest
                    {
                        Id = $"general_notify",
                        Name = "General Notifications",
                        Description = "Allow general app notifications",
                    });
                });
            })
        #endregion
            .UseMauiCommunityToolkit()
			.ConfigureMauiHandlers(handlers =>
			{
				handlers.AddHandler<SearchBar, CustomSearchBarHandler>();
			})
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<NewTodoViewModel>();
		builder.Services.AddTransient<NewTodoPage>();

		builder.Services.AddSingleton<TodoPopup>();

        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<ToDoViewModel>();
        builder.Services.AddSingleton<ToDoPage>();
        builder.Services.AddSingleton<DonePage>();
        builder.Services.AddSingleton<SchedulerViewModel>();
        builder.Services.AddSingleton<CalendarPage>();
        builder.Services.AddSingleton<SchedulePage>();
        builder.Services.AddSingleton<LabelPage>();
		    builder.Services.AddSingleton<LabelViewModel>();
        builder.Services.AddSingleton<DeletePage>();
        builder.Services.AddSingleton<DeleteViewModel>();		

        //Timer to recalculate todo priorities
		priorityTimer = new Timer(1000); //Initally set time to 1 second so when the app starts calculation takes place immediatly
		priorityTimer.Elapsed += TimerEvent;
		priorityTimer.AutoReset = true;
		priorityTimer.Start();


#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
	}

    /// <summary>
    /// Event handler for the priority timer. Recalculates todo priorities every 600 seconds.
    /// </summary>
	private static void TimerEvent(Object source, ElapsedEventArgs e)
	{
		priorityTimer.Interval = 600000;
		TodoModel _tm = new TodoModel();
        DeleteModel _dm = new DeleteModel();
		_tm.CalculatePriority();
        _dm.AutoDelete();
	}
}
