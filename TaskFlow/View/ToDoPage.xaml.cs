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
    /// Sorts the TodoList based on the selected item from the sort combo box.
    /// </summary>
    /// <param name="sender">SfComboBox that triggered the event</param>
    /// <param name="e">Selected item from the combo box</param>
    private void SortByComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as string;

        TodoList.DataSource.SortDescriptors.Clear();

        // Applying a sort descriptor to the TodoList if a valid sorting option is selected.
        if (selectedItem != null && !selectedItem.Equals("None")) 
        {
            TodoList.DataSource.SortDescriptors.Add(new Syncfusion.Maui.DataSource.SortDescriptor()
            {
                PropertyName = selectedItem,
                Direction = Syncfusion.Maui.DataSource.ListSortDirection.Ascending
            });
        }
        
    }

}