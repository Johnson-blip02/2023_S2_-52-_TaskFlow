using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace TaskFlow.Model
{
    public class DayModel : Database<Day>
    {
        public DayModel() : base()
        { 
            
        }

        protected override void CreateTableAsync()
        {
            dbConn.CreateTables<TodoItem, Day, TodoDayLink>();
        }

        protected override List<Day> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<Day>();
            }
            catch
            {
                return null;
            }
        }
    }
}
