using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.Model;

namespace TaskFlow
{
public partial class TimeBlock : ObservableObject
    {
        private readonly TodoModel _tm; // TodoModel

        [ObservableProperty]
        string name;

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

    }
}