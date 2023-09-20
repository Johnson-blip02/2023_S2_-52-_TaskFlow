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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((SchedulerViewModel)BindingContext).LoadTodoItems();

    }
}

