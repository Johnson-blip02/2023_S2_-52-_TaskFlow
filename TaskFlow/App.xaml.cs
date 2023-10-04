using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using TaskFlow.Model;

namespace TaskFlow;

public partial class App : Application
{
	// Single instances of model classes to be used by view models within the application.
	public static TodoModel TodoModel { get; private set; }
	public static LabelModel LabelModel { get; private set; }

	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjczOTAxMEAzMjMzMmUzMDJlMzBoakN0elZhSkJxNUIwSlk3R1JZM2tiRDlyUTlSMWllU2JLSUpMMU9EYlFNPQ==");

        InitializeComponent();
		ModifyEntry();

		TodoModel = new TodoModel();
		LabelModel = new LabelModel();

		MainPage = new AppShell();
	}

    /// <summary>
    /// Customize the entry handler mapping to have no underlines across the app.
    /// </summary>
    private void ModifyEntry()
	{
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
		{
			handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
		});
	}
}
