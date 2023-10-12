using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TaskFlow.Model;
using TaskFlow.View;
using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.ViewModel;

public partial class PomodoroViewModel : ObservableObject
{
    [ObservableProperty]
    int starter;
    [ObservableProperty]
    int breaker;
    [ObservableProperty]
    int whiler;
    [ObservableProperty]
    int pointerValue;

    public PomodoroViewModel()
    {
        Starter = 60; 
        Breaker = 0; 
        Whiler = 0;
        PointerValue = 60;
    }

    [RelayCommand]
    public async Task DisplayPomodoroSetUp()
    {
        await Shell.Current.GoToAsync(nameof(PomodoroSetupPage));
    }

    //[RelayCommand]
    //void Add()
    //{
    //    Starter = string.Empty;
    //    Breaker = string.Empty;
    //    Whiler = string.Empty;
    //}

    //public string starter;
    //public string breaker;
    //public string whiler;

    //var TimeValues 

    //public ObservableCollection<TimeValues> time { get; set; } = new ObservableCollection<TimeValues>();

    //public PomodoroViewModel()
}