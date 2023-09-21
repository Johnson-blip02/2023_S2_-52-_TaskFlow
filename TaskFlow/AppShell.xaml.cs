using System.Windows.Input;
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
        Routing.RegisterRoute(nameof(SelectPage), typeof(SelectPage));
        Routing.RegisterRoute(nameof(LabelPage), typeof(LabelPage));
    }
}
