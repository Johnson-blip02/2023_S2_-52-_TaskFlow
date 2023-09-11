using System.Windows.Input;
using TaskFlow.View;

namespace TaskFlow;

public partial class AppShell : Shell
{
    public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(NewTodoPage), typeof(NewTodoPage));
    }
}
