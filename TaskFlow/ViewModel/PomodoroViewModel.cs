using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TaskFlow.Model;
using TaskFlow.View;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace TaskFlow.ViewModel;

public partial class PomodoroViewModel : ObservableObject
{
    [ObservableProperty]
    int starter;

    int workStart;
    [ObservableProperty]
    int breakStart;
    [ObservableProperty]
    int whiler;
    [ObservableProperty]
    int pointerValue;
    [ObservableProperty]
    bool isPlayed;

    partial void OnStarterChanged(int value)
    {
        PointerValue = value;
    }

    public PomodoroViewModel()
    {
        Starter = 60; 
        Breaker = 0; 
        Whiler = 0;
        PointerValue = 60;
        IsPlayed = false;
    }

    [RelayCommand]
    public async Task DisplayPomodoroSetUp()
    {
        await Shell.Current.GoToAsync(nameof(PomodoroSetupPage));
    }

    public void StartTimer()
    {
        PointerValue -= 1;
        if (PointerValue == -1)
        {
            IsPlayed = false;
            PointerValue = 60;
        }
    }

}