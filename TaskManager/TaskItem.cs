using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    class TaskItem
    {
        public string Name { get; set; }
        public int PID { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string CPU { get; set; }
        public string Memory { get; set; }
        public string Description { get; set; }
    }
}
