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

	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjcxNDY1M0AzMjMzMmUzMDJlMzBQam9aVStLUmQxaE81andnQ2NzQU50RVZJcXpmRkl6dS9pbDlpdDNkT0drPQ==");

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
#if ANDROID
			handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
			});
	}
}
