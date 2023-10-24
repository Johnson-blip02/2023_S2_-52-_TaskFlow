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
        Routing.RegisterRoute(nameof(PomodoroSetupPage), typeof(PomodoroSetupPage));
        Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
        Routing.RegisterRoute(nameof(DeletePage), typeof(DeletePage));
        Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
        Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
        Routing.RegisterRoute(nameof(DonePage), typeof(DonePage));
        Routing.RegisterRoute(nameof(ArchivePage), typeof(ArchivePage));
        Routing.RegisterRoute(nameof(ToDoPage), typeof(ToDoPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); // Temporary for placeholder navigation
    }

}
