using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using TaskFlow.Messages;
using TaskFlow.Model;
using TaskFlow.View;

namespace TaskFlow.ViewModel;

/// <summary>
/// View model for logic of the <see cref="ProfilePage"/>. This class subscribers to the <see cref="ProfileUpdatedMessage"/> message.
/// </summary>
public partial class ProfileViewModel : ObservableObject, IRecipient<ProfileUpdatedMessage>
{
    public readonly List<Achievement> availableAchievements;  // All available achievements.

    [ObservableProperty]
    private int score;

    [ObservableProperty]
    private int completedItemsCount;

    [ObservableProperty]
    private ObservableCollection<Achievement> userAchievements;

    [ObservableProperty]
    private double progress;  // Value between 0 and 1 signifying progress towards next achievement.

    [ObservableProperty]
    private int nextAchievementScore;

    [ObservableProperty]
    private bool hasNoAchievements;

    private bool isFirstOpened;  // True on app start up; false once user interacts with todo page.

    public ProfileViewModel()
    {
        Score = 0;
        CompletedItemsCount = 0;
        isFirstOpened = true;
        Progress = 0.0;
        UserAchievements = new ObservableCollection<Achievement>();
        availableAchievements = AchievementManager.AvailableAchievements;  // Initiating with all avaibale achievements.
        NextAchievementScore = 1;
        HasNoAchievements = true;

        // Register class as subscriber to message.
        WeakReferenceMessenger.Default.Register<ProfileUpdatedMessage>(this);
    }

    /// <summary>
    /// Calls local LoadAchievements() and LoadProgress() methoods when the score is updated.
    /// </summary>
    /// <param name="value">The updated score.</param>
    partial void OnScoreChanged(int value)
    {
        LoadAchievements(value);
        LoadProgress();
    }

    /// <summary>
    /// Loads the user's acquired achievements and displays a popup by calling local DisplayPopup() method.
    /// </summary>
    /// <remarks>
    /// If the score is being updated on app startup, the popup will not be displayed.
    /// </remarks>
    /// <param name="newScore">The updated score.</param>
    public void LoadAchievements(int newScore)
    {
        List<Achievement> filteredAchievements = availableAchievements
            .Where(achievement => achievement.MeetsRequirements(newScore)).ToList();

        if (!isFirstOpened && filteredAchievements.Count > UserAchievements.Count)
            DisplayPopup(filteredAchievements.Last().Title);

        UserAchievements = new ObservableCollection<Achievement>(filteredAchievements);

        HasNoAchievements = UserAchievements.Count <= 0 || UserAchievements is null;
    }

    /// <summary>
    /// Calculates and updates the progress status.
    /// </summary>
    public void LoadProgress()
    {
        int nextAchievementScore = AchievementManager.ScoreRequirements.FirstOrDefault(score => score > Score);

        if (nextAchievementScore == 0)
        {
            Progress = 1.0;   // User has achieved all available achievements
            NextAchievementScore = 0;
        }
        else
        {
            int previousAchievementScore = AchievementManager.ScoreRequirements.LastOrDefault(score => score <= Score);

            double progressNeeded = (Score - previousAchievementScore) / (double)(nextAchievementScore - previousAchievementScore);

            Progress = Math.Min(progressNeeded, 1.0);
            NextAchievementScore = nextAchievementScore;
        }
    }

    /// <summary>
    /// Receives new score and completed items count from the received message.
    /// </summary>
    /// <param name="message">The message containing the user's updated score and completed item count.</param>
    public void Receive(ProfileUpdatedMessage message)
    {
        // Ensuring code is executed on main thread.
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Score = message.Value.UserScore;
            CompletedItemsCount = message.Value.UserCompletedCount;
        });
        isFirstOpened = false;
    }

    /// <summary>
    /// Creates and displays a popup with the most recently acquired achievement title.
    /// </summary>
    /// <param name="achievementTitle">Title of the acquired achievement.</param>
    private void DisplayPopup(string achievementTitle)
    {
        var contentTemplate = new DataTemplate(() =>
        {
            var label = new Label
            {
                Text = achievementTitle,
                TextColor = Colors.White,
                VerticalOptions = LayoutOptions.Center
            };

            var image = new Image
            {
                Source = "trophy_star.png",
                HeightRequest = 30,
                VerticalOptions = LayoutOptions.Center
            };

            var contentLayout = new HorizontalStackLayout
            {
                Spacing = 10,
                Children = { image, label },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
            };

            return contentLayout;
        });

        SfPopup achievedPopup = new()
        {
            HeaderTitle = "Achievement Unlocked!",
            HeaderHeight = 65,
            HeightRequest = 110,
            ShowHeader = true,
            PopupStyle = new PopupStyle()
            {
                PopupBackground = Color.Parse("#341C4F"),
                HeaderTextAlignment = TextAlignment.Center,
                HeaderTextColor = Colors.White,
                HeaderFontSize = 20,
                MessageTextColor = Colors.White,
                HasShadow = true
            },

            ContentTemplate = contentTemplate
        };

        achievedPopup.Show();

    }

}
