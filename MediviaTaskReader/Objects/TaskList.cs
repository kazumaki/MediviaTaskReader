using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace MediviaTaskReader.Objects
{
  public class TaskList
  {
    private TimerCallback updateCallback;
    private Timer updateTimer;
    private Form1 mainForm;
    private string allFileName;
    public TaskList(Form1 mainForm)
    {
      this.mainForm = mainForm;
      this.updateCallback = this.update;
      this.updateTimer = new Timer(this.updateCallback, null, 1000, 1000);
      this.allFileName = Directory.GetCurrentDirectory() + $@"\tasks\alltasks.txt";
      this.fileCreation(allFileName);
    }

    private void update(object state)
    {
      List<CreatureTask> selectedTasks = new List<CreatureTask>();

      this.mainForm.Invoke(new System.Action(() => selectedTasks = this.mainForm.tasksCheckedListBox.CheckedItems.Cast<CreatureTask>().ToList()));
      File.WriteAllText(this.allFileName, "");
      foreach(CreatureTask task in selectedTasks)
      {
        task.update();
        File.AppendAllText(allFileName,task.ToString() + Environment.NewLine);
      }
    }

    private void fileCreation(string fileName)
    {
      if (!File.Exists(fileName))
      {
        File.Create(fileName).Close();
      }
    }
  }
}
