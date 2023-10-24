using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using TaskFlow.View;
using TaskFlow.ViewModel;
using System.Timers;
using Timer = System.Timers.Timer;
using TaskFlow.Model;
using TaskFlow.CustomControls;

namespace TaskFlow;

public static class MauiProgram
{
	private static Timer priorityTimer;

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
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

		builder.Services.AddSingleton<ToDoViewModel>();
        builder.Services.AddSingleton<ToDoPage>();
        builder.Services.AddSingleton<DonePage>();

        builder.Services.AddSingleton<SchedulerViewModel>();
        builder.Services.AddSingleton<CalendarPage>();
        builder.Services.AddSingleton<SchedulePage>();
        builder.Services.AddTransient<SelectPage>();

        builder.Services.AddSingleton<LabelPage>();
		builder.Services.AddSingleton<LabelViewModel>();

		priorityTimer = new Timer(1000);
		priorityTimer.Elapsed += TimerEvent;
		priorityTimer.AutoReset = true;
		priorityTimer.Start();
#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
	}

	private static void TimerEvent(Object source, ElapsedEventArgs e)
	{
		priorityTimer.Interval = 600000;
		TodoModel _tm = new TodoModel();
		_tm.CalculatePriority();
	}
}
