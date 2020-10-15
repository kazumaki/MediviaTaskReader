using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Pipes;
using System.IO;
using MediviaTaskReader.Objects;
using MediviaTaskReader.APICall;

namespace MediviaTaskReader.IPC
{
  public class MainLayer
  {
    private bool isConnected;
    private NamedPipeServerStream namedPipeServer;
    private Thread processMessagesThread;
    private ConcurrentStack<string> messagesStack;
    private ConcurrentDictionary<string, ConcurrentStack<TaskMessage>> messagesDictionary;
    private List<CreatureTask> tasks;
    private Timer filterMessagesTimer;
    private TimerCallback filterMessagesTimerCallback;
    public MainLayer()
    {
      this.isConnected = false;
      this.messagesStack = new ConcurrentStack<string>();
      this.messagesDictionary = new ConcurrentDictionary<string, ConcurrentStack<TaskMessage>>();
      this.processMessagesThread = new Thread(() => this.processMessagesMethod())
      {
        IsBackground = true
      };
    }

    public ConcurrentDictionary<string, ConcurrentStack<TaskMessage>> MessagesDictionary
    {
      get
      {
        return this.messagesDictionary;
      }
    }

    public void Connect(Character character, List<CreatureTask> tasks)
    {
      this.tasks = tasks;
      if(this.isConnected)
      {
        return;
      }

      DLLInjector.Inject(character.Handle(), @Directory.GetCurrentDirectory() + @"\taskreader.dll");
      this.namedPipeServer = new NamedPipeServerStream("MediviaTaskReader", PipeDirection.InOut, 100, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

      this.processMessagesThread.Start();
      this.filterMessagesTimerCallback = this.filterMessages;
      this.filterMessagesTimer = new Timer(this.filterMessagesTimerCallback, "Test", 1000, 5000);
    }

    public void Disconnect()
    {
      if(!this.isConnected)
      {
        return;
      }

      this.namedPipeServer.Disconnect();
    }

    private void processMessagesMethod()
    {
      this.namedPipeServer.WaitForConnection();
      System.Windows.Forms.MessageBox.Show("Conectado");
      while (true)
      {
        try
        {
          string message = Encoding.UTF8.GetString(this.readBytes()).Trim('\0');
          this.messagesStack.Push(message);
        }
        catch
        {

        }
      }
    }

    private void filterMessages(object state)
    {
      HashSet<string> alreadyAppeared = new HashSet<string>();

      while (!this.messagesStack.IsEmpty)
      {
        string currentMessage;
        messagesStack.TryPop(out currentMessage);
        string[] strippedMessage = currentMessage.Split(' ');

        if(strippedMessage.Length >= 11 && strippedMessage.Length <= 14)
        {
          int nameLen = strippedMessage.Length - 10;
          int current = Convert.ToInt32(strippedMessage[5]);
          int total = Convert.ToInt32(strippedMessage[strippedMessage.Length - 2]);
          string[] strippedCreatureName = new string[nameLen];
          Array.Copy(strippedMessage, 6, strippedCreatureName, 0, nameLen);
          string creatureName = string.Join(" ", strippedCreatureName).ToLower();
          if(this.messagesDictionary.ContainsKey(creatureName))
          {
            this.messagesDictionary[creatureName].Push(new TaskMessage(currentMessage, current, total));
          }
          else
          {
            this.messagesDictionary[creatureName] = new ConcurrentStack<TaskMessage>();
          }
        }
      }
    }

    private byte[] readBytes()
    {
      byte[] buffer = new byte[512];
      this.namedPipeServer.Read(buffer, 0, buffer.Length);
      return buffer;
    }

  }
}
