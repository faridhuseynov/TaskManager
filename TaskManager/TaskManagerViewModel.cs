using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Dynamic;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;

namespace TaskManager
{
    class TaskManagerViewModel : ViewModelBase
    {
        private ObservableCollection<TaskItem> taskList;
        public ObservableCollection<TaskItem> TaskList { get => taskList; set => Set(ref taskList, value); }

        private string newTaskRequest;
        public string NewTaskRequest { get => newTaskRequest; set => Set(ref newTaskRequest, value); }

        private TaskItem selectedItem;
        public TaskItem SelectedItem { get => selectedItem; set => Set(ref selectedItem, value); }

        public TaskManagerViewModel()
        {
            TaskList = new ObservableCollection<TaskItem>();
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                var newTask = new TaskItem();
                newTask.Name = processes[i].ProcessName.ToString();
                newTask.PID = processes[i].Id;
                newTask.UserName = GetProcessExtraInformation(newTask.PID);
                try
                {
                newTask.CPU = $"{processes[i].TotalProcessorTime.ToString()[0]}{processes[i].TotalProcessorTime.ToString()[1]}";
                newTask.Description = processes[i].MainModule.FileVersionInfo.FileDescription;
                }
                catch (Exception)
                {
                    
                }
                newTask.Memory = (Convert.ToDouble(processes[i].PrivateMemorySize64) / 1024).ToString() + " K";
                TaskList.Add(newTask);
            }

        }
        public string GetProcessExtraInformation(int processId)
        {
            // Query the Win32_Process
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            // Create a dynamic object to store some properties on it
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            {
                // Retrieve username 
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return Username
                   return response.Username = argList[0];

                    // You can return the domain too like (PCDesktop-123123\Username using instead
                    //response.Username = argList[1] + "\\" + argList[0];
                }
            }

            return "N/A";
        }

        private RelayCommand endTaskCommand;
        public RelayCommand EndTaskCommand
        {
            get => endTaskCommand ?? (endTaskCommand = new RelayCommand(
               () =>
                {
                    var request = Process.GetProcessById(SelectedItem.PID);
                    if (request!=null)
                    {
                        request.Kill();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TaskList.Remove(SelectedItem);
                        });
                        SelectedItem = null;
                    }
                }
            ));
        }

        private RelayCommand startTaskCommand;
        public RelayCommand StartTaskCommand
        {
            get => startTaskCommand ?? (startTaskCommand = new RelayCommand(
               () =>
               {
                   if (!String.IsNullOrEmpty(NewTaskRequest))
                   {
                       Process.Start(NewTaskRequest);
                       var request = Process.GetProcessesByName(NewTaskRequest);
                       NewTaskRequest = "";
                       if (request != null)
                       {
                           Application.Current.Dispatcher.Invoke(() =>
                           {
                               TaskList.Add(SelectedItem);
                           });
                       }
                   }
                   else
                       MessageBox.Show("Check the task name");
                   SelectedItem = null;
               }
            ));
        }
    }

}
