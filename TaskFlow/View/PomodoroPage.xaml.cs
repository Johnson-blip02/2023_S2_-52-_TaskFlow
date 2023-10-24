using TaskFlow.ViewModel;

namespace TaskFlow.View;

/// <summary>
/// Page for PomordoPage
/// Backend of the PomodoroPage
/// </summary>

public partial class PomodoroPage : ContentPage
{
    public PomodoroPage(PomodoroViewModel vm)
    {

        InitializeComponent();
        BindingContext = vm;
    }


    public bool isCircularTimerOn = false;
    private void play_pause_Clicked(object sender, EventArgs e)
    {
        /// <summary>
        /// button for puase
        /// </summary>
        /// <param name="sender">Pause that triggered an event</param>
        /// <param name="Pause">Stops timer and changes button image to pause</param>
        isCircularTimerOn = !isCircularTimerOn;
        if (isCircularTimerOn)
        {
            ((PomodoroViewModel)BindingContext).IsPlayed = true;
        }
        /// <summary>
        /// button for play
        /// </summary>
        /// <param name="sender">Play that triggered an event</param>
        /// <param name="Pause">Plays timer and changes button image to play
        /// Goes down by 1 second</param>
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!isCircularTimerOn)
            {
                ((PomodoroViewModel)BindingContext).IsPlayed = false;
                return false;
            }
            /// <summary>
            /// Calls the Start Method in ViewModel
            /// Switches between Work and Break
            /// </summary>
            Dispatcher.DispatchAsync(() =>
            {
                isCircularTimerOn = ((PomodoroViewModel)BindingContext).Start();
            });

            return true;
        });

    }

}