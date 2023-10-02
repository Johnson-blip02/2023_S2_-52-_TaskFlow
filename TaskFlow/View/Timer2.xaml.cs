namespace TaskFlow.View;

public partial class Timer2 : ContentPage
{
	private TimeOnly time = new();
	private bool isRunning;

	public Timer2()
	{
		InitializeComponent();
	}

	private async void OnStart(object sender, EventArgs e)
	{
		isRunning = !isRunning;
		startStopButton.Source = isRunning ? "pause" : "play";

		while (isRunning) { 
		time = time.Add(TimeSpan.FromSeconds(1));
			SetTime();
			await Task.Delay(TimeSpan.FromSeconds(1));	
		}
	}

	private void OnReset(object sender, EventArgs e)
	{
		time = new TimeOnly();
		SetTime();
	}

	private void SetTime()
	{
		timeLabel.Text = $"{time.Minute}:{time.Second:000}";
	}

}