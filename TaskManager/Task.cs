using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class Task
    {
        public int ID {  get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }

        [ConcurrencyCheck]
        public Guid Version { get; set; }
    }
}
