using System.Collections.ObjectModel;
using TaskFlow.Model;

namespace TaskFlow.View.Components;

/// <summary>
/// This is the code behind for TaskReminderComponent.xaml. This component is the enabled version of the TaskReminderComponent
/// which allows editing of the reminder settings.
/// </summary>
public partial class TaskReminderComponent : ContentView
{
	public static readonly BindableProperty ReminderEnabledProperty = BindableProperty.Create(nameof(ReminderEnabled), typeof(bool), typeof(TaskReminderComponent), false, BindingMode.TwoWay);
	public static readonly BindableProperty NotifyTimeProperty = BindableProperty.Create(nameof(NotifyTime), typeof(TimeSpan), typeof(TaskReminderComponent), new TimeSpan(0,30,0), BindingMode.TwoWay);	

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

    public TaskReminderComponent()
	{
		InitializeComponent();
	}
}