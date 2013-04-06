namespace MassDataHandler.Gui
{
  partial class ConStringTester
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.BtnTestCon = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.TxtConString = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // BtnTestCon
      // 
      this.BtnTestCon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnTestCon.Location = new System.Drawing.Point(524, 1);
      this.BtnTestCon.Name = "BtnTestCon";
      this.BtnTestCon.Size = new System.Drawing.Size(75, 23);
      this.BtnTestCon.TabIndex = 5;
      this.BtnTestCon.Text = "Test";
      this.BtnTestCon.UseVisualStyleBackColor = true;
      this.BtnTestCon.Click += new System.EventHandler(this.BtnTestCon_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(64, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Connection:";
      // 
      // TxtConString
      // 
      this.TxtConString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtConString.Location = new System.Drawing.Point(76, 3);
      this.TxtConString.Name = "TxtConString";
      this.TxtConString.Size = new System.Drawing.Size(442, 20);
      this.TxtConString.TabIndex = 3;
      // 
      // ConStringTester
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.BtnTestCon);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.TxtConString);
      this.Name = "ConStringTester";
      this.Size = new System.Drawing.Size(610, 30);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button BtnTestCon;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TxtConString;
  }
}

