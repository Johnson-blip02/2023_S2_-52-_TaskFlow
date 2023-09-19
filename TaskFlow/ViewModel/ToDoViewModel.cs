using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskFlow.Model;
using TaskFlow.View;

namespace TaskFlow.ViewModel;

/// <summary>
/// ViewModel for logic of the <see cref="ToDoPage"/> View.
/// </summary>
public partial class ToDoViewModel : ObservableObject
{
    private readonly TodoModel _tm; // TodoModel

    private readonly NewTodoPage _newTodoPage;

    [ObservableProperty]
    public ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    public TodoItem selectedTodo;

    [ObservableProperty]
    public bool popupVisibility;

    public ToDoViewModel(NewTodoPage newTodoPage)
    {
        _tm = App.TodoModel;
        _newTodoPage = newTodoPage;
        TodoItems = new ObservableCollection<TodoItem>();
        PopupVisibility = false;
    }

    /// <summary>
    /// Loads todo items from the database and updates the <see cref="TodoItems"/> collection
    /// </summary>
    public void LoadTodoItems()
    {
        try
        {
            var itemsList = _tm.GetData();

            if (itemsList != null && itemsList.Count > 0)
            {
                TodoItems.Clear();
                foreach (var item in itemsList)
                {
                    TodoItems.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading todo items: {ex}");
        }
    }

    /// <summary>
    /// Navigates to the <see cref="NewTodoPage"/> view
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task GoToNewTaskPage()
    {
        await App.Current.MainPage.Navigation.PushAsync(_newTodoPage);
    }

    /// <summary>
    /// Updates the todo items from the Observable Collection,
    /// refreshes the displayed list of items, finally sets the selected
    /// item to last updated item.
    /// </summary>
    [RelayCommand]
    public void RefreshTodo(TodoItem todo)
    {
        _tm.InsertAll(TodoItems.ToList());
        LoadTodoItems();
        SetSelectedItem(todo);
    }
    
    /// <summary>
    /// Sets the lists selected item to the specified todo object.
    /// </summary>
    /// <param name="selected">Todo item to set as selected</param>
    [RelayCommand]
    public void SetSelectedItem(TodoItem selected)
    {
        SelectedTodo = selected;
        PopupVisibility = !PopupVisibility;
    }

    /// <summary>
    /// Updates a todo item's completion status in the database.
    /// </summary>
    /// <param name="todoItem">TodoItem to be updated</param>
    /// <param name="completed">New completion status of the todo item</param>
    public void UpdateTodoCompletion(TodoItem todoItem, bool completed)
    {
        try
        {
            if (TodoItems.Contains(todoItem))
            {
                todoItem.Completed = completed;
                _tm.Insert(todoItem);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating todo item: {ex}");
        }

    }
}
