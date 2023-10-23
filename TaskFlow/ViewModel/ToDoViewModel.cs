using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskFlow.Model;
using TaskFlow.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TaskFlow.ViewModel;

/// <summary>
/// ViewModel for logic of the <see cref="ToDoPage"/> View.
/// </summary>
public partial class ToDoViewModel : ObservableObject
{
    private readonly IDatabase<TodoItem> _tm;    // TodoModel
    private readonly IDatabase<LabelItem> _lm;   // LabelModel
    private readonly IDatabase<DeleteHistoryList> _dh;   // DeleteModel

    private ProfileViewModel profileVM;         

    [ObservableProperty]
    public ObservableCollection<TodoItem> todoItems;

    [ObservableProperty]
    private ObservableCollection<TodoItem> doneItems;

    [ObservableProperty]
    private ObservableCollection<LabelItem> labelItems;

    [ObservableProperty]
    private IDictionary<string, string> sortItems;

    [ObservableProperty]
    public TodoItem selectedTodo;

    [ObservableProperty]
    public bool popupVisibility;

    [ObservableProperty]
    private string searchBarText;

    [ObservableProperty]
    private LabelItem selectedLabel;

    [ObservableProperty]
    private bool optionsMenuOpened;

    [ObservableProperty]
    private string labelFilterPlaceholder;

    [ObservableProperty]
    private int score;

    #region Constructor
    public ToDoViewModel(ProfileViewModel profVM)
    {
        _tm = App.TodoModel;
        _lm = App.LabelModel;
        _dh = App.DeleteModel;
        TodoItems = new ObservableCollection<TodoItem>();
        DoneItems = new ObservableCollection<TodoItem>();
        LabelItems = new ObservableCollection<LabelItem>();
        SortItems = new Dictionary<string, string>();
        SelectedLabel = new LabelItem();
        SearchBarText = string.Empty;
        OptionsMenuOpened = false;
        LoadSortDictionary();
        PopupVisibility = false;
        ItemIndex = -1;
        LabelFilterPlaceholder = string.Empty;
        this.profileVM = profVM;
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
                Score = 0;
                foreach (var item in itemsList)
                {
                    if (item.InTrash || item.Archived)  // Don't add items in the trash to the list
                        continue;

                    if(item.Completed == true)
                    {
                        DoneItems.Add(item);
                        Score += item.Importance;  // Increment Score by item's importance
                    } 
                    else 
                    {                    
                        TodoItems.Add(item);
                    }

                }
            }
            if (SearchBarText != string.Empty || SelectedLabel != null)
                SearchAndLabelFilter();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading todo items: {ex}");
        }
    }

    /// <summary>
    /// Loads label items from the database and updates the <see cref="LabelItems"/> collection.
    /// </summary>
    public void LoadLabelItems()
    {
        try
        {
            var labelsList = _lm.GetData();
            LabelItems.Clear();

            if (labelsList != null && labelsList.Count > 0)
            {
                foreach (var label in labelsList)
                    LabelItems.Add(label);
                LabelFilterPlaceholder = "Filter by label";
            }
            else
            {
                LabelFilterPlaceholder = "No labels to filter";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading label items: {ex}");
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
        ((DeleteModel)_dh).SetupDeleteTime(todoItem);

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
#if DEBUG
    // Method that should pass its test.
    public int Add(int num1, int num2)
    {
        int sum = num1 + num2;  // change to - and check that it does not pass.
        return sum;
    }
#endif
    /// <summary>
    /// Calls the <see cref="SearchAndLabelFilter"/> method when the <see cref="SearchBarText"/> property changes.
    /// </summary>
    /// <param name="value">The search string input.</param>
    partial void OnSearchBarTextChanged(string value)
    {
        SearchAndLabelFilter();
    }

    /// <summary>
    /// Calls the <see cref="SearchAndLabelFilter"/> method when the <see cref="SelectedLabel"/> property changes.
    /// </summary>
    /// <param name="value">The selected label item.</param>
    partial void OnSelectedLabelChanged(LabelItem value)
    {
        SearchAndLabelFilter();
    }

    /// <summary>
    /// Clears the current Todo Item list. Filters by calling the <see cref="getSearchedAndFilteredItems"/> method.
    /// Adds retrieved items to list.
    /// </summary>
    public void SearchAndLabelFilter()
    {
        TodoItems.Clear();

        // Get items that match search query and selected label
        var filteredItems = getSearchedAndFilteredItems();

        foreach (var item in filteredItems)
        {
            TodoItems.Add(item);
        }
    }

    /// <summary>
    /// Retrieves a list of todo items from the database based on the selected label and search criteria.
    /// </summary>
    /// <returns>A list of TodoItem objects that match the specified criteria.</returns>
    /// <remarks>
    /// Matches searchbar text to todo item titles if search text is not null or empty.
    /// Filters by selected label if label is not null.
    /// </remarks>
    private List<TodoItem> getSearchedAndFilteredItems()
    {
        var query = _tm.GetData().Where(item =>
            (string.IsNullOrEmpty(SearchBarText) || item.Title.Trim().ToLower().Contains(SearchBarText.Trim().ToLower())) &&
            ((SelectedLabel == null || SelectedLabel.Id == 0) || item.Labels.Contains(SelectedLabel)) &&
            !item.InTrash &&
            !item.Archived &&
            !item.Completed
        );

        return query.ToList();
    }

    /// <summary>
    /// Updates the profile view model's score and completed items count when the <see cref="Score"/> property changes.
    /// </summary>
    /// <param name="value">The new score.</param>
    partial void OnScoreChanged(int value)
    {
        profileVM.Score = value;
        profileVM.CompletedItemsCount = DoneItems.Count;
    }
}