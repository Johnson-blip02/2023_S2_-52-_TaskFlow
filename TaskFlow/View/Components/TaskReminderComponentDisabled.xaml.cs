using System.Collections.ObjectModel;
using TaskFlow.Model;

namespace TaskFlow.View.Components;

/// <summary>
/// Code behind for TaskReminderComponentDisabled.xaml. This component is the disabled version of the TaskReminderComponent.
/// </summary>
public partial class TaskReminderComponentDisabled : ContentView
{
    public static readonly BindableProperty ReminderEnabledProperty = BindableProperty.Create(nameof(ReminderEnabled), typeof(bool), typeof(TaskReminderComponentDisabled), false, BindingMode.TwoWay);
    public static readonly BindableProperty NotifyTimeProperty = BindableProperty.Create(nameof(NotifyTime), typeof(TimeSpan), typeof(TaskReminderComponentDisabled), new TimeSpan(0, 30, 0), BindingMode.TwoWay);

    public TimeSpan NotifyTime
    {
        get => (TimeSpan)GetValue(NotifyTimeProperty);
        set => SetValue(NotifyTimeProperty, value);
    }

    /// <summary>
    /// Reminder enabling/disabling
    /// </summary>
    public bool ReminderEnabled
    {
        get => (bool)GetValue(ReminderEnabledProperty);
        set
        {
            toggleSwitch.IsToggled = value;
            RowDropdown.IsVisible = value;

            OnPropertyChanged(nameof(ReminderEnabled));
            OnPropertyChanged(nameof(RowDropdown));

            SetValue(ReminderEnabledProperty, value);
        }
    }

    /// <summary>
    /// The list of selectable time blocks
    /// </summary>
	public ObservableCollection<TimeSpan> TimeNotifyList
    {
        get => new ObservableCollection<TimeSpan>(TodoItem.TimeBlockGenerator());
    }

    public TaskReminderComponentDisabled()
	{
		InitializeComponent();
	}
}