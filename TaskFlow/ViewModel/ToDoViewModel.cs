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
    private readonly IDatabase<UserProfile> _um; // UserProfileModel

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

    public UserProfile UserProfile;

    #region Constructor
    public ToDoViewModel()
    {
        _tm = App.TodoModel;
        _lm = App.LabelModel;
        _um = App.UserProfileModel;
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
        UserProfile = new();
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
    /// Loads the user's profile from the database and updates <see cref="UserProfile"/>.
    /// </summary>
    public void LoadUserProfile()
    {
        try
        {
            var profile = _um.GetData();
            if (profile != null && profile.Count == 1)
            {
                foreach (var prof in profile)
                {
                    this.UserProfile = prof;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading user profile: {ex}");
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
            UpdateUserScore(todoItem);
            LoadTodoItems();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating todo item: {ex}");
        }

    }

    // Method that should pass its test.
    public int Add(int num1, int num2)
    {
        int sum = num1 + num2;  // change to - and check that it does not pass.
        return sum;
    }

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
    /// Calls the local CalculateNewScore() method to update the local user profile's score. Updates 
    /// the database with the new user profile.
    /// </summary>
    /// <param name="todoItem">The todo item whose completed status has changed.</param>
    public void UpdateUserScore(TodoItem todoItem)
    {
        CalculateNewScore(todoItem);
        _um.Insert(this.UserProfile);
        LoadUserProfile();
    }

    /// <summary>
    /// Updates the local user profile's score based on the completed todo item's importance. 
    /// </summary>
    /// <param name="todoItem">The todo item whose completed status has changed.</param>
    /// <remarks>
    /// If todo item's completed status is true, its importance is added to the score. If false, it is subtracted.
    /// </remarks>
    private void CalculateNewScore(TodoItem todoItem)
    {
        if (todoItem.Completed)
        {
            this.UserProfile.Score += todoItem.Importance;
        }
        else
        {
            this.UserProfile.Score -= todoItem.Importance;
        }
    }
}