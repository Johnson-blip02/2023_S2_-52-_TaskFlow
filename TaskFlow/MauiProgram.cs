using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using TaskFlow.View;
using TaskFlow.ViewModel;

namespace TaskFlow;

public static class MauiProgram
{
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

#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
