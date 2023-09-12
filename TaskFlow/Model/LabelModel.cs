using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    internal class LabelModel : Database<LabelItem>
    {
        public LabelModel() : base()
        {
            this.hasUpdates = true;
        }
        protected override void CreateTableAsync()
        {
            dbConn.CreateTables<LabelItem, TodoItem, TodoLabelLink>();
        }

        protected override List<LabelItem> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<LabelItem>();
            }
            catch
            {
                return null;
            }
        }
    }
}
