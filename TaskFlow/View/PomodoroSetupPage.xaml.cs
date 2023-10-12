using CommunityToolkit.Maui.Views;
using TaskFlow.Model;

namespace TaskFlow.View;

public partial class PomodoroSetupPage : ContentPage
{
	TimeValues timeValues;

	public PomodoroSetupPage()
	{
		InitializeComponent();
	}

	private async void SubmitTime(object sender, EventArgs e)
    {

    }

    //private async void OnStartChange(object sender, EventArgs e)
    //{
    //	string startValue = ValueStart.Text;
    //	string breakValue = ValueBreak.Text;
    //	string whileValue = ValueLoop.Text;
    //	if (startValue == "" || breakValue == "" || whileValue == "")
    //	{
    //		await DisplayAlert("Problem", "Time set up is incomplete", "Ok");
    //	}
    //	else
    //	{
    //		TimeValues timeValues = new TimeValues(startValue, breakValue, whileValue);
    //		await Navigation.PushAsync(new Pomodoro(timeValues));
    //	}

    //   }

}