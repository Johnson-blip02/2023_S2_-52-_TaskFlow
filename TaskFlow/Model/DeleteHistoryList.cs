using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;
using SQLite;

namespace TaskFlow.Model
{
    public class DeleteHistoryList
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(TodoItem))]
        public int todo { get; set; }
        public DateTime deleteTime { get; set; }
    }
}
