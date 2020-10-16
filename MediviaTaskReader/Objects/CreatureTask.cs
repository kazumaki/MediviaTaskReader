using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using MediviaTaskReader.IPC;

namespace MediviaTaskReader.Objects
{
  public class CreatureTask
  {
    private int current;
    private int total;
    private string name;
    private MainLayer ipc;
    private TimerCallback updateTimerCallback;
    private Timer updateTimer;
    string allFileName;
    string creatureFileName;
    string currentFileName;
    string totalFileName;
    public CreatureTask(string name, MainLayer ipc)
    {
      this.updateTimerCallback = this.update;
      this.updateTimer = new Timer(this.updateTimerCallback, "Test", 1000, 1000);
      this.name = name;
      this.allFileName = Directory.GetCurrentDirectory() + $@"\tasks\{this.getTaskName()}_all.txt";
      this.creatureFileName = Directory.GetCurrentDirectory() + $@"\tasks\{this.getTaskName()}_creature.txt";
      this.currentFileName = Directory.GetCurrentDirectory() + $@"\tasks\{this.getTaskName()}_current.txt";
      this.totalFileName = Directory.GetCurrentDirectory() + $@"\tasks\{this.getTaskName()}_total.txt";
      this.startFiles();
      this.current = 0;
      this.total = 0;
      this.ipc = ipc;
    }

    public int GetCurrent()
    {
      return this.current;
    }

    public void SetCurrent(int value)
    {
      this.current = value;
    }

    public int GetTotal()
    {
      return this.total;
    }

    public string GetName()
    {
      return this.name;
    }

    public void SetTotal(int value)
    {
      this.total = value;
    }

    private string getTaskName()
    {
      return this.name.ToLower() + 's';
    }

    public override string ToString()
    {
      return this.name + " " + this.current.ToString() + "/" + this.total.ToString();
    }

    private void startFiles()
    {
      this.fileCreation(allFileName);
      this.fileCreation(creatureFileName);
      this.fileCreation(currentFileName);
      this.fileCreation(totalFileName);
    }

    private void fileCreation(string fileName)
    {
      if (!File.Exists(fileName))
      {
        File.Create(fileName).Close();
      }
    }

    private void update(object state)
    {
      try
      {
        if (this.ipc.MessagesDictionary.ContainsKey(this.getTaskName()))
        {
          TaskMessage result;
          if(this.ipc.MessagesDictionary[this.getTaskName()].TryPop(out result))
          {
            this.ipc.MessagesDictionary[this.getTaskName()].Clear();
            this.updateFiles(result);
          }

        }
      }catch(Exception error)
      {
        System.Windows.Forms.MessageBox.Show(error.ToString());
      }

    }

    private void updateFiles(TaskMessage message)
    {
      this.current = message.Current;
      this.total = message.Total;
      File.WriteAllText(this.allFileName, this.ToString());
      File.WriteAllText(this.creatureFileName, this.name);
      File.WriteAllText(this.currentFileName, this.current.ToString());
      File.WriteAllText(this.totalFileName, this.total.ToString());
    }
  }
}
