using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediviaTaskReader.Objects
{
  public class CreatureTask
  {
    private int current;
    private int total;
    public CreatureTask(string name = "")
    {
      this.current = 0;
      this.total = 0;
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

    public void SetTotal(int value)
    {
      this.total = value;
    }
  }
}
