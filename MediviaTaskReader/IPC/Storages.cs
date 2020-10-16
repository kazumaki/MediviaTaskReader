using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MediviaTaskReader.Objects;

namespace MediviaTaskReader.IPC
{
  public class Storages
  {
    private ConcurrentStack<string> messagesStack;
    private ConcurrentDictionary<string, ConcurrentStack<TaskMessage>> messagesDictionary;
    public Storages()
    {
      this.messagesStack = new ConcurrentStack<string>();
      this.messagesDictionary = new ConcurrentDictionary<string, ConcurrentStack<TaskMessage>>();
    }

    public ConcurrentStack<string> MessagesStack
    {
      get { return this.messagesStack; }
      private set { }
    }
    public ConcurrentDictionary<string, ConcurrentStack<TaskMessage>> MessagesDictionary
    {
      get { return this.messagesDictionary; }
      private set { }
    }
  }
}
