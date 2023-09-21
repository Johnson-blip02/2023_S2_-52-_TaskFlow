using TaskFlow.Model;
using TaskFlow.ViewModel;


namespace TaskFlow.View;

public partial class SelectPage : ContentPage
{

    public SelectPage(SchedulerViewModel vm)
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
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var button = sender as Border;
        var todoItem = button.BindingContext as TodoItem;
        ((SchedulerViewModel)BindingContext).AddTodo(todoItem);
    }

    //private async void onAddTapped() {
    //    ((SchedulerViewModel)BindingContext).ScheduleNewEvent = ;
    //}
}