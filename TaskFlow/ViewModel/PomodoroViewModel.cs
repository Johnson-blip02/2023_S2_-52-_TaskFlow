using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.View;
using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.ViewModel;

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


    public bool IsWorking = true;

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

    public PomodoroViewModel(bool isWorking)
    {
        IsWorking = isWorking;
    }

    [RelayCommand]
    public async Task DisplayPomodoroSetUp()
    {
        await Shell.Current.GoToAsync(nameof(PomodoroSetupPage));
    }

    [RelayCommand]
    public void SetTime()
    {
        Starter = WorkStart;
    }

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