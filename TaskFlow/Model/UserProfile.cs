using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    /// <summary>
    /// User profile class for storing attributes of which there are only one record. Initialized using:
    /// <code>
    /// new UserProfile() { };
    /// </code>
    /// </summary>
    [Table("UserProfile")]
    public class UserProfile
    {
        [PrimaryKey]
        public int Id { get; set; } = 1;  // Ensures only one record in the database.

        public int Score { get; set; } = 0;

        public UserProfile() { }
    }
}
