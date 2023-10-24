using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    [Table("ScheduledTime")]
    public class ScheduledTime
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //Id of the entry
        [MaxLength(255), NotNull]
        
        public DateTime StartTime { get; set; } // Times a todo item is scheduled


        [ManyToMany(typeof(ScheduledTimeLink))]
        public List<TodoItem> TodoItem { get; set; }

        /// <summary>
        /// Create a new StartTime.
        /// </summary>
        public ScheduledTime() { }
        public ScheduledTime(DateTime scheduledTime)
        {
            this.StartTime = scheduledTime;
        }

        /// <summary>
        /// Returns hash code as the current StartTime's unique Id.
        /// </summary>
        /// <returns>int Id of the current StartTime's instance.</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Checks if the current scheduled time instance is equal to another scheduled time.
        /// </summary>
        /// <param name="obj">StartTime to be compared with.</param>
        /// <returns>True if the object is not null, an instance of a <see cref="ScheduledTime"/>, 
        /// and has the same Id as the current StartTime item; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj is not ScheduledTime) 
                return false;
            else
                return GetHashCode() == ((ScheduledTime)obj).GetHashCode();
        }
    }
}
