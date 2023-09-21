using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Practice.Model;
using System.Collections.ObjectModel;
using TaskFlow.Model;
using System.ComponentModel;
using TaskFlow.View;
using CommunityToolkit.Maui.Views;

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
        ObservableCollection<object> selectedLabels;

        [ObservableProperty]
        string importance;

        [ObservableProperty]
        string newLabelTitle;

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
            _lm = App.LabelModel;

            if (_tm == null)
                throw new Exception();

            labelItems = new ObservableCollection<LabelItem>();

            SelectedDate = DateTime.Now.Date;

            //Initialize the selectable time blocks, Time blocks in increments of 15 mins
            this.TimeBlockList = new ObservableCollection<TimeSpan>(TodoItem.TimeBlockGenerator());

            SelectedLabels = new ObservableCollection<object>();

            SelectedBlock = new TimeSpan(0, 0, 0);

            NewLabelTitle = string.Empty;

        }

        /// <summary>
        /// Updates label list each time <see cref="NewTodoPage"/> appears on screen
        /// </summary>
        public void OnAppearing()
        {
            _UpdateList();
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

            if (SelectedLabels != null)
            {
                foreach(var label in  SelectedLabels)
                    item.Labels.Add((LabelItem)label);
            }

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

        /// <summary>
        /// Adds a new <see cref="LabelItem"/> to the database with title and updates list
        /// </summary>
        [RelayCommand]
        public void AddNewLabel()
        {
            if(!string.IsNullOrWhiteSpace(NewLabelTitle))
            {
                _lm.Insert(new LabelItem(NewLabelTitle));
            }

            NewLabelTitle = string.Empty;
            _UpdateList();
        }

        /// <summary>
        /// Navigates to the <see cref="LabelPage"/> view.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task GoToLabelPage()
        {
            await Shell.Current.GoToAsync(nameof(LabelPage));
        }
    }
}
