using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    [Table("TodoLabelLink")]
    public class TodoLabelLink
    {
        [ForeignKey(typeof(TodoItem))]
        public int TodoId { get; set; }
        [ForeignKey(typeof(LabelItem))]
        public int LabelId { get; set; }
    }
}
