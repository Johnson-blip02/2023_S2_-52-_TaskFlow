using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    internal class TodoModel : Database<TodoItem>
    {
        /// <summary>
        /// Creates a new object for managing todo items in the database.
        /// <list type="">
        /// Initializes its abstract parent:
        /// <see cref="Database{T}"/>
        /// </list></summary>
        public TodoModel() : base() 
        {

        }

        protected override async void CreateTableAsync()
        {
            await dbConn.CreateTableAsync<TodoItem>();
        }

        protected override List<TodoItem> GetDataAbstract()
        {
            return dbConn.Table<TodoItem>().ToListAsync().Result;
        }
    }
}
