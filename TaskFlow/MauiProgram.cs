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
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<NewTodoViewModel>();
		builder.Services.AddSingleton<NewTodoPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
