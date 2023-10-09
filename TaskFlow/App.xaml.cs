#if ANDROID
using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif
using TaskFlow.Model;

namespace TaskFlow;

public partial class App : Application
{
	// Single instances of model classes to be used by view models within the application.
	public static TodoModel TodoModel { get; private set; }
	public static LabelModel LabelModel { get; private set; }

	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9ed3RWQmBcVUZ3Xks=");

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
