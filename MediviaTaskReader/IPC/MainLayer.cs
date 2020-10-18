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
          UInt64 type = BitConverter.ToUInt64(this.readBytes(), 0);
          string message = Encoding.UTF8.GetString(this.readBytes()).Trim('\0');
          this.storages.MessagesStack.Push(new TextMessage(message, type));
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

    private byte[] readBytes()
    {
      byte[] buffer = new byte[512];
      this.sendDataPipeServer.Read(buffer, 0, buffer.Length);
      return buffer;
    }

  }
}
