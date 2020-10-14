using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using MediviaTaskReader.Objects;

namespace MediviaTaskReader.IPC
{
  public class MainLayer
  {
    private bool isConnected;
    private NamedPipeServerStream namedPipeServer;
    public MainLayer()
    {
      this.isConnected = false;
    }

    public void Connect(Character character)
    {
      this.namedPipeServer = new NamedPipeServerStream("MediviaTaskReader", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
      this.namedPipeServer.WaitForConnection();
    }

    public void Disconnect()
    {
      if(!this.isConnected)
      {
        return;
      }
    }

  }
}
