#if ANDROID
using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using TaskFlow.Model;

namespace TaskFlow;

public partial class App : Application
{
	// Single instances of model classes to be used by view models within the application.
    public static NotificationCenterModel NotificationCenterModel { get; private set; }
	public static IDatabase<TodoItem> TodoModel { get; set; }
	public static IDatabase<LabelItem> LabelModel { get; set; }
	public static IDatabase<DeleteHistoryList> DeleteModel { get; set;}

	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjcxNDY1M0AzMjMzMmUzMDJlMzBQam9aVStLUmQxaE81andnQ2NzQU50RVZJcXpmRkl6dS9pbDlpdDNkT0drPQ==");

        InitializeComponent();
		ModifyEntry();


        NotificationCenterModel = new NotificationCenterModel();
        _ = NotificationCenterModel.RestoreNotifcations();
        TodoModel = new TodoModel();
        LabelModel = new LabelModel();
		DeleteModel = new DeleteModel();

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
