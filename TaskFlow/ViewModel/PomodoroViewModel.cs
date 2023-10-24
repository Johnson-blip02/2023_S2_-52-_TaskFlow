using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.View;
using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.ViewModel;
/// <summary>
/// ViewModel for PomordoPage and PomordoSetUpPage
/// </summary>
public partial class PomodoroViewModel : ObservableObject
{
    [ObservableProperty]
    int starter;
    [ObservableProperty]
    int workStart;
    [ObservableProperty]
    int breakStart;
    [ObservableProperty]
    int whileStart;
    [ObservableProperty]
    int pointerValue;
    [ObservableProperty]
    bool isPlayed;

    /// <summary>
    /// Makes the pointer equal to starter so both are constant with each other
    /// </summary>
    partial void OnStarterChanged(int value)
    {
        PointerValue = Starter;
    }

    public PomodoroViewModel()
    {
        //In case the user doesn't set a time
        //Set Starter to 
        Starter = 60;
        //Set Work to 60
        WorkStart = 60;
        //Set Break to 30
        BreakStart = 30; 
        //Set Cycle to 2
        WhileStart = 2;
        PointerValue = 60;
        IsPlayed = false;
    }
    /// <summary>
    /// Switch between Break and StartTime if true or false
    /// </summary>
    public bool IsWorking = true;
    public PomodoroViewModel(bool isWorking)
    {
        IsWorking = isWorking;
    }
    /// <summary>
    /// If User clicks Setting Button in PomodoroPage
    /// </summary>
    /// <returns>Opens the PomodoroSetUpPage</returns>
    [RelayCommand]
    public async Task DisplayPomodoroSetUp()
    {
        await Shell.Current.GoToAsync(nameof(PomodoroSetupPage));
    }

    /// <summary>
    /// Sets the timer to Work time from User values
    /// </summary>
    [RelayCommand]
    public void SetTime()
    {
        Starter = WorkStart;
    }

    /// <summary>
    /// If Pointer hits 0 then start the Cycle Check using num
    /// Switch between Break Time and Work Time until cycle ends
    /// Once Cycle ends then Timer stops 
    /// </summary>
    int num = 0;
    public bool Start()
    {
        PointerValue -= 1;
        if (PointerValue == -1)
        {
            if (num < WhileStart)
            {
                if(IsWorking)
                {
                    Starter = BreakStart;
                    PointerValue = BreakStart;
                    num++;
                    IsWorking = false;

                }
                else if (!IsWorking)
                {
                    Starter = WorkStart;
                    PointerValue = WorkStart;
                    IsWorking = true;
                }
            }
            else
            {
                num = 0;
                PointerValue = 0;
                return false;
            }
        }
        return true;
    }
}