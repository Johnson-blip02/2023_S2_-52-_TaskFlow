using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    public class TodoDayLink
    {
        [ForeignKey(typeof(TodoItem))]
        public TodoItem todo { get; set; }
        [ForeignKey(typeof(Day))]
        public Day day { get; set; }
    }
}
