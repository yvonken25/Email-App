using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfAEmailApp.Models;

namespace WpfAEmailApp.ViewModels
{
    public class TodoViewModel : INotifyPropertyChanged
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");
        private string _newTaskTitle;
        private TaskItem _selectedTask;

        public ObservableCollection<TaskItem> Tasks { get; set; }

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set
            {
                _newTaskTitle = value;
                OnPropertyChanged();
            }
        }

        public TaskItem SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand SaveTasksCommand { get; }

        public TodoViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>();

            AddTaskCommand = new RelayCommand(AddTask, () => !string.IsNullOrWhiteSpace(NewTaskTitle));
            DeleteTaskCommand = new RelayCommand(DeleteTask, () => SelectedTask != null);
            SaveTasksCommand = new RelayCommand(SaveTasks);

            // Charger les tâches existantes au démarrage
            LoadTasks();
        }

        private void AddTask()
        {
            Tasks.Add(new TaskItem { Title = NewTaskTitle, IsDone = false });
            NewTaskTitle = string.Empty;
        }

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                Tasks.Remove(SelectedTask);
            }
        }

        private void SaveTasks()
        {
            try
            {
                var lines = new List<string>();
                foreach (var task in Tasks)
                {
                    lines.Add($"{task.IsDone};{task.Title}");
                }
                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erreur lors de la sauvegarde : " + ex.Message);
            }
        }

        private void LoadTasks()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var lines = File.ReadAllLines(_filePath);
                    Tasks.Clear();
                    foreach (var line in lines)
                    {
                        var parts = line.Split(new[] { ';' }, 2);
                        if (parts.Length == 2)
                        {
                            bool.TryParse(parts[0], out bool isDone);
                            Tasks.Add(new TaskItem { IsDone = isDone, Title = parts[1] });
                        }
                    }
                }
                catch
                {
                    // Fichier vide ou corrompu
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
