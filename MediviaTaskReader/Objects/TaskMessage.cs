﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediviaTaskReader.Objects
{
  public class TaskMessage
  {
    private DateTime date;

    private int current;
    private int total;
    public TaskMessage(int current, int total)
    {
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

  }
}
