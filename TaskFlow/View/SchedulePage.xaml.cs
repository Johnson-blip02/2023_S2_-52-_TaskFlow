using TaskFlow.View;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class SchedulePage : ContentPage
{

    public SchedulePage(SchedulerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((SchedulerViewModel)BindingContext).LoadTodoItems();
        ((SchedulerViewModel)BindingContext).GenerateAppointments();
    }
}

