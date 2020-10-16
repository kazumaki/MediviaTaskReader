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
    private MainLayer mainLayer;
    private Client client;
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.client = new Client(this);
    }

    private void ButtonTaskAdd_Click(object sender, EventArgs e)
    {
      this.client.AddTask();
    }

    private void ButtonTaskRemove_Click(object sender, EventArgs e)
    {
      this.client.RemoveTask();
    }

    private void ButtonTaskClear_Click(object sender, EventArgs e)
    {
      this.client.ClearTask();
    }

    private void ButtonClientRefresh_Click(object sender, EventArgs e)
    {
      this.client.RefreshCharacters();
    }

    private void ButtonClientSelect_Click(object sender, EventArgs e)
    {
      this.client.SelectClient();
    }

    private void ButtonClientExit_Click(object sender, EventArgs e)
    {
      this.client.ExitClient();
    }
  }
}
