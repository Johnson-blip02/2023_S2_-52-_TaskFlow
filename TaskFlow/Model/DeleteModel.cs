using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    public class DeleteModel : Database<DeleteHistoryList>, IDatabase<DeleteHistoryList>
    {
        /// <summary>
        /// Creates a new object for managing todo items in the database.
        /// <list type="">
        /// Initializes its abstract parent:
        /// <see cref="Database{T}"/>
        /// </list></summary>
        public DeleteModel() : base()
        {
            this.hasUpdates = true;
        }

        protected override void CreateTableAsync()
        {
            dbConn.CreateTables<TodoItem, DeleteHistoryList>();
        }

        protected override List<DeleteHistoryList> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<DeleteHistoryList>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Will insert a new item into the database based on the todo item.
        /// Used for setting up the delete time for an item.
        /// </summary>
        /// <param name="item"></param>
        public void SetupDeleteTime(TodoItem item)
        {
            if(this.GetData().Any(x => x.todo == item.Id))
            {
                return;
            }

            DeleteHistoryList history = new DeleteHistoryList();
            history.todo = item.Id;
            history.deleteTime = DateTime.Now;
            this.Insert(history);
        }

        /// <summary>
        /// Removes an delete history entry from the database based on the input
        /// todo item.
        /// </summary>
        /// <param name="todo"></param>
        public void Delete(TodoItem todo)
        {
            foreach(var item in this.GetData())
            {
                if(item.todo == todo.Id)
                {
                    base.Delete(item);
                }
            }
        }

        /// <summary>
        /// When run will delete any items that have been in the trash for more than 30 days.
        /// </summary>
        public void AutoDelete()
        {
            TodoModel tm = new TodoModel();
            foreach(var item in this.GetData())
            {
                if(item.deleteTime < DateTime.Now - TimeSpan.FromDays(30))
                {
                    tm.Delete(item.todo);
                    this.Delete(item);
                }
            }
        }
    }
}
