using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    public MainLayer()
    {
      this.isConnected = false;
    }

    public void Connect(Character character)
    {
      if(this.isConnected)
      {
        return;
      }

      MessageBox.Show(DLLInjector.Inject(character.Handle(), @Directory.GetCurrentDirectory() + @"\taskreader.dll").ToString());
      this.namedPipeServer = new NamedPipeServerStream("MediviaTaskReader", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
      this.namedPipeServer.WaitForConnection();
      MessageBox.Show("Conectado");
    }

    public void Disconnect()
    {
      if(!this.isConnected)
      {
        return;
      }

      this.namedPipeServer.Disconnect();
    }

  }
}
