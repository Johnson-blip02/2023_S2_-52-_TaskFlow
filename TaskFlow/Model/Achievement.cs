namespace TaskFlow.Model;

/// <summary>
/// Class which represents an achievement with a title and defined requirements for unlocking.
/// </summary>
public class Achievement
{
    public string Title { get; private set; }
    public Func<int, bool> Requirements { get; private set; }

    public Achievement(string title, Func<int, bool> requirements)
    {
        Title = title;
        Requirements = requirements;
    }

    public bool MeetsRequirements(int userScore)
    {
        return Requirements(userScore); ;
    }
}

/// <summary>
/// Class which stores and manages the available achievements and their respective score requirements.
/// </summary>
public class AchievementManager
{
    public static List<int> ScoreRequirements { get; private set; } = new List<int> { 1, 15 , 45, 70, 100, 135, 175, 220, 270, 320, 370, 420, 470, 520, 570, 670, 770, 870, 970, 1070 };

    public static List<Achievement> AvailableAchievements { get; private set; } = new List<Achievement>
    {
        new Achievement("Beginner", score => score >= ScoreRequirements[0]),
        new Achievement("Task tackler", score => score >= ScoreRequirements[1]),
        new Achievement("Productivity Pro", score => score >= ScoreRequirements[2]),
        new Achievement("Efficiency Expert", score => score >= ScoreRequirements[3]),
        new Achievement("Goal Getter", score => score >= ScoreRequirements[4]),
        new Achievement("Workflow Ace", score => score >= ScoreRequirements[5]),
        new Achievement("Deadline Dynamo", score => score >= ScoreRequirements[6]),
        new Achievement("Scheduler Champ", score => score >= ScoreRequirements[7]),
        new Achievement("Time Master", score => score >= ScoreRequirements[8]),
        new Achievement("Focus Wizard", score => score >= ScoreRequirements[9]),
        new Achievement("Completion Champ", score => score >= ScoreRequirements[10]),
        new Achievement("Milestone Mover", score => score >= ScoreRequirements[11]),
        new Achievement("Priority Whiz", score => score >= ScoreRequirements[12]),
        new Achievement("List Ninja", score => score >= ScoreRequirements[13]),
        new Achievement("Task Titan", score => score >= ScoreRequirements[14]),
        new Achievement("Efficiency Emperor", score => score >= ScoreRequirements[15]),
        new Achievement("Goal Grandmaster", score => score >= ScoreRequirements[16]),
        new Achievement("Planner Genius", score => score >= ScoreRequirements[17]),
        new Achievement("Todo Tactician", score => score >= ScoreRequirements[18]),
        new Achievement("Achievement Annihilator", score => score >= ScoreRequirements[19])
    };
}
