using Android.App;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Practice.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Java.Util.Jar.Attributes;
using TaskFlow.Model;
using System.ComponentModel;

namespace TaskFlow.ViewModel
{
    public partial class NewTodoViewModel : ObservableObject, INotifyPropertyChanged
    {
        private TodoModel _tm; //Todo model
        private LabelModel _lm; //List model

        [ObservableProperty]
        ObservableCollection<TodoItem> todoItems;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        Color todoColor;

        [ObservableProperty]
        ObservableCollection<LabelItem> labelItems;

        [ObservableProperty]
        ObservableCollection<LabelItem> selectedLabel;

        [ObservableProperty]
        string importance;

        [ObservableProperty]
        ObservableCollection<TimeSpan> timeBlockList;

        [ObservableProperty]
        TimeSpan selectedBlock;

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value.Date.AddMinutes(selectedTime.TotalMinutes);
                DateTime oldDt = _selectedDate;
                OnPropertyChanged();
            }
        }

        private TimeSpan _selectedTime;
        public TimeSpan selectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                SelectedDate = SelectedDate.Date;
                SelectedDate = SelectedDate.AddMinutes(value.TotalMinutes);
            }
        }

        private string _colorLabel;
        public string ColourLabel
        {
            get => _colorLabel;
            set
            {
                TodoColor = DefinedColorsExtension.Parse(value);
                _colorLabel = value;
            }
        }

        public DateTime MinDate => DateTime.Now;

        public NewTodoViewModel()
        {
            Importance = "1";

            _tm = App.TodoModel;
            _lm = new LabelModel();

            if (_tm == null)
                throw new Exception();

            labelItems = new ObservableCollection<LabelItem>();
            //Update label items
            _UpdateList();

            SelectedDate = DateTime.Now.Date;

            //Initialize the selectable time blocks, Time blocks in increments of 15 mins
            this.TimeBlockList = new ObservableCollection<TimeSpan>(TodoItem.TimeBlockGenerator());

            SelectedBlock = new TimeSpan(0, 0, 0);
        }

        /// <summary>
        /// Adds the current todo item to the database
        /// </summary>
        [RelayCommand]
        public void AddTodo()
        {
            if (String.IsNullOrWhiteSpace(Name))
                return;

            //Create a new Todo item from the currently set data
            TodoItem item = new TodoItem(Name)
            {
                Description = this.Description,
                DueDate = this.SelectedDate,
                Color = TodoColor.ToArgbHex(),
                Importance = int.Parse(this.Importance),
                TimeBlock = this.SelectedBlock
            };

            if (SelectedLabel != null)
                item.Labels = SelectedLabel.ToList();

            _tm.Insert(item);

            App.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Updates the labels in the local collection from the database
        /// </summary>
        private void _UpdateList()
        {
            List<LabelItem> labelItems = _lm.GetData();

            if (labelItems == null)
                return;

            LabelItems = new ObservableCollection<LabelItem>();

            foreach (LabelItem li in labelItems)
                LabelItems.Add(li);
        }
    }
}
