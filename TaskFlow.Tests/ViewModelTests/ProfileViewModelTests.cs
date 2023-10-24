namespace TaskFlow.Tests.ViewModelTests;

public class ProfileViewModelTests
{
    [Theory]
    [InlineData(0, 0)]     // Score = 0, expected result: no achievements.
    [InlineData(1, 1)]     // Score = 1, expected result: 1 achievement.  
    [InlineData(1070, 20)] // Score = 1070, expected result: all achievements.
    [InlineData(420, 12)]  // Score = 420, expected result: 12 achievements.
    public void LoadAchievements_GivenNewScore_ShouldLoadCorrectAchievements(int score, int expectedAchievementsCount)
    {
        // Arrange
        var viewModel = new ProfileViewModel();

        // Act
        viewModel.LoadAchievements(score);

        // Assert
        Assert.Equal(expectedAchievementsCount, viewModel.UserAchievements.Count);
    }

    [Theory]
    [InlineData(0, 0)]     // Score = 0, expected result: no progress, 0.
    [InlineData(1, 0)]     // Score = 1, expected result: progress reverts to 0 (preparing for next achievement).
    [InlineData(1070, 1)]  // Score = 1070, expected result: earned all achievements so progress = 1.
    [InlineData(85, 0.5)]  // Score = 85 halfway between 2 achievements, expected result: progress is halfway 0.5
    public void LoadProgress_GivenScore_ShouldShowCorrectProgress(int score, double expectedProgress)
    {
        // Arrange
        var viewModel = new ProfileViewModel();
        viewModel.Score = score;

        // Act
        viewModel.LoadProgress();

        // Assert
        Assert.Equal(expectedProgress, viewModel.Progress);              
    }

    [Theory]
    [InlineData(0, 1)]     // Score = 0, expected result: next achievement score = 1
    [InlineData(1, 15)]    // Score = 1, expected result: next achievement score = 15
    [InlineData(1070, 0)]  // Score = 1070, expected result: no more achievements, next achievement score = 0
    [InlineData(76, 100)]  // Score = 75 , expected result: next achievement score = 100
    public void LoadProgress_GivenScore_ShouldShowCorrectNextAchievementScore(int score, int expectedNextAchievementScore)
    {
        // Arrange
        var viewModel = new ProfileViewModel();
        viewModel.Score = score;

        // Act
        viewModel.LoadProgress();

        // Assert
        Assert.Equal(expectedNextAchievementScore, viewModel.NextAchievementScore);
    }
}




