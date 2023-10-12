

using CommunityToolkit.Maui.Views;
using TaskFlow.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class Pomodoro : ContentPage
{
    public Pomodoro(PomodoroViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        //this.BindingContext = new PomodoroPopupPage();
    }

    //private int start = 80;
    bool isCircularTimerOn = false;
    private void play_pause_Clicked(object sender, EventArgs e)
    {
        isCircularTimerOn = !isCircularTimerOn;
        if (isCircularTimerOn)
        {
            play.IsVisible = false;
            pause.IsVisible = true;
        }

        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!isCircularTimerOn)
            {
                play.IsVisible = true;
                pause.IsVisible = false;
                return false;
            }

            Dispatcher.DispatchAsync(() =>
            {
                pointer.Value -= 1;
                if (pointer.Value == -1)
                {
                    isCircularTimerOn = false;
                    pointer.Value = 60;
                    timer.Text = "01:00";
                }
                else
                {
                    timer.Text = pointer.Value.ToString("00:00");
                }
            });

            return true;
        });

    }


    //private TimeOnly time = new();
    //private bool isRunning;

    //public Pomodoro()
    //{
    //    InitializeComponent();
    //}

    //private async void OnStart(object sender, EventArgs e)
    //{
    //    isRunning = !isRunning;
    //    startStopButton.Source = isRunning ? "pause" : "play";

    //    while (isRunning)
    //    {
    //        time = time.Add(TimeSpan.FromSeconds(1));
    //        SetTime();
    //        await Task.Delay(TimeSpan.FromSeconds(1));
    //    }
    //}

    //private void OnReset(object sender, EventArgs e)
    //{
    //    time = new TimeOnly();
    //    SetTime();
    //}

    //private void SetTime()
    //{
    //    timeLabel.Text = $"{time.Minute}:{time.Second:000}";
    //}

}