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

        protected override async void CreateTableAsync()
        {
            await dbConn.CreateTableAsync<TodoItem>();
            await dbConn.CreateTableAsync<Label>();
        }

        protected override List<TodoItem> GetDataAbstract()
        {
            return dbConn.Table<TodoItem>().ToListAsync().Result;
        }
    }
}
