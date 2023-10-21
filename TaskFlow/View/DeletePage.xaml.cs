using Syncfusion.Maui.DataSource;
using TaskFlow.Model;
using TaskFlow.ViewModel;
using SwipeEndedEventArgs = Syncfusion.Maui.ListView.SwipeEndedEventArgs;
using TaskFlow.Comparers;
using Syncfusion.Maui.Popup;
using CommunityToolkit.Maui.Behaviors;


namespace TaskFlow.View;

public partial class DeletePage : ContentPage
{
	public DeletePage(DeleteViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    /// <summary>
    /// Loads todo and label items from view model whenever page is about to appear on screen.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((DeleteViewModel)BindingContext).LoadTodoItems();
        ((DeleteViewModel)BindingContext).LoadLabelItems();
    }

    /// <summary>
    /// Handles the CheckBox checked changed event and updates associated todo todoItem's completion.
    /// </summary>
    /// <param name="sender">CheckBox that triggered the event</param>
    /// <param name="completed">New completion status of the todo todoItem</param>
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs completed)
    {
        var checkBox = (CheckBox)sender;
        var todoItem = checkBox.BindingContext as TodoItem;
        if (todoItem != null && completed.Value == true)
        {
            ((DeleteViewModel)BindingContext).UpdateTodoCompletion(todoItem, completed.Value);
        }
    }

    /// <summary>
    /// Clears any previous sorting and grouping for the todo list.
    /// </summary>
    private void ClearSortAndGroup()
    {
        TodoList.DataSource.SortDescriptors.Clear();
        TodoList.DataSource.GroupDescriptors.Clear();
    }

    /// <summary>
    /// Sets up the group header template for the todo list.
    /// </summary>
    private void SetupGroupHeaderTemplate()
    {
        TodoList.GroupHeaderTemplate = new DataTemplate(() =>
        {
            var grid = new Grid { Margin = 0 };

            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.White,
                FontSize = 16,
                HeightRequest = 20
            };

            label.SetBinding(Label.TextProperty, "Key");

            grid.Children.Add(label);

            return grid;
        });
    }

    /// <summary>
    /// Sets up due date grouping for the todo list.
    /// </summary>
    private void SetupDueDateGrouping()
    {
        TodoList.DataSource.GroupComparer = new DueDateGroupComparer();
        TodoList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
        {
            PropertyName = nameof(TodoItem.DueDate),
            KeySelector = (object obj) => GetDueDateGroupKey((TodoItem)obj)
        });
    }
    /// <summary>
    /// Gets the group key for a todo item based in its due date.
    /// </summary>
    /// <param name="todoItem">The todo item</param>
    /// <returns>The group key as a string representation of <see cref="DueDateGroup"/> enum.</returns>
    private string GetDueDateGroupKey(TodoItem todoItem)
    {
        var dueDate = todoItem.DueDate;

        DueDateGroup group = DueDateGroupExtension.GetDueDateGroup(dueDate);
        return group.ToFriendlyString();
    }

    /// <summary>
    /// Applies a new sort descriptor to the todo list.
    /// </summary>
    /// <param name="propertyName">The property to sort by.</param>
    /// <param name="direction">The sorting direction.</param>
    private void ApplySortDescriptor(string propertyName, ListSortDirection direction)
    {
        TodoList.DataSource.SortDescriptors.Add(new SortDescriptor()
        {
            PropertyName = propertyName,
            Direction = direction
        });
    }

    /// <summary>
    /// Opens the task popup when event is driven by setting:
    /// <code>popup.IsOpen = true</code>
    /// </summary>
    private void TodoItemMenuButton_Clicked(object sender, EventArgs e)
    {
        popup.IsOpen = true;
    }

    public void OpenPopup()
    {
        popup.IsOpen = true;
    }

    /// <summary>
    /// Resets the index of the sflist swiped item index.
    /// </summary>
    private void TodoList_SwipeStarting(object sender, EventArgs e)
    {
        ((DeleteViewModel)BindingContext).ItemIndex = -1;
    }

    /// <summary>
    /// Updates the index of the current swiped item. Called from the sfList
    /// swipe ended method.
    /// </summary>
    private void TodoList_SwipeEnded(object sender, SwipeEndedEventArgs e)
    {
        ((DeleteViewModel)BindingContext).ItemIndex = e.Index;
    }

    private ImageButton _lastPressed;

    /// <summary>
    /// Handler for moving a task to the trash when when the button is pressed.
    /// </summary>
    private void DeleteImage_Clicked(object sender, EventArgs e)
    {
        SfPopup confirmPopup = new SfPopup()
        {
            HeaderTitle = "Delete Task",
            Message = "Do you want to delete the task?",
            AutoSizeMode = PopupAutoSizeMode.Height,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AcceptButtonText = "Delete",
            DeclineButtonText = "Cancel",
            ShowFooter = true,
            AcceptCommand = new Command(ExecuteDelete),
            HeightRequest = 180,
            PopupStyle = new PopupStyle()
            {
                PopupBackground = Color.Parse("#341C4F"),
                HeaderTextColor = Colors.White,
                MessageTextColor = Colors.White,
                AcceptButtonTextColor = Colors.White,
                DeclineButtonTextColor = Colors.White
            }
        };
        confirmPopup.Closing += ResetSwipe;
        confirmPopup.Show();
        _lastPressed = (ImageButton)sender;
    }

    /// <summary>
    /// Deletes the task
    /// </summary>
    public async void ExecuteDelete()
    {
        var todoItem = _lastPressed.BindingContext as TodoItem;
        await ((DeleteViewModel)BindingContext).DeleteSelectedItem(todoItem);
    }

    /// <summary>
    /// Handler for moving a task to the archive when when the button is pressed.
    /// </summary>
    private void ArchiveImage_Clicked(object sender, EventArgs e)
    {
        SfPopup confirmPopup = new SfPopup()
        {
            HeaderTitle = "Archive Task",
            Message = "Do you want to archive the task?",
            AutoSizeMode = PopupAutoSizeMode.Height,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AcceptButtonText = "Archive",
            DeclineButtonText = "Cancel",
            ShowFooter = true,
            AcceptCommand = new Command(ExecuteArchive),
            HeightRequest = 180,
            PopupStyle = new PopupStyle()
            {
                PopupBackground = Color.Parse("#341C4F"),
                HeaderTextColor = Colors.White,
                MessageTextColor = Colors.White,
                AcceptButtonTextColor = Colors.White,
                DeclineButtonTextColor = Colors.White,
            }
        };
        confirmPopup.Closing += ResetSwipe;
        confirmPopup.Show();
        _lastPressed = (ImageButton)sender;
    }

    /// <summary>
    /// Archives the task
    /// </summary>
    public async void ExecuteArchive()
    {
        var todoItem = _lastPressed.BindingContext as TodoItem;
        await ((DeleteViewModel)BindingContext).ArchiveSelectedItem(todoItem);
    }

    /// <summary>
    /// Resets the todo list's swiped value
    /// </summary>
    private void ResetSwipe(object sender, EventArgs e)
    {
        TodoList.ResetSwipeItem(true);
    }

    #region TabNavigation
    /// <summary>
    /// Handlers for navigating to each page using the non-shell tab bar.
    /// </summary>
    private async void OnToDoTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ToDoPage");
    }

    private async void OnSchedulerTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SchedulePage");
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
    #endregion
}