using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
namespace TaskFlow.Model
{
    public class TodoModel : Database<TodoItem>, IDatabase<TodoItem>
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

        public void CalculatePriority()
        {
            var data = this.GetData();
            foreach (var item in data)
            {
                TimeSpan minutes = item.DueDate.Subtract(DateTime.Today);
                item.Priority = (int)_Calculate(item.Importance, minutes.TotalMinutes);
                if(item.Priority < 0)
                {
                    item.Priority = 0;
                }
            }

            this.InsertAll(data);
        }
        private double _Calculate(int importance, double minutes)
        {
            double priority = (4 * Math.Log(importance, 1.1)) -
                Math.Log(minutes, 1.1) + 101 - Math.Pow(importance,2);

            return priority;
        }
    }
}
