using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    class TaskManagerViewModel : ViewModelBase
    {
        private string taskName;
        public string TaskName { get => taskName; set => Set(ref taskName, value); }

        private string pid;
        public string PID { get => pid; set => Set(ref pid, value); }


        private string status;
        public string Status { get => status; set => Set(ref status, value); }

        private string userName;
        public string UserName { get => userName; set => Set(ref userName, value); }

        private string cpu;
        public string CPU { get => cpu; set => Set(ref cpu, value); }

        private string memory;
        public string Memory { get => memory; set => Set(ref memory, value); }

        private string description;
        public string Description { get => description; set => Set(ref description, value); }

        private ObservableCollection<TaskItem> taskList;
        public ObservableCollection<TaskItem> TaskList { get => taskList; set => Set(ref taskList, value); }

        public TaskManagerViewModel()
        {
            TaskList = new ObservableCollection<TaskItem>();
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                TaskList.Add(new TaskItem
                {
                    Name = processes[i].ProcessName.ToString(),
                    PID=processes[i].Id,
                    Status=processes[i].get
                    CPU=processes[i].TotalProcessorTime.ToString(),
                    Memory=

                });
            }

        }
    }

}
