using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MediviaTaskReader.IPC;

namespace MediviaTaskReader.Objects
{
  public class Client
  {
    Form1 mainForm;
    bool isConnected;
    MainLayer mainLayerIPC;

    public Client(Form1 mainForm)
    {
      this.mainForm = mainForm;
      this.isConnected = false;
      this.mainLayerIPC = new MainLayer();
      this.getClients();
    }

    public void AddTask()
    {
      string creatureName = this.mainForm.textTaskCreatureName.Text;
      CreatureTask newTask = new CreatureTask(creatureName, this.mainLayerIPC);
      this.mainForm.tasksCheckedListBox.Items.Add(creatureName);
    }

    public void RemoveTask()
    {
      int selectedIndex = this.mainForm.tasksCheckedListBox.SelectedIndex;
      int count = this.mainForm.tasksCheckedListBox.Items.Count;

      if (selectedIndex >= 0)
      {
        this.mainForm.tasksCheckedListBox.Items.RemoveAt(selectedIndex);

        if(selectedIndex < count - 1)
        {
          this.mainForm.tasksCheckedListBox.SelectedIndex = selectedIndex;
        }
      }
      else
        this.displayError("No selected Task");
    }

    public void ClearTask()
    {
      this.mainForm.tasksCheckedListBox.Items.Clear();
    }

    public void RefreshCharacters()
    {
      this.mainForm.clientCharacterListBox.Items.Clear();
      this.getClients();
    }

    public void SelectClient()
    {
      int selectedIndex = this.mainForm.clientCharacterListBox.SelectedIndex;

      if (selectedIndex >= 0)
      {

      }
      else
        this.displayError("No character selected!");
    }

    private void getClients()
    {
      Process[] processList = Process.GetProcessesByName("Medivia");
      foreach(Process p in processList)
      {
        this.mainForm.clientCharacterListBox.Items.Add(new Character(p));
      }
    }

    private void displayError(string error, string title = "Error")
    {
      System.Windows.Forms.MessageBox.Show(error, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    }
  }
}
