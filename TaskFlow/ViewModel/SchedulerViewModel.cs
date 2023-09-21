using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using TaskFlow.Model;

namespace TaskFlow.ViewModel
{
    public partial class SchedulerViewModel : ObservableObject
    {
        private readonly TodoModel _tm; // TodoModel

        [ObservableProperty]
        private ObservableCollection<TodoItem> todoItems;

        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> events;

        #region Constructor
        public SchedulerViewModel()
        {
            _tm = App.TodoModel;
            TodoItems = new ObservableCollection<TodoItem>();
            Events = new ObservableCollection<SchedulerAppointment>();
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
                var appointment = new SchedulerAppointment
                {
                    StartTime = todoItem.DueDate - todoItem.TimeBlock,
                    EndTime = todoItem.DueDate,
                    Subject = todoItem.Title,
                    Background = new SolidColorBrush(ConvertColorStringToColor(todoItem.Color)),
                };

                this.Events.Add(appointment);
            }
        }
        #endregion
    }
}
