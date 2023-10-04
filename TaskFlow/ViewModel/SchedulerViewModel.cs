using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using TaskFlow.Model;
using CommunityToolkit.Mvvm.Input;
using TaskFlow.View;


namespace TaskFlow.ViewModel
{
    public partial class SchedulerViewModel : ObservableObject
    {
        private readonly TodoModel _tm; // TodoModel

        [ObservableProperty]
        private ObservableCollection<TodoItem> todoItems;

        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> events;

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

        private Color ConvertColorStringToColor(string colorString)
        {
            if (Color.TryParse(colorString, out Color color))
            {
                return color;
            }
            return Color.FromArgb("#FF8B1FA9");
        }

        #region Method
        public void GenerateAppointments()
        {
            Events.Clear();
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
