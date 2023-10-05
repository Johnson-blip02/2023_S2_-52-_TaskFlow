using System.Collections.ObjectModel;
using System.Diagnostics;
using Syncfusion.Maui.Scheduler;
using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.Model;
using CommunityToolkit.Mvvm.Input;
using TaskFlow.View;


namespace TaskFlow.ViewModel
{
    /// <summary>
    /// ViewModel for logic of the Scheduler View and Calendar View.
    /// </summary>
    
    public partial class SchedulerViewModel : ObservableObject
    {
        private readonly TodoModel _tm; // TodoModel

        [ObservableProperty]
        private ObservableCollection<TodoItem> todoItems; // Collection of todo items from database.

        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> events; // Collection of calendar appointments

        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> scheduleEvents;


        #region Constructor
        public SchedulerViewModel()
        {
            _tm = App.TodoModel;
            TodoItems = new ObservableCollection<TodoItem>();
            Events = new ObservableCollection<SchedulerAppointment>();
            ScheduleEvents = new ObservableCollection<SchedulerAppointment>();
            this.LoadTodoItems();
            this.GenerateAppointments();
        }
        #endregion

        /// <summary>
        /// Loads todo items from the database and updates the <see cref="TodoItems"/> collection
        /// </summary>
        #region Method
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
        #endregion

        /// <summary>
        /// Takes a string and converts it into a Maui.Graphics.Color value.
        /// </summary>
        #region Method
        private Color ConvertColorStringToColor(string colorString)
        {
            if (Color.TryParse(colorString, out Color color))
            {
                return color;
            }
            return Color.FromArgb("#FF8B1FA9");
        }
        #endregion

        /// <summary>
        /// Iterates through the <see cref="TodoItems"/> collection and creates a SchedulerAppointment
        /// for each task that is compatible with the syncfusion scheduler for the CalendarPage View.
        /// </summary>
        #region Method
        public void GenerateAppointments()
        {
            Events.Clear(); // Clear events collection so calendar events are not duplicated everytime the scheduler view model is loaded.
            foreach (var todoItem in TodoItems)
            {
                //if (todoItem.TimeBlock.Equals(0))
                //{
                //    todoItem.TimeBlock = new TimeSpan(0,);
                //}
                var appointment = new SchedulerAppointment
                {
                    StartTime = todoItem.DueDate,
                    EndTime = todoItem.DueDate + todoItem.TimeBlock,
                    Subject = todoItem.Title,
                    Background = new SolidColorBrush(ConvertColorStringToColor(todoItem.Color)),
                };
                //if(appointment.StartTime ==  DateTime.MinValue)
                this.Events.Add(appointment);
            }
        }
        #endregion


        public void AddTodo(TodoItem todoItem)
        {
            
            var timeBlock = new SchedulerAppointment
            {
                StartTime = todoItem.DueDate,
                EndTime = todoItem.DueDate + todoItem.TimeBlock,
                Subject = todoItem.Title,
                Background = new SolidColorBrush(ConvertColorStringToColor(todoItem.Color)),
            };

            this.ScheduleEvents.Add(timeBlock);
            OnPropertyChanged(nameof(ScheduleEvents));
        }

        [RelayCommand]
        public async Task GoToSelectTask()
        {
            await Shell.Current.GoToAsync(nameof(SelectPage));
        }
    }
}
