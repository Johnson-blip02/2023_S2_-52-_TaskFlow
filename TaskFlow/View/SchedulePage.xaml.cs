using Syncfusion.Maui.Popup;
using Syncfusion.Maui.Scheduler;
using TaskFlow.View;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class SchedulePage : ContentPage
{
    private ImageButton _lastDropped;

    public SchedulePage(SchedulerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    /// <summary>
    /// Load all items in the todo list and generate appointments to display in scheduler component.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((SchedulerViewModel)BindingContext).LoadTodoItems();
        ((SchedulerViewModel)BindingContext).GenerateAppointments();
    }

    /// <summary>
    /// If an appointment on the scheduler is tapped, display a pop up for appointment unschedule confirmation.
    /// If user presses cancel, close pop up. If user presses remove, call ExexcuteUnschedule method.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
    {
        SfPopup confirmPopup = new SfPopup() // Create pop up to confirm if user wants to unschedule the task
        {
            HeaderTitle = "Remove Task",
            Message = "Do you want to unschedule this task?",
            AutoSizeMode = PopupAutoSizeMode.Height,
            AppearanceMode = PopupButtonAppearanceMode.TwoButton,
            AcceptButtonText = "Remove",
            DeclineButtonText = "Cancel",
            AcceptCommand = new Command(() => ExecuteUnschedule(e)), // Method to unschedule, pass tapped appointment details as argument
            ShowFooter = true,
            HeightRequest = 180,
            PopupStyle = new PopupStyle()
            {
                PopupBackground = Color.Parse("#341C4F"),
                HeaderTextColor = Colors.White,
                MessageTextColor = Colors.White,
                AcceptButtonTextColor = Colors.White,
                DeclineButtonTextColor = Colors.White
            }
        };

        confirmPopup.Show();
    }

    /// <summary>
    /// Take appiontment details for tapped event and pass it through as a parameter to scheduler view model RemoveTaskFromSchedule method.
    /// This should remove the task from the scheduler and refresh the view.
    /// </summary>
    /// <param name="e"></param>
    public void ExecuteUnschedule(SchedulerTappedEventArgs e)
    {

        // If user tapped on an appointment, check if it was null and then send title and start time to RemoveTaskFromSchedule as identifiers
        if (e.Element == SchedulerElement.SchedulerCell || e.Element == SchedulerElement.Appointment)
        {
            if (e.Appointments != null)
            {
                String title = (e.Appointments[0] as SchedulerAppointment).Subject;
                DateTime startTime = (e.Appointments[0] as SchedulerAppointment).StartTime;

                ((SchedulerViewModel)BindingContext).RemoveTaskFromSchedule(title, startTime);
            }
        }
    }
}

