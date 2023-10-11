using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((ProfileViewModel)BindingContext).LoadUserProfile();
    }

    /// <summary>
    /// Handlers for navigating to each page using the non-shell tab bar.
    /// </summary>
    private async void OnToDoTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ToDoPage");
    }

    private async void OnSchedulerTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnCalendarTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CalendarPage");
    }

    private async void OnTimerTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnNotesTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}