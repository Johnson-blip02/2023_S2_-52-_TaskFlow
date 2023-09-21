using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskFlow.Model;
using TaskFlow.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static System.Net.Mime.MediaTypeNames;

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
        ItemIndex = -1;
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
                    if (item.InTrash || item.Archived) //Dont add items in the trash to the list
                        continue;

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
    /// Sets the InTrash property of the todo item to true. Creates a toast
    /// notifying the change. Refreshes the Todo item list.
    /// </summary>
    /// <param name="index">Index of the todo item to trash</param>
    /// <returns></returns>
    [RelayCommand]
    public async Task DeleteSelectedItem(int index)
    {
        TodoItems.ElementAt(index).InTrash = true;
        
        //Create and show toast
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string text = "Sent \"" + TodoItems.ElementAt(index).Title + "\" to the trash";
        var toast = Toast.Make(text, ToastDuration.Long, 14);
        await toast.Show(cancellationTokenSource.Token);

        _tm.InsertAll(TodoItems.ToList());
        LoadTodoItems();
    }

    /// <summary>
    /// Sets the Archived property of the todo item to true. Creates a toast
    /// notifying the change. Refreshes the Todo item list.
    /// </summary>
    /// <param name="index">Index of the todo item to archive</param>
    /// <returns></returns>
    [RelayCommand]
    public async Task ArchiveSelectedItem(int index)
    {
        TodoItems.ElementAt(index).Archived = true;
        
        //Create and show toast
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string text = "Archived \"" + TodoItems.ElementAt(index).Title + "\"";
        var toast = Toast.Make(text, ToastDuration.Long, 14);
        await toast.Show(cancellationTokenSource.Token);

        _tm.InsertAll(TodoItems.ToList());
        LoadTodoItems();
    }
    /// <summary>
    /// Property for the current index of the swiped item.
    /// </summary>
    public int ItemIndex { get; set; } = -1;

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
