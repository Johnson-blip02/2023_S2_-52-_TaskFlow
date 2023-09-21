using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow
{
    public class ControlModel
    {
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string Name { get; set; }
        public Brush Color { get; set; }
    }
}