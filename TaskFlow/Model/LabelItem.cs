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

        /// <summary>
        /// Returns hash code as the current label item's unique Id.
        /// </summary>
        /// <returns>int Id of the current label item instance.</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Checks if the current label item instance is equal to another label item.
        /// </summary>
        /// <param name="obj">Label item to be compared with.</param>
        /// <returns>True if the object is not null, an instance of a <see cref="LabelItem"/>, 
        /// and has the same Id as the current label item; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj is not LabelItem) 
                return false;
            else
                return GetHashCode() == ((LabelItem)obj).GetHashCode();
        }
    }
}
