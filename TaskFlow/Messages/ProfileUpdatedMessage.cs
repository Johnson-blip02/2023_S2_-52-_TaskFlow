using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskFlow.Messages;

public class ProfileUpdatedMessage : ValueChangedMessage<UserInfo>
{
    public ProfileUpdatedMessage(UserInfo userInfo) : base(userInfo)
    {
    }
}

public class UserInfo
{
    public int UserScore { get; set; }
    public int UserCompletedCount { get; set; }
}
