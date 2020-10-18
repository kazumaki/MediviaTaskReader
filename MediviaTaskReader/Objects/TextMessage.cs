using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediviaTaskReader.Objects
{
  public class TextMessage
  {
    private string message;
    private UInt64 type;
    public TextMessage(string message, UInt64 type)
    {
      this.message = message;
      this.type = type;
    }

    public string Message
    {
      get { return this.message; }
      private set { }
    }

    public UInt64 Type
    {
      get { return this.type; }
      private set { }
    }

  }
}
