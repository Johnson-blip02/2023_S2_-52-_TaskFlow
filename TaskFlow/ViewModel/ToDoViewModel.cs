using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Scheduler;
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

    [ObservableProperty]
    private ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private ObservableCollection<string> sortItems;

    #region Constructor
    public ToDoViewModel()
    {
        _tm = App.TodoModel;
        TodoItems = new ObservableCollection<TodoItem>();
        SortItems = new ObservableCollection<string>();
        LoadSortItems();
        this.GenerateAppointments();
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
    /// Loads items by which todo list can be sorted by, and updates local <see cref="SortItems"/> collection.
    /// To add a new condition for sorting, the string must be an exact match to the corresponding <see cref="TodoItem"/> property.
    /// </summary>
    public void LoadSortItems()
    {
        List<string> sortItems = new List<string>
        {
            "None",
            "Title",
            "DueDate"
        };

        foreach(string item in sortItems)
            SortItems.Add(item);
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

    #region Properties
    public ObservableCollection<SchedulerAppointment> Events { get; set; }
    #endregion

    #region Method
    private void GenerateAppointments()
    {
        this.Events = new ObservableCollection<SchedulerAppointment>();

        //Adding the schedule appointments in the schedule appointment collection.
        this.Events.Add(new SchedulerAppointment
        {
            StartTime = DateTime.Now.Date.AddHours(10),
            EndTime = DateTime.Now.Date.AddHours(11),
            Subject = "Client Meeting",
            Background = new SolidColorBrush(Color.FromArgb("#FF8B1FA9")),
        });
    }

    #endregion
}
