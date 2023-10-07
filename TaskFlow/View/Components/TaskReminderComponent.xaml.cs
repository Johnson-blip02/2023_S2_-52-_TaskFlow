using TaskFlow.Model;

namespace TaskFlow.View.Components;

public partial class TaskReminderComponent : ContentView
{
	public static readonly BindableProperty ReminderEnabledProperty = BindableProperty.Create(nameof(ReminderEnabled), typeof(bool), typeof(bool), false);
	public static readonly BindableProperty TodoItemProperty = BindableProperty.Create(nameof(TodoItem), typeof(TodoItem), typeof(TodoItem), null);	

	public TodoItem TodoItem
	{
        get => (TodoItem)GetValue(TodoItemProperty);
        set => SetValue(TodoItemProperty, value);
    }

	public bool ReminderEnabled
	{
        get => (bool)GetValue(ReminderEnabledProperty);
        set => SetValue(ReminderEnabledProperty, value);
    }


	public TaskReminderComponent()
	{
		InitializeComponent();
	}
}