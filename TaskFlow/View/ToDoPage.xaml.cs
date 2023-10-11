using Syncfusion.Maui.DataSource;
using TaskFlow.Model;
using TaskFlow.ViewModel;
using SwipeEndedEventArgs = Syncfusion.Maui.ListView.SwipeEndedEventArgs;
using TaskFlow.Comparers;
using Syncfusion.Maui.Popup;
using CommunityToolkit.Maui.Behaviors;

namespace TaskFlow.View;

public partial class ToDoPage : ContentPage
{
    readonly Color iconAccentTint = new();  // Stores accent color of application for image tints.
    private double currentRotationAngle;    // Stores current rotation angle of drop down image.

    public ToDoPage(ToDoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        iconAccentTint = Color.Parse("#7EC8BA");
        currentRotationAngle = 0;
        menuDropDownImage.Behaviors.Add(new IconTintColorBehavior { TintColor = Color.Parse("#919191") });
    }

    /// <summary>
    /// Loads todo and label items from view model whenever page is about to appear on screen.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((ToDoViewModel)BindingContext).LoadTodoItems();
        ((ToDoViewModel)BindingContext).LoadLabelItems();

        // Checking whether search/sort/filter is activated.
        ChangeMenuDropDownImageTint();
        if (filterComboBox.SelectedValue != null)
        {
            ChangeImageTint(true, filterImage);
            ChangeImageTint(true, menuFilterIcon);
        }
        if (sortComboBox.SelectedItem != null)
        {
            ChangeImageTint(true, sortImage);
            ChangeImageTint(true, menuSortIcon);
        }
        if (!string.IsNullOrEmpty(searchBar.Text))
        {
            ChangeImageTint(true, menuSearchIcon);
        }
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
            ((ToDoViewModel)BindingContext).UpdateTodoCompletion(todoItem, completed.Value);
        }
    }

    /// <summary>
    /// Handles the selection changed event of the sorting combo box. 
    /// Sorts the TodoList based on the selected sorting option.
    /// </summary>
    /// <param name="sender">The SfComboBox that triggered the event</param>
    /// <param name="e">The selected todo item from the combo box</param>
    private void SortByComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        // Change image colors.
        ChangeImageTint(false, sortImage);
        ChangeImageTint(false, menuSortIcon);
        ChangeMenuDropDownImageTint();

        // Check if a valid selection was made.
        if (e.CurrentSelection.FirstOrDefault() == null)
            return;
            
        // Extract the selected sorting option as a key-value pair.
        var selectedItem = (KeyValuePair<string, string>)e.CurrentSelection.FirstOrDefault();
        string selectedValue = selectedItem.Value;

        ClearSortAndGroup();
        
        if (selectedValue == null)
        {
            sortComboBox.SelectedItem = null;  // Reset the combo box selected sort option.
        }
        else
        {
            ChangeImageTint(true, sortImage);  // Add color tint to sort image.
            ChangeImageTint(true, menuSortIcon);

            ApplySortDescriptor(selectedValue, ListSortDirection.Ascending);

            if (selectedItem.Value == nameof(TodoItem.DueDate))
            {
                SetupGroupHeaderTemplate();
                SetupDueDateGrouping();
            }
            OnAppearing();

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
        searchBar.Unfocus();
        popup.IsOpen = true;
    }

    /// <summary>
    /// Resets the index of the sflist swiped item index.
    /// </summary>
    private void TodoList_SwipeStarting(object sender, EventArgs e)
    {
        ((ToDoViewModel)BindingContext).ItemIndex = -1;
    }

    /// <summary>
    /// Updates the index of the current swiped item. Called from the sfList
    /// swipe ended method.
    /// </summary>
    private void TodoList_SwipeEnded(object sender, SwipeEndedEventArgs e)
    {
        ((ToDoViewModel)BindingContext).ItemIndex = e.Index;
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
        await ((ToDoViewModel)BindingContext).DeleteSelectedItem(todoItem);
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
        await((ToDoViewModel)BindingContext).ArchiveSelectedItem(todoItem);
    }

    /// <summary>
    /// Resets the todo list's swiped value
    /// </summary>
    private void ResetSwipe(object sender, EventArgs e)
    {
        TodoList.ResetSwipeItem(true);
    }

    /// <summary>
    /// Clears the selection of the filter combo box by setting selected item to null.
    /// </summary>
    private void ClearFilterImageButton_Clicked(object sender, EventArgs e)
    {
        filterComboBox.SelectedItem = null;
        ChangeMenuDropDownImageTint();
    }

    /// <summary>
    /// Changes filter image color on selection changed.
    /// </summary>
    private void FilterComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        LabelItem selectedLabel = filterComboBox.SelectedItem as LabelItem;

        ChangeImageTint(false, filterImage);  
        ChangeImageTint(false, menuFilterIcon);

        if (selectedLabel != null && selectedLabel.Id!=0 )
        {
            ChangeImageTint(true,filterImage); 
            ChangeImageTint(true, menuFilterIcon);
        }

        ChangeMenuDropDownImageTint();
    }

    /// <summary>
    /// Adds or removes the color tint from an image.
    /// </summary>
    /// <param name="add">True if color tint is to be added; false if it is to be cleared.</param>
    /// <param name="image">Image whose color tint is to be changed.</param>
    private void ChangeImageTint(bool add, Image image)
    {
        image.Behaviors.Clear();
        if (add)
            image.Behaviors.Add(new IconTintColorBehavior { TintColor = iconAccentTint });
    }

    /// <summary>
    /// Changes the visibility of the options menu and animates affected elements.
    /// </summary>
    private void OptionsMenuIconBorder_Tapped(object sender, TappedEventArgs e)
    {
        bool menuOpened = ((ToDoViewModel)BindingContext).OptionsMenuOpened;

        // Animate affected elements.
        OptionsMenuTappedAnimate(menuOpened);

        //Change options menu visibility.
        ((ToDoViewModel)BindingContext).OptionsMenuOpened = !menuOpened;

    }

    /// <summary>
    /// Animates the rotation and translation of elements associated with the options menu.
    /// </summary>
    /// <param name="menuOpened">Indicates whether options menu has been opened or not.</param>
    private void OptionsMenuTappedAnimate(bool menuOpened)
    {
        // Rotate the menu icon.
        currentRotationAngle += 180;
        Task.Run(() => menuDropDownImage.RotateTo(currentRotationAngle, 200));

        if (menuOpened)
        {
            // Slide down the options menu and todo list.
            optionsMenuGrid.TranslationY = optionsMenu.Height;
            optionsMenuGrid.TranslateTo(0, 0, 200);

            TodoList.TranslationY = optionsMenu.Height;
            TodoList.TranslateTo(0, 0, 200);
        }
        else
        {
            // Slide up the options menu and todo list.
            optionsMenuGrid.TranslationY = -optionsMenu.Height;
            optionsMenuGrid.TranslateTo(0, 0, 200);

            TodoList.TranslationY = -optionsMenu.Height;
            TodoList.TranslateTo(0, 0, 200);
        }

    }

    /// <summary>
    /// Changes the tint color of the menu drop-down icon based on whether search/filter/sort has been activated.
    /// </summary>
    private void ChangeMenuDropDownImageTint()
    {
        menuDropDownImage.Behaviors.Clear();
        if (!string.IsNullOrEmpty(searchBar.Text) || sortComboBox.SelectedItem != null || filterComboBox.SelectedValue != null)
        {
            ChangeImageTint(true, menuDropDownImage);
        }
    }

    /// <summary>
    /// Changes icon color depending whether search text is empty or not..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        ChangeMenuDropDownImageTint();
        if (!string.IsNullOrEmpty(searchBar.Text))
        {
            searchBar.IconColor = iconAccentTint;
            ChangeImageTint(true, menuSearchIcon);
        }
        else
        {
            searchBar.IconColor = Colors.White;
            ChangeImageTint(false, menuSearchIcon);
        }
            
    }
}
