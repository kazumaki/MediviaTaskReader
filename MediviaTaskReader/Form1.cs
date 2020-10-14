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
using MediviaTaskReader.Objects;
using MediviaTaskReader.IPC;

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
      listBox1.Items.Clear();
      foreach(Process p in Process.GetProcessesByName("Medivia"))
      {
        listBox1.Items.Add(new Character(p));
      }

    }

    private void Button2_Click(object sender, EventArgs e)
    {
      Character currentCharacter = (Character)listBox1.SelectedItem;
      MainLayer mainLayer = new MainLayer();
      mainLayer.Connect(currentCharacter);
    }
  }
}
