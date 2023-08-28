using SQLite;

namespace Practice.Model
{
    public class Label
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //ID of the entry
        [MaxLength(255), NotNull]
        public string Title { get; set; } //The title of the label
        public string Colour { get; set; } = "white"; //The display colour the label will have will default to white

        /// <summary>
        /// Create a new label. Has a default colour of white.
        /// </summary>
        public Label() { }
    }
}
