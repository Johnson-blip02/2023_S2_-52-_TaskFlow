using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Picker;
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


    private void Button_Clicked(object sender, EventArgs e)
    {
        button = sender as Button;

        this.Picker.IsOpen = true;
    }

    private void OnDatePickerSelectionChanged(object sender, DatePickerSelectionChangedEventArgs e)
    {
        scheduledTime = e.NewValue;
    }

    private async void OnDateTimePickerOkButtonClicked(object sender, EventArgs e)
    {
        if (button != null)
        {
            var todoItem = button.BindingContext as TodoItem;
            ((SchedulerViewModel)BindingContext).AddTodo(todoItem, scheduledTime);
            button = null;
        }

        await Shell.Current.GoToAsync("//SchedulePage");
    }

    private void OnDateTimePickerCancelButtonClicked(object sender, EventArgs e)
    {
        this.Picker.IsOpen = false;
    }
}