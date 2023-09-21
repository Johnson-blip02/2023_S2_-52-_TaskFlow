using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using TaskFlow.View;
using TaskFlow.ViewModel;
using System.Timers;
using Timer = System.Timers.Timer;
using TaskFlow.Model;

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
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<NewTodoViewModel>();
		builder.Services.AddTransient<NewTodoPage>();
		builder.Services.AddSingleton<ToDoViewModel>();
		builder.Services.AddSingleton<ToDoPage>();
		builder.Services.AddSingleton<LabelPage>();
		builder.Services.AddSingleton<LabelViewModel>();

		priorityTimer = new Timer(10000);
		priorityTimer.Elapsed += TimerEvent;
		priorityTimer.AutoReset = true;
		priorityTimer.Enabled = true;

#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
	}

	private static void TimerEvent(Object source, ElapsedEventArgs e)
	{
		TodoModel _tm = new TodoModel();
		_tm.CalculatePriority();
	}
}
