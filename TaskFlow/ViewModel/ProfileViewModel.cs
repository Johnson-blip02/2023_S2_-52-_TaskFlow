using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.View;

namespace TaskFlow.ViewModel;

/// <summary>
/// View model for logic of the <see cref="ProfilePage"/>.
/// </summary>
public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private int score;

    [ObservableProperty]
    private int completedItemsCount;

    public ProfileViewModel()
    {
        Score = 0;
        CompletedItemsCount = 0;
    }
}
