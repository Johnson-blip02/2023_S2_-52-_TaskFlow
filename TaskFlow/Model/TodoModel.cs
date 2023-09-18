using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
namespace TaskFlow.Model
{
    public class TodoModel : Database<TodoItem>
    {
        /// <summary>
        /// Creates a new object for managing todo items in the database.
        /// <list type="">
        /// Initializes its abstract parent:
        /// <see cref="Database{T}"/>
        /// </list></summary>
        public TodoModel() : base() 
        {
            this.hasUpdates = true;
        }

        protected override void CreateTableAsync()
        {
            dbConn.CreateTables<TodoItem, LabelItem, TodoLabelLink>();
        }

        protected override List<TodoItem> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<TodoItem>();
            }
            catch
            {
                return null;
            }
        }
    }
}
