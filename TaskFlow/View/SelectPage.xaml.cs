using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Picker;
using System.Diagnostics;
using TaskFlow.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class SelectPage : ContentPage
{
    Button button;
    DateTime scheduledTime;

    public SelectPage(SchedulerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        this.Picker.SelectionChanged += this.OnDatePickerSelectionChanged;
        this.Picker.OkButtonClicked += this.OnDateTimePickerOkButtonClicked;
    }

    /// <summary>
    /// Loads todo items from view model whenever page is about to appear on screen.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((SchedulerViewModel)BindingContext).LoadTodoItems();
    }

    /// <summary>
    /// Opens DateTime picker if user selects a task to schedule, and identify which task the event belongs to.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Clicked(object sender, EventArgs e)
    {
        button = sender as Button;
        this.Picker.IsOpen = true;
    }

    /// <summary>
    /// Retrieves DateTime value selected by the user from the DateTime picker popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDatePickerSelectionChanged(object sender, DatePickerSelectionChangedEventArgs e)
    {
        scheduledTime = e.NewValue;  // Booking start DateTime selected the user
    }

    /// <summary>
    /// Checks if potential booking overlaps with any existing bookings, if it does then send an alert, 
    /// if it does not then create booking and add it to schedule.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnDateTimePickerOkButtonClicked(object sender, EventArgs e)
    {
        if (button != null)
        {
            var todoItem = button.BindingContext as TodoItem;
            bool appointmentOverlap = ((SchedulerViewModel)BindingContext).IsBooked(scheduledTime, todoItem.TimeBlock); // Check if new booking overlaps with existing bookings

            if (appointmentOverlap)
            {
                await DisplayAlert("Oh no!", "You already have something scheduled then.", "OK"); // Running even if appoinmentOverlap is false!!
            }
            else
            {
                ((SchedulerViewModel)BindingContext).AddTodo(todoItem, scheduledTime); // Add new booking to obervable collection of scheduler bookings
                button = null;

                await Shell.Current.GoToAsync("//SchedulePage"); // Exit booking select page and return to schedule
            }
        }

    }

    /// <summary>
    /// Closes DateTime Picker if the user presses cancel on the popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDateTimePickerCancelButtonClicked(object sender, EventArgs e)
    {
        this.Picker.IsOpen = false;
    }
}