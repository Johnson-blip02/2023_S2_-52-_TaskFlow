using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    /// <summary>
    /// Abstract class for creating and accessing database tables.
    /// </summary>
    /// <typeparam name="T">Data Object type to be the basis of the table</typeparam>
    public abstract class Database<T>
    {
        /// <summary>
        /// Connection for accessing the database and executing commands
        /// </summary>
        protected static SQLiteAsyncConnection dbConn;
        /// <summary>
        /// If true, the database has a updates not local to memory
        /// </summary>
        protected bool hasUpdates;
        /// <summary>
        /// The list of objects stored in the database's table
        /// </summary>
        protected List<T> data;

        /// <summary>
        /// Creates a new database object. Establishes a connection with the local DB.
        /// </summary>
        public Database()
        {
            dbConn = EstablishConnection();
            CreateTableAsync();
            this.data = new List<T>();
        }
        /// <summary>
        /// Establishes a new database connection in the current app's directory
        /// </summary>
        /// <returns>SQLiteAsyncConnection</returns>
        private SQLiteAsyncConnection EstablishConnection()
        {
            //Specify the store location of the database -> app data not cache
            string dbLocation =  Path.Combine(FileSystem.Current.AppDataDirectory, "taskflow.db");
            return new SQLite.SQLiteAsyncConnection(dbLocation); //Create a new connection and return it
        }

        protected abstract void CreateTableAsync();

        protected abstract List<T> GetDataAbstract();

        public List<T> GetData()
        {
            if (this.hasUpdates)
                this.data = this.GetDataAbstract();

            this.hasUpdates = false;

            return this.data;
        }

        protected int Insert(T data)
        {
            this.hasUpdates = true;
            return dbConn.InsertAsync(data).Result;
        }

        protected int InsertAll(List<T> data)
        {
            this.hasUpdates = true;
            return dbConn.InsertAllAsync(data).Result;
        }

        protected int Delete(T data)
        {
            this.hasUpdates = true;
            return dbConn.DeleteAsync(data).Result; 
        }

    }
}
