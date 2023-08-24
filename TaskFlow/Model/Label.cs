using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Model
{
    public class Label
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //ID of the entry
        [MaxLength(255), NotNull]
        public required string Title { get; set; } //The title of the label
        public Color Colour { get; set; } = Colors.White; //The display colour the label will have will default to white

        /// <summary>
        /// Create a new label. Has a default colour of white.
        /// </summary>
        public Label() { }
    }
}
