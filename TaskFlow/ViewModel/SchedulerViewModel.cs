
using System.Collections.ObjectModel;
using System.Diagnostics;
using Syncfusion.Maui.Scheduler;
using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.Model;
using CommunityToolkit.Mvvm.Input;
using TaskFlow.View;
using Microsoft.Maui.Platform;

namespace TaskFlow.ViewModel
{
    /// <summary>
    /// ViewModel for logic of the Scheduler View and Calendar View.
    /// </summary>
    
    public partial class SchedulerViewModel : ObservableObject
    {
        private readonly IDatabase<TodoItem> _tm; // TodoModel
        private readonly IDatabase<Day> _dm; // DayModel
        private readonly IDatabase<ScheduledTime> _sm; //ScheduledTimeModel

        [ObservableProperty]
        private ObservableCollection<TodoItem> todoItems; // Collection of todo items from  database.

        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> events; // Collection of calendar appointments

        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> scheduleEvents;

        #region Constructor
        public SchedulerViewModel()
        {
            _tm = App.TodoModel;
            _dm = App.DayModel;
            _sm = App.ScheduledTimeModel;
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

                if (itemsList != null && itemsList.Count > 0) // Check the database is not empty
                {
                    TodoItems.Clear();
                    foreach (var item in itemsList)
                    {
                        if (item.InTrash || item.Archived || item.Completed) // Don't add items in trash, archive, or done list
                        {
                            continue;
                        } 
                        else
                        {
                            TodoItems.Add(item); // Add remaining items to the todo list
                        } 
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
        public Color ConvertColorStringToColor(string colorString)
        {
            if (Color.TryParse(colorString, out Color color))
            {
                return color;
            }
            return Color.FromArgb("#FF8B1FA9");
        }
        #endregion

        public static string DarkenHexColor(string hexColor, double percentage)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1); // Remove # if hex code contains it to isolate the number
            }

            // Convert hex to RGB
            int red = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int green = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int blue = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            // Darken the RGB values
            red = (int)(red * (1 - percentage));
            green = (int)(green * (1 - percentage));
            blue = (int)(blue * (1 - percentage));

            // Validate number range (can't go below 0 or over 255 for each RGB value)
            red = Math.Max(0, Math.Min(255, red));
            green = Math.Max(0, Math.Min(255, green));
            blue = Math.Max(0, Math.Min(255, blue));

            // Convert the darker RGB values back into hex
            string darkenedHexColor = string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);

            return darkenedHexColor; // Return darker hex value
        }


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
                var appointment = new SchedulerAppointment // Create calendar events of the todoItem at duedate
                {
                    StartTime = todoItem.DueDate,
                    EndTime = todoItem.DueDate + todoItem.TimeBlock,
                    Subject = todoItem.Title,
                    Background = new SolidColorBrush(ConvertColorStringToColor(todoItem.Color)),
                };

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
            // If scheduled time is null, add to just before due date
            if (scheduledTime is null)
                scheduledTime = todoItem.DueDate - todoItem.TimeBlock;

            Day day = null;
            ScheduledTime newStartTime = new ScheduledTime((DateTime)scheduledTime); // Create a new ScheduledTime item in the database 

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

            if (!day.TodoItem.Contains(todoItem)) // If day doesn't contain the todoItem already, add it
            {
                day.TodoItem.Add(todoItem);
            }

            todoItem.ScheduledTimes.Add(newStartTime); // Add new scheduled time to list in todoItem object

            // Update the database with new changes
            _sm.Insert(newStartTime);
            _tm.Insert(todoItem);
            _dm.Insert(day);

            ScheduleRefresh(); // Call refresh to remake the scheduler timeblocks
        }

        /// <summary>
        /// Method to retrieve all tasks from the database and remakes scheduler timeblocks to update the ScheduleEvents collection.
        /// </summary>
        public void ScheduleRefresh()
        {
            ScheduleEvents.Clear();

            List<TodoItem> AllTasks = _tm.GetData();

            foreach (var todoItem in AllTasks)
            { 
                foreach (var appointmentStartTime in todoItem.ScheduledTimes)
                {
                    Color originalColor = ConvertColorStringToColor(todoItem.Color); // Retrieve colour selected by user
                    Color darkenedColor = ConvertColorStringToColor(DarkenHexColor(todoItem.Color, 0.3)); // Creates a darker version of colour selected by user

                    var timeBlock = new SchedulerAppointment // Generate an appointment for scheduler with task data and datetime input from picker
                    {
                        StartTime = appointmentStartTime.StartTime,
                        EndTime = appointmentStartTime.StartTime + todoItem.TimeBlock,
                        Subject = todoItem.Title,
                        Background = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 1),
                            EndPoint = new Point(0.5, 0),
                            GradientStops = new GradientStopCollection // Create a colour gradient for the background of the timeblock
                            {
                               new GradientStop
                                {
                                   Color = darkenedColor,
                                    Offset = 0.0F
                                },
                                new GradientStop
                                {
                                    Color = originalColor,
                                   Offset = 1.0F
                               }
                            }
                        }
                    };

                    this.ScheduleEvents.Add(timeBlock); // Add the newly created timeblock to the scheduleevents collecton
                }

                OnPropertyChanged(nameof(ScheduleEvents)); // Trigger refresh when change is observed
            }
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
                if((potentialStartTime <= appointment.StartTime && potentialEndTime > appointment.StartTime) || // New appointment begins before existing appointment but ends midway through existing appointment
                   (potentialStartTime >= appointment.StartTime && potentialEndTime < appointment.EndTime) || // New appointment begins and ends midway through existing appointment.
                   (potentialStartTime < appointment.EndTime && potentialEndTime >= appointment.EndTime) || // New appointment begins midway through existing appointment but ends after an existing appointment.
                   (potentialStartTime <= appointment.StartTime && potentialEndTime >= appointment.EndTime) || // New appointment begins before existing appointment and ends after existing appointment.
                   (potentialStartTime >= appointment.StartTime && potentialStartTime < appointment.EndTime && potentialEndTime >= appointment.EndTime)) // New appointment begins after existing appointment begins and ends after existing appointment ends.
                {
                    return true; // Potential booking does overlap with existing bookings
                }
            }
            return false; // Potential booking does NOT overlap with existing bookings
        }

        /// <summary>
        /// Takes a title and startTime and removes the scheduledtime from the todoItem, day, and scheduledTime table.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startTime"></param>
        public void RemoveTaskFromSchedule(string title, DateTime startTime)
        {
            List<TodoItem> AllTasks = _tm.GetData();
            List<Day> AllDays = _dm.GetData();
            List<ScheduledTime> AllScheduledTimes = _sm.GetData();

            // Remove the scheduled time from the table of scheduled times in the database
            foreach (var scheduledTime in AllScheduledTimes)
            {
                if (scheduledTime.StartTime == startTime) // No double booking, so startTime can be used as an Id
                {
                    _sm.Delete(scheduledTime);
                    break;
                }
            }

            // Remove the day if the task being unscheduled was the only task scheduled to that day
            foreach (var day in AllDays)
            {
                if (day.Date == startTime.Date)
                {
                    if (day.TodoItem.Count <= 1)
                    {
                        _dm.Delete(day);
                        break;
                    }
                }
            }

            // Remove Scheduled Time from the list of scheduled times in the todoItem
            foreach (var todoItem in AllTasks)
            {
                if (todoItem.Title == title)
                {
                    for (int i = 0; i < todoItem.ScheduledTimes.Count; i++)
                    {
                        if (todoItem.ScheduledTimes[i].StartTime == startTime)
                        {
                            todoItem.ScheduledTimes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            ScheduleRefresh();
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

    /* 
     
                // Remove the scheduled time from the table of scheduled times in the database
            foreach (var scheduledTime in AllScheduledTimes)
            {
                //if (scheduledTime.StartTime == startTime) // No double booking, so startTime can be used as an Id
                //{
                    _sm.Delete(scheduledTime);
                   // break;
                //}
            }

            // Remove the day if the task being unscheduled was the only task scheduled to that day
            foreach (var day in AllDays)
            {
               // if(day.Date == startTime.Date)
                //{
                 //   if(day.TodoItem.Count <= 1)
                   // {
                        _dm.Delete(day);
                     //   break;
                    //}
                //}
            }

            // Remove Scheduled Time from the list of scheduled times in the todoItem
            foreach (var todoItem in AllTasks)
            {
                    // if(todoItem.Title == title)
                    //{
                    //  for (int i = 0; i < todoItem.ScheduledTimes.Count; i++)
                    //{
                    //  if (todoItem.ScheduledTimes[i].StartTime == startTime)
                    //    {
                    //todoItem.ScheduledTimes.RemoveAt(i);
                   
                    //    break;
                    //}    
                    //}

                    // }
            }

            ScheduleRefresh();

    */
}