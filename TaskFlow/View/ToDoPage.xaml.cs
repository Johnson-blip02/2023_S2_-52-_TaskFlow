using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.ListView;
using TaskFlow.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class ToDoPage : ContentPage
{
    public ToDoPage(ToDoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    /// <summary>
    /// Loads todo items from view model whenever page is about to appear on screen
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        ((ToDoViewModel)BindingContext).LoadTodoItems();
    }

    /// <summary>
    /// Handles the CheckBox checked changed event and updates associated todo item's completion
    /// </summary>
    /// <param name="sender">CheckBox that triggered the event</param>
    /// <param name="completed">New completion status of the todo item</param>
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs completed)
    {
        var checkBox = (CheckBox)sender;
        var todoItem = checkBox.BindingContext as TodoItem;
        if (todoItem != null)
        {
            // Update completion status using ViewModel.
            ((ToDoViewModel)BindingContext).UpdateTodoCompletion(todoItem, completed.Value);
        }
    }

    /// <summary>
    /// Opens the task popup when event is driven by setting:
    /// <code>popup.IsOpen = true</code>
    /// </summary>
    private void TodoItemMenuButton_Clicked(object sender, EventArgs e)
    {
        popup.IsOpen = true;
    }
}