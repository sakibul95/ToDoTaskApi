using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApp.Models
{
    public class ToDoTasks
    {
        public Guid? ID         { get; set; }
        public int SLNo       { get; set; }
        public string Task       { get; set; }
        public int IsComplete { get; set; }
    }
}
