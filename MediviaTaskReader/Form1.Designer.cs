namespace MediviaTaskReader
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.buttonClientRefresh = new System.Windows.Forms.Button();
      this.clientCharacterListBox = new System.Windows.Forms.ListBox();
      this.buttonClientSelect = new System.Windows.Forms.Button();
      this.tasksCheckedListBox = new System.Windows.Forms.CheckedListBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.buttonTaskAdd = new System.Windows.Forms.Button();
      this.buttonTaskRemove = new System.Windows.Forms.Button();
      this.textTaskCreatureName = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.buttonClientExit = new System.Windows.Forms.Button();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.buttonTaskClear = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonClientRefresh
      // 
      this.buttonClientRefresh.Location = new System.Drawing.Point(29, 185);
      this.buttonClientRefresh.Name = "buttonClientRefresh";
      this.buttonClientRefresh.Size = new System.Drawing.Size(75, 23);
      this.buttonClientRefresh.TabIndex = 0;
      this.buttonClientRefresh.Text = "Refresh";
      this.buttonClientRefresh.UseVisualStyleBackColor = true;
      this.buttonClientRefresh.Click += new System.EventHandler(this.ButtonClientRefresh_Click);
      // 
      // clientCharacterListBox
      // 
      this.clientCharacterListBox.FormattingEnabled = true;
      this.clientCharacterListBox.Location = new System.Drawing.Point(29, 16);
      this.clientCharacterListBox.Name = "clientCharacterListBox";
      this.clientCharacterListBox.Size = new System.Drawing.Size(259, 134);
      this.clientCharacterListBox.TabIndex = 1;
      // 
      // buttonClientSelect
      // 
      this.buttonClientSelect.Location = new System.Drawing.Point(123, 185);
      this.buttonClientSelect.Name = "buttonClientSelect";
      this.buttonClientSelect.Size = new System.Drawing.Size(75, 23);
      this.buttonClientSelect.TabIndex = 2;
      this.buttonClientSelect.Text = "Select";
      this.buttonClientSelect.UseVisualStyleBackColor = true;
      this.buttonClientSelect.Click += new System.EventHandler(this.ButtonClientSelect_Click);
      // 
      // tasksCheckedListBox
      // 
      this.tasksCheckedListBox.FormattingEnabled = true;
      this.tasksCheckedListBox.Location = new System.Drawing.Point(18, 17);
      this.tasksCheckedListBox.Name = "tasksCheckedListBox";
      this.tasksCheckedListBox.Size = new System.Drawing.Size(229, 94);
      this.tasksCheckedListBox.TabIndex = 6;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.buttonTaskClear);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.tasksCheckedListBox);
      this.groupBox1.Controls.Add(this.buttonTaskAdd);
      this.groupBox1.Controls.Add(this.buttonTaskRemove);
      this.groupBox1.Controls.Add(this.textTaskCreatureName);
      this.groupBox1.Location = new System.Drawing.Point(351, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(264, 214);
      this.groupBox1.TabIndex = 7;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Tasks";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(15, 153);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(81, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Creature Name:";
      // 
      // buttonTaskAdd
      // 
      this.buttonTaskAdd.Location = new System.Drawing.Point(84, 185);
      this.buttonTaskAdd.Name = "buttonTaskAdd";
      this.buttonTaskAdd.Size = new System.Drawing.Size(75, 23);
      this.buttonTaskAdd.TabIndex = 9;
      this.buttonTaskAdd.Text = "Add";
      this.buttonTaskAdd.UseVisualStyleBackColor = true;
      this.buttonTaskAdd.Click += new System.EventHandler(this.ButtonTaskAdd_Click);
      // 
      // buttonTaskRemove
      // 
      this.buttonTaskRemove.Location = new System.Drawing.Point(18, 117);
      this.buttonTaskRemove.Name = "buttonTaskRemove";
      this.buttonTaskRemove.Size = new System.Drawing.Size(105, 23);
      this.buttonTaskRemove.TabIndex = 10;
      this.buttonTaskRemove.Text = "Remove";
      this.buttonTaskRemove.UseVisualStyleBackColor = true;
      this.buttonTaskRemove.Click += new System.EventHandler(this.ButtonTaskRemove_Click);
      // 
      // textTaskCreatureName
      // 
      this.textTaskCreatureName.Location = new System.Drawing.Point(102, 150);
      this.textTaskCreatureName.Name = "textTaskCreatureName";
      this.textTaskCreatureName.Size = new System.Drawing.Size(145, 20);
      this.textTaskCreatureName.TabIndex = 7;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.buttonClientExit);
      this.groupBox2.Controls.Add(this.clientCharacterListBox);
      this.groupBox2.Controls.Add(this.buttonClientRefresh);
      this.groupBox2.Controls.Add(this.buttonClientSelect);
      this.groupBox2.Location = new System.Drawing.Point(12, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(322, 214);
      this.groupBox2.TabIndex = 8;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Client Selector";
      // 
      // buttonClientExit
      // 
      this.buttonClientExit.Location = new System.Drawing.Point(213, 185);
      this.buttonClientExit.Name = "buttonClientExit";
      this.buttonClientExit.Size = new System.Drawing.Size(75, 23);
      this.buttonClientExit.TabIndex = 3;
      this.buttonClientExit.Text = "Exit";
      this.buttonClientExit.UseVisualStyleBackColor = true;
      this.buttonClientExit.Click += new System.EventHandler(this.ButtonClientExit_Click);
      // 
      // buttonTaskClear
      // 
      this.buttonTaskClear.Location = new System.Drawing.Point(142, 117);
      this.buttonTaskClear.Name = "buttonTaskClear";
      this.buttonTaskClear.Size = new System.Drawing.Size(105, 23);
      this.buttonTaskClear.TabIndex = 11;
      this.buttonTaskClear.Text = "Clear";
      this.buttonTaskClear.UseVisualStyleBackColor = true;
      this.buttonTaskClear.Click += new System.EventHandler(this.ButtonTaskClear_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(627, 237);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "Form1";
      this.Text = "MediviaTaskReader";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonClientRefresh;
    private System.Windows.Forms.Button buttonClientSelect;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button buttonClientExit;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonTaskAdd;
    private System.Windows.Forms.Button buttonTaskRemove;
    public System.Windows.Forms.TextBox textTaskCreatureName;
    public System.Windows.Forms.CheckedListBox tasksCheckedListBox;
    private System.Windows.Forms.Button buttonTaskClear;
    public System.Windows.Forms.ListBox clientCharacterListBox;
  }
}

