using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskFlow.Model;
using TaskFlow.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TaskFlow.ViewModel
{
    public partial class DeleteViewModel : ObservableObject
    {
        private readonly IDatabase<TodoItem> _tm;    // TodoModel
        private readonly IDatabase<LabelItem> _lm;   // LabelModel
        private readonly IDatabase<DeleteHistoryList> _dm;   // DeleteModel

        private ProfileViewModel profileVM;

        [ObservableProperty]
        public ObservableCollection<TodoItem> todoItems;

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
        private string deleteTime;

        [ObservableProperty]
        private bool optionsMenuOpened;

        [ObservableProperty]
        private string labelFilterPlaceholder;

        [ObservableProperty]
        private Rect contextAlignment;

        #region Constructor
        public DeleteViewModel()
        {
            _tm = App.TodoModel;
            _lm = App.LabelModel;
            _dm = App.DeleteModel;

            DeleteTime = "Automatically Deletes in 20 Days";

            TodoItems = new ObservableCollection<TodoItem>();
            LabelItems = new ObservableCollection<LabelItem>();
            SortItems = new Dictionary<string, string>();
            SelectedLabel = new LabelItem();
            SearchBarText = string.Empty;
            OptionsMenuOpened = false;
            PopupVisibility = false;
            ItemIndex = -1;
            LabelFilterPlaceholder = string.Empty;
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

                TodoItems.Clear();
                if (itemsList != null && itemsList.Count > 0)
                {
                    foreach (var item in itemsList)
                    {
                        if (item.InTrash)
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
        /// Permantly deletes the selected task. Creates a toast
        /// notifying the change. Refreshes the Todo item list.
        /// </summary>
        /// <param name="todoItem">The todo item to trash</param>
        /// <returns></returns>
        [RelayCommand]
        public async Task DeleteSelectedItem(TodoItem todoItem)
        {
            //Create and show toast
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Permantly Deleted \"" + todoItem.Title + "\"";
            var toast = Toast.Make(text, ToastDuration.Long, 14);
            await toast.Show(cancellationTokenSource.Token);

            _tm.Delete(todoItem);
            LoadTodoItems();
        }

        /// <summary>
        /// Restores the selected task. Creates a toast notifying
        /// of the change. Refreshed the Todo item list.
        /// </summary>
        /// <param name="todoItem">The todo item to remove from the trash</param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RestoreSelectedItem(TodoItem todoItem)
        {
            //Create and show toast
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Restored \"" + todoItem.Title + "\"";
            var toast = Toast.Make(text, ToastDuration.Long, 14);
            await toast.Show(cancellationTokenSource.Token);

            for (int i = 0; i < TodoItems.Count; i++)
                if (TodoItems.ElementAt(i).Id == todoItem.Id)
                {
                    TodoItems.ElementAt(i).InTrash = false;
                }

            _tm.InsertAll(TodoItems.ToList());
            ((DeleteModel)_dm).Delete(todoItem);
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
                {
                    TodoItems.ElementAt(i).Archived = true;
                    TodoItems.ElementAt(i).InTrash = false;
                }

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

        /// <summary>
        /// Updates the time that is displayed for when the task will be automatically
        /// deleted.
        /// </summary>
        /// <param name="item"></param>
        public void UpdateDeleteTime(TodoItem item)
        {
            foreach(var data in _dm.GetData())
            {
                if(data.todo == item.Id)
                {
                    DeleteTime = "Automatically Deletes in " + (data.deleteTime.Date + new TimeSpan(30,0,0,0) - DateTime.Now.Date).TotalDays + " Days";
                    OnPropertyChanged(nameof(DeleteTime));
                    return;
                }
            }
        }
    }
}
