using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    public class Day
    {
        public int Id { get; set; } //ID for the Day
        public DateTime Date { get; set; } //The day on which the items are for
        public List<TodoItem> DaysItems { get; set; } //The todo items that are assigned for today

        /// <summary>
        /// Create a new day which todo items can be added to. These items are to be carried out today.
        /// </summary>
        public Day ()
        {
            this.DaysItems = new List<TodoItem>();
        }
    }
}
