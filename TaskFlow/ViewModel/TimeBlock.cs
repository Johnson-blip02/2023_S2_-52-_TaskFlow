using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.Model;

namespace TaskFlow
{
public partial class TimeBlock : ObservableObject
    {
        [ObservableProperty]
        string name;

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value.Date.AddMinutes(SelectedTime.TotalMinutes);
                OnPropertyChanged();
            }
        }

        private TimeSpan _selectedTime;
        public TimeSpan SelectedTime
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