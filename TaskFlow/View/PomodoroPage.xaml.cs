using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class PomodoroPage : ContentPage
{
    public PomodoroPage(PomodoroViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }


    //private int start = 80;
    bool isCircularTimerOn = false;
    private void play_pause_Clicked(object sender, EventArgs e)
    {
        isCircularTimerOn = !isCircularTimerOn;
        if (isCircularTimerOn)
        {
            ((PomodoroViewModel)BindingContext).IsPlayed = true;
        }

        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!isCircularTimerOn)
            {
                ((PomodoroViewModel)BindingContext).IsPlayed = false;
                return false;
            }

            Dispatcher.DispatchAsync(() =>
            {
                isCircularTimerOn = ((PomodoroViewModel)BindingContext).Start();
            });

            return true;
        });

    }

}