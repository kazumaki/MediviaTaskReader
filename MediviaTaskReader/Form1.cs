using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MediviaTaskReader.APICall;

namespace MediviaTaskReader
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Button1_Click(object sender, EventArgs e)
    {
      Process test = Process.GetProcessesByName("Medivia")[0];
      UInt64 baseAddress = Convert.ToUInt64(test.MainModule.BaseAddress.ToInt64());
      UInt64 nameBaseAddress = baseAddress + 0xD43950;
      UInt64 nameLengthAddress = Memory.ReadUInt64(test.Handle, nameBaseAddress) + 0x40;
      Int32 nameLength = Memory.ReadInt32(test.Handle, nameLengthAddress);
      if(nameLength > 16)
      {
        UInt64 nameAddress = Memory.ReadUInt64(test.Handle, Memory.ReadUInt64(test.Handle, nameBaseAddress) + 0x30);
        MessageBox.Show(Memory.ReadString(test.Handle, nameAddress));
      } else
      {

      }
      MessageBox.Show(Memory.ReadInt32(test.Handle, nameLengthAddress).ToString());
    }
  }
}
