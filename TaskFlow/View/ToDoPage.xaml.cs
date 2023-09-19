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
    /// Loads todo items from view model whenever page is about to appear on screen.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((ToDoViewModel)BindingContext).LoadTodoItems();

    }

    /// <summary>
    /// Handles the CheckBox checked changed event and updates associated todo item's completion.
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
    /// Handles the <see cref="sortComboBox"/> selection changed event. 
    /// Sorts the TodoList based on the selected sorting option from the sort combo box.
    /// </summary>
    /// <param name="sender">SfComboBox that triggered the event</param>
    /// <param name="e">The selected item from the combo box</param>
    private void SortByComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() == null)
            return;

        // Extract the selected sorting option as a key-value pair.
        var selectedItem = (KeyValuePair<string,string>)e.CurrentSelection.FirstOrDefault();
        string selectedValue = selectedItem.Value;

        // Clear any previous sorting for todo list.
        TodoList.DataSource.SortDescriptors.Clear();

        // Apply a new sort descriptor if a valid sorting value is provided.
        if(selectedValue == null)
        {
            sortComboBox.SelectedItem = null;
        }
        else
        {
            TodoList.DataSource.SortDescriptors.Add(new Syncfusion.Maui.DataSource.SortDescriptor()
            {
                PropertyName = selectedValue,
                Direction = Syncfusion.Maui.DataSource.ListSortDirection.Ascending
            });
        }
        
    }

}