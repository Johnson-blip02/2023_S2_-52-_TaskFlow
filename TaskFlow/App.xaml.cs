using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using TaskFlow.Model;

namespace TaskFlow;

public partial class App : Application
{
	public static TodoModel TodoModel { get; private set; }
	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NGaF1cWGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX1ccHFURmJdWUR3WA==");

        InitializeComponent();

		ModifyHandlers();

		TodoModel = new TodoModel();

		MainPage = new AppShell();
	}

	private void ModifyHandlers()
	{
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
		{
			handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
		});
	}
}
