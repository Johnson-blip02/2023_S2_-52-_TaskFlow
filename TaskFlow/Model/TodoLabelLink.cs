using SQLiteNetExtensions.Attributes;

namespace TaskFlow.Model
{
    public class TodoLabelLink
    {
        [ForeignKey(typeof(Label))]
        public Label label { get; set; }
        [ForeignKey(typeof(TodoItem))]
        public TodoItem tdItem { get; set; }
    }
}
