using Syncfusion.Maui.DataSource;
using TaskFlow.Model;
using TaskFlow.ViewModel;
using SwipeEndedEventArgs = Syncfusion.Maui.ListView.SwipeEndedEventArgs;
using TaskFlow.Comparers;
using Syncfusion.Maui.Popup;
using CommunityToolkit.Maui.Behaviors;

namespace TaskFlow.View;

/// <summary>
/// Page for the trash bin.
/// </summary>
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
        if (todoItem != null)
        {
            ((DeleteViewModel)BindingContext).UpdateTodoCompletion(todoItem, completed.Value);
        }
    }

    /// <summary>
    /// Opens the task popup when event is driven by setting:
    /// <code>popup.IsOpen = true</code>
    /// </summary>
    private void TodoItemMenuButton_Clicked(object sender, EventArgs e)
    {
        contextMenu.IsOpen = true;
        UpdateDeleteTime();
        contextMenu.ShowRelativeToView((ImageButton)sender, PopupRelativePosition.AlignToLeftOf);
    }

    /// <summary>
    /// Shows a popup for showing the task info from the popup view button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewButton_Clicked(object sender, EventArgs e)
    {
        contextMenu.IsOpen = false;
        popup.IsOpen = true;
    }

    /// <summary>
    /// Shows popup for restoring a task from the popup restore button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RestoreButton_Clicked(object sender, EventArgs e)
    {
        contextMenu.IsOpen = false;
        ExecuteRestore();
    }

    /// <summary>
    /// Shows popup for archiving a task from the popup archive button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ArchiveButton_Clicked(object sender, EventArgs e)
    {
        contextMenu.IsOpen = false; 
        SfPopup confirmPopup = new SfPopup()
        {
            HeaderTitle = "Archive Task",
            Message = "Do you want to archive the task?",
            AutoSizeMode = PopupAutoSizeMode.Height,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AcceptButtonText = "Archive",
            DeclineButtonText = "Cancel",
            ShowFooter = true,
            AcceptCommand = new Command(ExecuteButtonArchive),
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
    }

    /// <summary>
    /// Shows popup for deleting a task from the popup delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        contextMenu.IsOpen = false;
        SfPopup confirmPopup = new SfPopup()
        {
            HeaderTitle = "Permanently Delete Task",
            Message = "Do you want to permanently delete the task?",
            AutoSizeMode = PopupAutoSizeMode.Height,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AcceptButtonText = "Delete",
            DeclineButtonText = "Cancel",
            ShowFooter = true,
            AcceptCommand = new Command(ExecuteButtonDelete),
            HeightRequest = 200,
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
    }

    /// <summary>
    /// Opens the task view popup
    /// </summary>
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
    /// Shows a popup for confirming the action.
    /// </summary>
    private void DeleteImage_Clicked(object sender, EventArgs e)
    {
        SfPopup confirmPopup = new SfPopup()
        {
            HeaderTitle = "Permanently Delete Task",
            Message = "Do you want to permanently delete the task?",
            AutoSizeMode = PopupAutoSizeMode.Height,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AcceptButtonText = "Delete",
            DeclineButtonText = "Cancel",
            ShowFooter = true,
            AcceptCommand = new Command(ExecuteDelete),
            HeightRequest = 200,
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
    /// Deletes the task using the selected task.
    /// </summary>
    public async void ExecuteDelete()
    {
        var todoItem = _lastPressed.BindingContext as TodoItem;
        await ((DeleteViewModel)BindingContext).DeleteSelectedItem(todoItem);
    }

    /// <summary>
    /// Changes the context menu's delete time info to the time of the currently selected task.
    /// </summary>
    public void UpdateDeleteTime()
    {
        ((DeleteViewModel)BindingContext).UpdateDeleteTime(((DeleteViewModel)BindingContext).SelectedTodo);
    }

    /// <summary>
    /// Deletes the task using the context menu button
    /// </summary>
    public async void ExecuteButtonDelete()
    {
        await ((DeleteViewModel)BindingContext).DeleteSelectedItem(((DeleteViewModel)BindingContext).SelectedTodo);
    }
    /// <summary>
    /// Deletes the task using the context menu button
    /// </summary>
    public async void ExecuteButtonArchive()
    {
        await ((DeleteViewModel)BindingContext).ArchiveSelectedItem(((DeleteViewModel)BindingContext).SelectedTodo);
    }
    /// <summary>
    /// Restores the task using the context menu button
    /// </summary>
    public async void ExecuteRestore()
    {
        await ((DeleteViewModel)BindingContext).RestoreSelectedItem(((DeleteViewModel)BindingContext).SelectedTodo);
    }

    /// <summary>
    /// Handler for moving a task to the archive when when the button is pressed.
    /// Creates a popup or confirming the action. 
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
    /// Archives the currently selected task
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