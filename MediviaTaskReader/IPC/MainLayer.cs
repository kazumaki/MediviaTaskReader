using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    private System.Threading.Timer timer;
    private System.Threading.TimerCallback timerCallback;
    public MainLayer()
    {
      this.isConnected = false;
      this.messagesStack = new ConcurrentStack<string>();
      this.timerCallback = this.cleanMessagesStack;
      this.timer = new System.Threading.Timer(this.timerCallback, "test", 1000, 10000);
      this.processMessagesThread = new Thread(() => this.processMessagesMethod())
      {
        IsBackground = true
      };
    }

    public void Connect(Character character)
    {
      if(this.isConnected)
      {
        return;
      }

      DLLInjector.Inject(character.Handle(), @Directory.GetCurrentDirectory() + @"\taskreader.dll");
      this.namedPipeServer = new NamedPipeServerStream("MediviaTaskReader", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

      this.processMessagesThread.Start();
    }

    public void Disconnect()
    {
      if(!this.isConnected)
      {
        return;
      }

      this.namedPipeServer.Disconnect();
    }

    private void cleanMessagesStack(Object state)
    {
      string currentMessage = "";
      while(this.messagesStack.Count != 0)
      {
        messagesStack.TryPop(out currentMessage);
        if (currentMessage.Contains("defeated"))
        {
          this.messagesStack.Clear();
          break;
        }
      }

      MessageBox.Show(currentMessage);
    }

    private void processMessagesMethod()
    {
      this.namedPipeServer.WaitForConnection();
      MessageBox.Show("Conectado");
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

    private byte[] readBytes()
    {
      byte[] buffer = new byte[512];
      this.namedPipeServer.Read(buffer, 0, buffer.Length);
      return buffer;
    }

  }
}
