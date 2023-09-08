using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    [Table("LabelItem")]
    public class LabelItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //ID of the entry
        [MaxLength(255), NotNull]
        public string Title { get; set; } //The title of the label
        public string Colour { get; set; } //The display colour the label will have will default to white

        [ManyToMany(typeof(TodoLabelLink))]
        public List<TodoItem> TodoItem { get; set; }

        /// <summary>
        /// Create a new label.
        /// </summary>
        public LabelItem() { }
        public LabelItem(string title)
        {
            this.Title = title;
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
