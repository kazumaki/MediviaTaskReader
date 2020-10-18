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

    private ConcurrentStack<TextMessage> messagesStack;
    private ConcurrentDictionary<string, ConcurrentQueue<TaskMessage>> messagesDictionary;
    public Storages()
    {
      this.messagesStack = new ConcurrentStack<TextMessage>();
      this.messagesDictionary = new ConcurrentDictionary<string, ConcurrentQueue<TaskMessage>>();
      this.filterMessagesCallback = this.filterMessages;
      this.filterMessagesTimer = new Timer(this.filterMessagesCallback, null, 1000, 500);
      this.cleanMessagesCallback = this.cleanMessages;
      this.cleanMessagesTimer = new Timer(this.cleanMessagesCallback, null, 1000, 3 * 60 * 1000);
    }

    public ConcurrentStack<TextMessage> MessagesStack
    {
      get { return this.messagesStack; }
      private set { }
    }
    public ConcurrentDictionary<string, ConcurrentQueue<TaskMessage>> MessagesDictionary
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
          TextMessage currentMessage;
          if (messagesStack.TryPop(out currentMessage))
          {
            string[] strippedMessage = currentMessage.Message.Split(' ');
            switch (currentMessage.Type)
            {
              case 9:
                if(strippedMessage.Length >= 14 && strippedMessage.Length <= 20)
                {
                  int nameLen = strippedMessage.Length - 14;
                  int killCount = Convert.ToInt32(strippedMessage[strippedMessage.Length - 2]);
                  string[] strippedCreatureName = new string[nameLen];
                  Array.Copy(strippedMessage, 8, strippedCreatureName, 0, nameLen);
                  string creatureName = string.Join(" ", strippedCreatureName).ToLower();
                  this.updateDictionary(creatureName, killCount, killCount);
                }
                break;

              case 16:
                if (strippedMessage.Length >= 11 && strippedMessage.Length <= 14)
                {
                  int nameLen = strippedMessage.Length - 10;
                  int current = Convert.ToInt32(strippedMessage[5]);
                  int total = Convert.ToInt32(strippedMessage[strippedMessage.Length - 2]);
                  string[] strippedCreatureName = new string[nameLen];
                  Array.Copy(strippedMessage, 6, strippedCreatureName, 0, nameLen);
                  string creatureName = string.Join(" ", strippedCreatureName).ToLower();
                  this.updateDictionary(creatureName, current, total);
                }
                break;
            }
          }
        }
      }
      catch (Exception error)
      {
        System.Windows.Forms.MessageBox.Show(error.ToString());
      }
    }

    private void updateDictionary(string creatureName, int current, int total)
    {
      if (this.messagesDictionary.ContainsKey(creatureName))
      {
        this.messagesDictionary[creatureName].Enqueue(new TaskMessage(current, total));
      }
      else
      {
        this.messagesDictionary[creatureName] = new ConcurrentQueue<TaskMessage>();
        this.messagesDictionary[creatureName].Enqueue(new TaskMessage(current, total));
      }
    }

    private void cleanMessages(object state)
    {
      this.messagesDictionary.Clear();
    }
  }
}
