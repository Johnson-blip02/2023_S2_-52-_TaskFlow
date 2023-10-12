using TaskFlow.View;

namespace TaskFlow;

public partial class AppShell : Shell
{
    public AppShell()
	{
		InitializeComponent();
        RegisterRoutes();
    }

    /// <summary>
    /// Registers the routes for navigation within the application.
    /// </summary>
    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(NewTodoPage), typeof(NewTodoPage));
        Routing.RegisterRoute(nameof(LabelPage), typeof(LabelPage));
        Routing.RegisterRoute(nameof(PomodoroSetupPage), typeof(PomodoroSetupPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); // Temporary for placeholder navigation
    }
}
