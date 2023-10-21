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


    private bool _IsWorking;
    public bool IsWorking
    {
        get
        {
            return _IsWorking;
        }
        set
        {
            IsWorking = true;
        }

    }

    partial void OnStarterChanged(int value)
    {
        PointerValue = Starter;
    }

    public PomodoroViewModel()
    {
        Starter = 60;
        WorkStart = 0;
        BreakStart = 0; 
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


    //private bool _isCircularTimerOn = false;
    //public bool isCircularTimerOn
    //{
    //    get
    //    {
    //        return _isCircularTimerOn;
    //    }
    //    set
    //    {
    //        isCircularTimerOn = _isCircularTimerOn;
    //    }
    //}

    //[RelayCommand]
    //public void play_pause_Clicked()
    //{
    //    isCircularTimerOn = !isCircularTimerOn;
    //    if (isCircularTimerOn)
    //    {
    //        IsPlayed = true;
    //    }

    //    Dispatcher.Start(TimeSpan.FromSeconds(1), () =>
    //    {
    //        if (!isCircularTimerOn)
    //        {
    //            IsPlayed = false;
    //            return false;
    //        }

    //        Dispatcher.DispatchAsync(() =>
    //        {
    //            isCircularTimerOn = Start();
    //        });

    //        return true;
    //    });

    //}

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