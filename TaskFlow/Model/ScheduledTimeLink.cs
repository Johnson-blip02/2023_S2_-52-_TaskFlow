using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    [Table("ScheduledTimeLink")]
    public class ScheduledTimeLink
    {
        [ForeignKey(typeof(TodoItem))]
        public int TodoId { get; set; }
        [ForeignKey(typeof(ScheduledTime))]
        public int ScheduledTimeId { get; set; }
    }
}
