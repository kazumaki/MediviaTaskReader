using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MediviaTaskReader.APICall;
using System.Windows.Forms;

namespace MediviaTaskReader.Objects
{
  public class Character
  {
    const Byte nameLengthOffset = 0x40;
    const Byte nameOffset = 0x30;
    const Int32 nameBaseOffset = 0xD43950;

    private IntPtr handle;
    private UInt64 baseAddress;
    public Character(Process process)
    {
      this.handle = process.Handle;
      this.baseAddress = Convert.ToUInt64(process.MainModule.BaseAddress.ToInt64());
    }

    public string Name()
    {
      UInt64 localBase = Memory.ReadUInt64(this.handle, this.baseAddress + nameBaseOffset);
      Int32 nameLength = Memory.ReadInt32(this.handle, localBase + nameLengthOffset);
      MessageBox.Show(nameLength.ToString());
      if(nameLength > 16)
      {
        UInt64 nameAddress = Memory.ReadUInt64(this.handle, localBase + nameOffset);
        return Memory.ReadString(this.handle, nameAddress);
      }

      return Memory.ReadString(this.handle, localBase + nameOffset);
    }

  }
}
