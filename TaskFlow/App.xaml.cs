using TaskFlow.Model;

namespace TaskFlow;

public partial class App : Application
{
	public static TodoModel TodoModel { get; private set; }
	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NGaF1cWGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX1ccHFURmJdWUR3WA==");

        InitializeComponent();

		TodoModel = new TodoModel();

		MainPage = new AppShell();
	}
}
