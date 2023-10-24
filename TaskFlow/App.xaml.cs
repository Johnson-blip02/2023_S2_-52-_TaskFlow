#if ANDROID
using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif
using TaskFlow.Model;

namespace TaskFlow;

public partial class App : Application
{
	// Single instances of model classes to be used by view models within the application.
	public static IDatabase<TodoItem> TodoModel { get; set; }
	public static IDatabase<LabelItem> LabelModel { get; set; }
    public static IDatabase<ScheduledTime> ScheduledTimeModel { get; set; }

    public static IDatabase<Day> DayModel { get; set; }

	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjczOTAxMEAzMjMzMmUzMDJlMzBoakN0elZhSkJxNUIwSlk3R1JZM2tiRDlyUTlSMWllU2JLSUpMMU9EYlFNPQ==");

        InitializeComponent();
		ModifyEntry();

		TodoModel = new TodoModel();
		LabelModel = new LabelModel();
		DayModel = new DayModel();
        ScheduledTimeModel = new ScheduledTimeModel();

        MainPage = new AppShell();
	}

	/// <summary>
	/// Customize the entry handler mapping to have no underlines across the app.
	/// </summary>
	private void ModifyEntry()
	{
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
			});
	}
}
