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
        private readonly DayModel _dm; // DayModel

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
            _dm = App.DayModel;
            TodoItems = new ObservableCollection<TodoItem>();
            Events = new ObservableCollection<SchedulerAppointment>();
            ScheduleEvents = new ObservableCollection<SchedulerAppointment>();
            this.LoadTodoItems();
            this.GenerateAppointments();
            ScheduleRefresh();
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

        /// <summary>
        /// Takes todoItem passed from scheduler select page and create a new SchedulerAppointment and add to ScheduleEvents
        /// obvervable collection. This collection is the list of scheduled events with the dates and times they are scheduled to
        /// display on the scheduler.
        /// </summary>
        public void AddTodo(TodoItem todoItem, DateTime? scheduledTime)
        {
            if (scheduledTime is null)
                scheduledTime = todoItem.DueDate - todoItem.TimeBlock; //Todo: make have the right value

            Day day = null;
            List<Day> AllDays = _dm.GetData();

            foreach (var getDay in AllDays) //Search for the day related to the scheduled task
            {
                if(getDay.Date == ((DateTime)scheduledTime).Date)
                    day = getDay;
            }

            if(day is null) //Initalizes day if there is none in the db
            {
                day = new Day();
                day.InitalizeDay();
            }

            todoItem.ScheduledTime = (DateTime)scheduledTime;

            /* Updates the todo item if it is already added to the current day
               (only allows the same todo item to be scheduled once per day) */
            int index = day.TodoItem.FindIndex(x => x.Id == todoItem.Id);
            if(index != -1)
                day.TodoItem[index] = todoItem;
            else
                day.TodoItem.Add(todoItem);

            _tm.Insert(todoItem);
            _dm.Insert(day);

            ScheduleRefresh();
        }


        public void ScheduleRefresh()
        {
            ScheduleEvents.Clear();

            List<Day> AllDays = _dm.GetData();

            foreach (var day in AllDays)
            {
                foreach (TodoItem todoItem in day.TodoItem)
                {
                    var timeBlock = new SchedulerAppointment
                    {
                        StartTime = todoItem.ScheduledTime,
                        EndTime = todoItem.ScheduledTime + todoItem.TimeBlock,
                        Subject = todoItem.Title,
                        Background = new SolidColorBrush(ConvertColorStringToColor(todoItem.Color)),
                    };

                    this.ScheduleEvents.Add(timeBlock);
                }
            }
            OnPropertyChanged(nameof(ScheduleEvents));
        }

        /// <summary>
        /// Method to check if the time selected by the DatePicker in SelectPage already has an existing booking. 
        /// Return true if it does.
        /// </summary>
        public Boolean IsBooked(DateTime potentialStartTime, TimeSpan timeBlock)
        {
            var potentialEndTime = potentialStartTime + timeBlock;

            foreach (var appointment in ScheduleEvents)
            {
                if((potentialStartTime >= appointment.StartTime && potentialStartTime < appointment.EndTime) || // For case when new booking start time is within the time span of another booking
                    (potentialStartTime < appointment.StartTime && potentialEndTime <= appointment.EndTime)) // For case when new booking starts before and ends after original booking
                {
                    return true; // potential booking does overlap with existing bookings
                }
            }
            return false; // potential booking does not overlap with existing bookings
        }

        /// <summary>
        /// Opens popup to select a task to add to the scheduler
        /// </summary>
        [RelayCommand]
        public async Task GoToSelectTask()
        {
            await Shell.Current.GoToAsync(nameof(SelectPage));
        }
    }
}
