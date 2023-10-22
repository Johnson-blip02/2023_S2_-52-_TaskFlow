using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using TaskFlow.Messages;
using TaskFlow.View;

namespace TaskFlow.ViewModel;

/// <summary>
/// View model for logic of the <see cref="ProfilePage"/>.
/// </summary>
public partial class ProfileViewModel : ObservableObject, IRecipient<ProfileUpdatedMessage>
{
    [ObservableProperty]
    private int score;

    [ObservableProperty]
    private int completedItemsCount;

    public ProfileViewModel()
    {
        Score = 0;
        CompletedItemsCount = 0;

        // Register as subscriber to message.
        WeakReferenceMessenger.Default.Register<ProfileUpdatedMessage>(this);
    }

    /// <summary>
    /// Receives new score and completed items count from todo view model.
    /// </summary>
    /// <param name="message"></param>
    public void Receive(ProfileUpdatedMessage message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Score = message.Value.UserScore;
            CompletedItemsCount = message.Value.UserCompletedCount;
        });
    }

}
