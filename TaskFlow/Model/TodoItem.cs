using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    /// <summary>
    /// Todo class for assigning attributes for a task item. Initialized using:
    /// <code>
    /// new TodoItem { };
    /// </code>
    /// </summary>
    [Table("TodoItem")]
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255), NotNull]
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public DateTime DueDate { get; set; }
        public TimeSpan TimeBlock { get; set; }

        [ManyToMany(typeof(TodoLabelLink))]
        public List<LabelItem> Labels { get; set; } = new List<LabelItem>();
        public int Importance { get; set; } = 0;
        public int Priority { get; set; } = 0;
        public bool Completed { get; set; } = false;
        public DateTime DayAllocation { get; set; }
        public bool InTrash { get; set; } = false;
        public bool Archived { get; set; } = false;
        public string Color { get; set; } = "white";

        /// <summary>
        /// Creates a new Todo item. To create a new item either:
        /// <example>
        /// <code>
        /// TodoItem item = new TodoItem("title")
        /// {
        ///     Description = "description",
        ///     ...
        /// }
        /// </code>
        /// <b>Or by:</b>
        /// <code>
        /// TodoItem item = new TodoItem();
        /// </code>
        /// </example>
        /// </summary>
        public TodoItem(string title)
        {
            this.Title = title;
            this.Labels = new List<LabelItem>();
        }

        public TodoItem() { }

        /// <summary>
        /// Adds a new label to the task. If the label already is assigned to the todo item, will ignore the add.
        /// </summary>
        /// <param name="label">Label to add</param>
        public void AddLabel(LabelItem label)
        {
            if (!this.Labels.Contains(label))
                this.Labels.Add(label);
        }

        /// <summary>
        /// Removes a label from the todo item
        /// </summary>
        /// <param name="label">Label to remove</param>
        public void RemoveLabel(LabelItem label)
        {
            this.Labels.Remove(label);
        }

        /// <summary>
        /// Will return a list of time spans representing available time blocks
        /// </summary>
        /// <returns>List&lt;TimeSpan&gt; </returns>
        public static List<TimeSpan> TimeBlockGenerator()
        {
            List<TimeSpan> list = new List<TimeSpan>();
            for (int i = 0; i <= 24; i++)
            {
                TimeSpan increment = new TimeSpan(0, i * 15, 0);
                list.Add(increment);
            }

            return list;
        }
    }
}
