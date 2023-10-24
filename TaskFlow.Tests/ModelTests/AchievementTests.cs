namespace TaskFlow.Tests.ModelTests;

public class AchievementTests
{
    [Fact]
    public void MeetsRequirements_GivenPassingScore_ShouldReturnTrue()
    {
        // Arrange
        int userScore = 20;
        Achievement achievement = new Achievement("Test Achievement", score => score >= 10);

        // Act
        bool meetsRequirements = achievement.MeetsRequirements(userScore);

        // Assert
        Assert.True(meetsRequirements);
    }

    [Fact]
    public void MeetsRequirements_GivenNonPassingScore_ShouldReturnFalse()
    {
        // Arrange
        int userScore = 5;
        Achievement achievement = new Achievement("Test Achievement", score => score >= 10);

        // Act
        bool meetsRequirements = achievement.MeetsRequirements(userScore);

        // Assert
        Assert.False(meetsRequirements);
    }
}

public class AchievementManagerTests
{

    [Fact]
    public void ScoreRequirements_GivenValues_ShouldContainValuesInOrder()
    {
        // Arrange
        List<int> scoreRequirements = AchievementManager.ScoreRequirements;

        // Assert
        for (int i = 1; i < scoreRequirements.Count; i++)
        {
            Assert.True(scoreRequirements[i] > scoreRequirements[i - 1]);
        }
    }
}
