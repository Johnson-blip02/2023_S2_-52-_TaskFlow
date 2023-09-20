using TaskFlow.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class DonePage : ContentPage
{
    public DonePage(ToDoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

    }

    /// <summary>
    /// Loads todo items from view model whenever page is about to appear on screen.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ToDoViewModel)BindingContext).LoadTodoItems();

    }

    /// <summary>
    /// Handles the CheckBox checked changed event and updates associated todo todoItem's completion.
    /// </summary>
    /// <param name="sender">CheckBox that triggered the event</param>
    /// <param name="completed">New completion status of the todo todoItem</param>
    private async void DoneCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs completed)
    {
        var checkBox = (CheckBox)sender;
        var todoItem = checkBox.BindingContext as TodoItem;
        if (todoItem != null && completed.Value == false)
        {
            // Update completion status using ViewModel.
            await ((ToDoViewModel)BindingContext).UpdateTodoCompletion(todoItem, completed.Value);
        }
    }

    private async void OnToDoTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ToDoPage");
    }

    private async void OnSchedulerTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnCalendarTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CalendarPage");
    }

    private async void OnTimerTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnNotesTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}