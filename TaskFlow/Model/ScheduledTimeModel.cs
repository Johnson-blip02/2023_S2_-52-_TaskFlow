using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    public class ScheduledTimeModel : Database<ScheduledTime>, IDatabase<ScheduledTime>
    {
        public ScheduledTimeModel() : base()
        {
            this.hasUpdates = true;
        }
        protected override void CreateTableAsync()
        {
            dbConn.CreateTables<ScheduledTime, TodoItem, ScheduledTimeLink>();
        }

        protected override List<ScheduledTime> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<ScheduledTime>();
            }
            catch
            {
                return null;
            }
        }
    }
}
