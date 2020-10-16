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
    private TimerCallback filterMessagesCallback;
    private Timer filterMessagesTimer;
    private TimerCallback cleanMessagesCallback;
    private Timer cleanMessagesTimer;

    private ConcurrentStack<string> messagesStack;
    private ConcurrentDictionary<string, ConcurrentStack<TaskMessage>> messagesDictionary;
    public Storages()
    {
      this.messagesStack = new ConcurrentStack<string>();
      this.messagesDictionary = new ConcurrentDictionary<string, ConcurrentStack<TaskMessage>>();
      this.filterMessagesCallback = this.filterMessages;
      this.filterMessagesTimer = new Timer(this.filterMessagesCallback, null, 1000, 500);
      this.cleanMessagesCallback = this.cleanMessages;
      this.cleanMessagesTimer = new Timer(this.cleanMessagesCallback, null, 1000, 3 * 60 * 1000);
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

    private void filterMessages(object state)
    {
      try
      {
        while (!this.messagesStack.IsEmpty)
        {
          string currentMessage;
          if (messagesStack.TryPop(out currentMessage))
          {
            string[] strippedMessage = currentMessage.Split(' ');
            if (strippedMessage.Length >= 11 && strippedMessage.Length <= 14)
            {
              int nameLen = strippedMessage.Length - 10;
              int current = Convert.ToInt32(strippedMessage[5]);
              int total = Convert.ToInt32(strippedMessage[strippedMessage.Length - 2]);
              string[] strippedCreatureName = new string[nameLen];
              Array.Copy(strippedMessage, 6, strippedCreatureName, 0, nameLen);
              string creatureName = string.Join(" ", strippedCreatureName).ToLower();
              if (this.messagesDictionary.ContainsKey(creatureName))
              {
                this.messagesDictionary[creatureName].Push(new TaskMessage(currentMessage, current, total));
              }
              else
              {
                this.messagesDictionary[creatureName] = new ConcurrentStack<TaskMessage>();
                this.messagesDictionary[creatureName].Push(new TaskMessage(currentMessage, current, total));
              }
            }
          }
        }
      }
      catch (Exception error)
      {
        System.Windows.Forms.MessageBox.Show(error.ToString());
      }
    }

    private void cleanMessages(object state)
    {
      this.messagesDictionary.Clear();
    }
  }
}
