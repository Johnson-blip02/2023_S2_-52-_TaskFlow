using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskFlow.Messages;

/// <summary>
/// Class which represents a message indicating that a user's profile information has been updated.
/// </summary>
/// <remarks>
/// Inherits from the ValueChangedMessage class with UserInfo as the value type.
/// </remarks>
public class ProfileUpdatedMessage : ValueChangedMessage<UserInfo>
{
    public ProfileUpdatedMessage(UserInfo userInfo) : base(userInfo)
    {
    }
}

/// <summary>
/// Class which represents user-specific information including their score and sum of completed todo items.
/// </summary>
public class UserInfo
{
    public int UserScore { get; set; }
    public int UserCompletedCount { get; set; }
}
