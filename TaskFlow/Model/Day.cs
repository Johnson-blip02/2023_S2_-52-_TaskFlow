using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    [Table("Day")]
    public class Day
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //ID for the Day
        [NotNull]
        public DateTime Date { get; set; } //The day on which the items are for
        [ManyToMany(typeof(TodoDayLink))]
        public List<TodoItem> DaysItems { get; set; } //The todo items that are assigned for today

        /// <summary>
        /// Create a new day which todo items can be added to. These items are to be carried out today.
        /// </summary>
        public Day()
        {
            this.DaysItems = new List<TodoItem>();
        }

        public void InitalizeDay()
        {
            this.Date = DateTime.Now.Date;
        }
    }
}
