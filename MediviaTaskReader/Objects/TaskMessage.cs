using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediviaTaskReader.Objects
{
  public class TaskMessage
  {
    private DateTime date;
    private string message;
    private int current;
    private int total;
    public TaskMessage(string message, int current, int total)
    {
      this.message = message;
      this.current = current;
      this.total = total;
      this.date = DateTime.Now;
    }

    public DateTime Date
    {
      get
      {
        return this.date;
      }
    }

    public int Current
    {
      get
      {
        return this.current;
      }
    }

    public int Total
    {
      get
      {
        return this.total;
      }
    }

    public string Message
    {
      get
      {
        return this.message;
      }
    }

  }
}
