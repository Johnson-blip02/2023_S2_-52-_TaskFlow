using TaskFlow.View;
using TaskFlow.ViewModel;

namespace TaskFlow;

public partial class AppShell : Shell
{
    public AppShell()
	{
        InitializeComponent();
        RegisterRoutes();

        // Load new profile page at startup to get values
        profilePage.Content = new ProfilePage(new ProfileViewModel());
    }

    /// <summary>
    /// Registers the routes for navigation within the application.
    /// </summary>
    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(NewTodoPage), typeof(NewTodoPage));
        Routing.RegisterRoute(nameof(LabelPage), typeof(LabelPage));

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); // Temporary for placeholder navigation
    }

}
