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

        /// <summary>
        /// Removes the todo item from the database based on the id of the input
        /// todo item.
        /// </summary>
        /// <param name="id">Id of the task to be deleted</param>
        public void Delete(int id)
        {
            foreach(var item in GetData())
            {
                if(item.Id == id)
                {
                    base.Delete(item);
                    return;
                }
            }
        }

        /// <summary>
        /// Calculates the proirity of each todo item in the database. Calls the
        /// _calculate method to do the actual calculation.
        /// </summary>
        public void CalculatePriority()
        {
            DeleteModel dm = new DeleteModel();
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

        /// <summary>
        /// Calculates the priority of a todo item based on its importance and minitues till
        /// due.
        /// </summary>
        /// <param name="importance">The importance value of the task</param>
        /// <param name="minutes">How many minutes till the task is due</param>
        /// <returns></returns>
        private double _Calculate(int importance, double minutes)
        {
            double priority = (4 * Math.Log(importance, 1.1)) -
                Math.Log(minutes, 1.1) + 101 - Math.Pow(importance,2);

            return priority;
        }
    }
}
