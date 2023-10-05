using TaskFlow.View;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class CalendarPage : ContentPage
{

    public CalendarPage(SchedulerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    /// <summary>
    /// Loads todo items from view model whenever page is about to appear on screen.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((SchedulerViewModel)BindingContext).LoadTodoItems();
        ((SchedulerViewModel)BindingContext).GenerateAppointments();
    }
}

