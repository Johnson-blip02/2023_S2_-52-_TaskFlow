using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    [Table("TodoDayLink")]
    public class TodoDayLink
    {
        [ForeignKey(typeof(TodoItem))]
        public int TodoId { get; set; }
        [ForeignKey(typeof(Day))]
        public int DayId { get; set; }
    }
}
