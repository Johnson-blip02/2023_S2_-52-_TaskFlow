using TaskFlow.View;
using TaskFlow.Model;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskFlow.ViewModel;

/// <summary>
/// ViewModel for logic of the <see cref="ProfilePage"/> View.
/// </summary>
public partial class ProfileViewModel : ObservableObject
{
    private readonly IDatabase<UserProfile> _um;  // UserProfileModel

    private UserProfile UserProfile;

    [ObservableProperty]
    private int score;

    public ProfileViewModel()
    {
        _um = App.UserProfileModel;
        UserProfile = new();
        Score = 0;
    }

    /// <summary>
    /// Loads the user's profile from the database and updates the local UserProfile and Score.
    /// </summary>
    public void LoadUserProfile()
    {
        try
        {
            var profile = _um.GetData();
            if (profile != null && profile.Count == 1)
            {
                foreach(var prof in profile)
                {
                    UserProfile = prof;
                }

                Score = UserProfile.Score;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading user profile: {ex}");
        }
    }


}
