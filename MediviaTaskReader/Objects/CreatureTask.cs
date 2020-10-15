using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public CreatureTask(string name, MainLayer ipc)
    {
      this.updateTimerCallback = this.update;
      this.updateTimer = new Timer(this.updateTimerCallback, "Test", 1000, 5000);
      this.name = name;
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
      return this.name + " " + this.current.ToString() + " " + this.total.ToString();
    }

    private void update(object state)
    {
      if (this.ipc.MessagesDictionary.ContainsKey(this.getTaskName()))
      {
        TaskMessage result;
        if(this.ipc.MessagesDictionary[this.getTaskName()].TryPop(out result))
        {
          this.current = result.Current;
          this.total = result.Total;
          System.Windows.Forms.MessageBox.Show(this.ToString());
          this.ipc.MessagesDictionary[this.getTaskName()].Clear();
        }
      }
    }
  }
}
