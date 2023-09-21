using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using TaskFlow.Model;
using TaskFlow.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace TaskFlow.ViewModel;

/// <summary>
/// ViewModel for logic of the <see cref="ToDoPage"/> View.
/// </summary>
public partial class ToDoViewModel : ObservableObject
{
    private readonly TodoModel _tm; // TodoModel

    [ObservableProperty]
    public ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private ObservableCollection<TodoItem> doneItems;

    [ObservableProperty]
    private IDictionary<string, string> sortItems;

    [ObservableProperty]
    public TodoItem selectedTodo;

    [ObservableProperty]
    public bool popupVisibility;

    #region Constructor
    public ToDoViewModel()
    {
        _tm = App.TodoModel;
        TodoItems = new ObservableCollection<TodoItem>();
        DoneItems = new ObservableCollection<TodoItem>();
        SortItems = new Dictionary<string, string>();
        LoadSortDictionary();
        PopupVisibility = false;
        ItemIndex = -1;
    }

    #endregion

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
                DoneItems.Clear();
                foreach (var item in itemsList)
                {
                    if (item.InTrash || item.Archived)  //Don't add items in the trash to the list
                        continue;

                    if(item.Completed == true)
                    {
                        DoneItems.Add(item);
                    } 
                    else 
                    {                    
                        TodoItems.Add(item);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading todo items: {ex}");
        }
    }

    /// <summary>
    /// Loads the available sorting options for the todo list and updates the local SortItems dictionary.
    /// Each element in the dictionary consists of a key for its representation in the sort combo box, 
    /// and a value corresponding to the todo item property used for sorting.
    /// </summary>
    public void LoadSortDictionary()
    {
        SortItems = new Dictionary<string, string>()
        {
            { "Date created", null },  // set to null to clear the sort.
            { "Task Title", nameof(TodoItem.Title) },
            { "Deadline", nameof(TodoItem.DueDate) },
            { "Importance" , nameof(TodoItem.Importance )}
        };
    }

    /// <summary>
    /// Navigates to the <see cref="NewTodoPage"/> view
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task GoToNewTaskPage()
    {
        await Shell.Current.GoToAsync(nameof(NewTodoPage));
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
    /// <param name="todoItem">The todo item to trash</param>
    /// <returns></returns>
    [RelayCommand]
    public async Task DeleteSelectedItem(TodoItem todoItem)
    {
        for (int i = 0; i < TodoItems.Count; i++)
            if (TodoItems.ElementAt(i).Id == todoItem.Id)
                TodoItems.ElementAt(i).InTrash = true;

        //Create and show toast
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string text = "Sent \"" + todoItem.Title + "\" to the trash";
        var toast = Toast.Make(text, ToastDuration.Long, 14);
        await toast.Show(cancellationTokenSource.Token);

        _tm.InsertAll(TodoItems.ToList());
        LoadTodoItems();
    }

    /// <summary>
    /// Sets the Archived property of the todo item to true. Creates a toast
    /// notifying the change. Refreshes the Todo item list.
    /// </summary>
    /// <param name="todoItem">The todo item to archive</param>
    /// <returns></returns>
    [RelayCommand]
    public async Task ArchiveSelectedItem(TodoItem todoItem)
    {
        for (int i = 0; i < TodoItems.Count; i++)
            if (TodoItems.ElementAt(i).Id == todoItem.Id)
                TodoItems.ElementAt(i).Archived = true;

        //Create and show toast
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string text = "Archived \"" + todoItem.Title + "\"";
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
            todoItem.Completed = completed;
            _tm.Insert(todoItem);
            LoadTodoItems();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating todo item: {ex}");
        }

    }


}
