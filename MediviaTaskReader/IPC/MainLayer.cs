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
    private Storages storages;
    private NamedPipeServerStream sendDataPipeServer;
    private NamedPipeServerStream receiveDataPipeServer;
    private Thread processMessagesThread;

    private Timer filterMessagesTimer;
    private TimerCallback filterMessagesTimerCallback;
    private Timer cleanMessagesTimer;
    private TimerCallback cleanMessagesTimerCallback;
    public MainLayer()
    {
      this.isConnected = false;
      this.storages = new Storages();
    }

    public bool IsConnected
    {
      get { return this.isConnected; }
      private set { }
    }

    public Storages DataStorages
    {
      get { return this.storages; }
      private set { }
    }

    public void Connect(Character character)
    {
      if(this.isConnected)
      {
        return;
      }

      DLLInjector.Inject(character.Handle(), @Directory.GetCurrentDirectory() + @"\taskreader.dll");
      this.sendDataPipeServer = new NamedPipeServerStream("MediviaTaskReaderSend", PipeDirection.InOut, 100, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
      this.receiveDataPipeServer = new NamedPipeServerStream("MediviaTaskReaderReceive", PipeDirection.InOut, 100, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
      this.processMessagesThread = new Thread(() => this.processMessagesMethod())
      {
        IsBackground = true
      };
      this.processMessagesThread.Start();
      this.filterMessagesTimerCallback = this.filterMessages;
      this.filterMessagesTimer = new Timer(this.filterMessagesTimerCallback, "Test", 1000, 500);
      this.cleanMessagesTimerCallback = this.cleanMessages;
      this.cleanMessagesTimer = new Timer(this.cleanMessagesTimerCallback, "Test", 1000, 30000);
      this.isConnected = true;
    }

    public void Disconnect()
    {
      if(!this.isConnected)
      {
        return;
      }

      this.isConnected = false;
      this.receiveDataPipeServer.Write(BitConverter.GetBytes(0xFF), 0, 1);
      this.sendDataPipeServer.Disconnect();
      this.receiveDataPipeServer.Disconnect();
      this.processMessagesThread.Abort();
    }

    private void processMessagesMethod()
    {
      this.sendDataPipeServer.WaitForConnection();
      this.receiveDataPipeServer.WaitForConnection();
      System.Windows.Forms.MessageBox.Show("Conectado");
      while (this.isConnected)
      {
        try
        {
          string message = Encoding.UTF8.GetString(this.readBytes()).Trim('\0');
          this.storages.MessagesStack.Push(message);
        }
        catch(Exception error)
        {
          if(error.GetType().ToString() == "System.Threading.ThreadAbortException")
          {
            return;
          }
          System.Windows.Forms.MessageBox.Show(error.ToString());
        }
      }
    }

    private void cleanMessages(object state)
    {
      this.storages.MessagesStack.Clear();
    }

    private void filterMessages(object state)
    {
      try
      {
        while (!this.storages.MessagesStack.IsEmpty)
        {
          string currentMessage;
          if(messagesStack.TryPop(out currentMessage))
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

    private byte[] readBytes()
    {
      byte[] buffer = new byte[512];
      this.sendDataPipeServer.Read(buffer, 0, buffer.Length);
      return buffer;
    }

  }
}
