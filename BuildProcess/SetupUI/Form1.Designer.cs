namespace SetupUI
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.BtnTestDatabase = new System.Windows.Forms.Button();
      this.TxtDataSource = new System.Windows.Forms.TextBox();
      this.TxtSaPassword = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.BtnMSBuild = new System.Windows.Forms.Button();
      this.BtnMSTest = new System.Windows.Forms.Button();
      this.TxtMsBuild = new System.Windows.Forms.TextBox();
      this.TxtMsTest = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.BtnFinalCheck = new System.Windows.Forms.Button();
      this.BtnReset = new System.Windows.Forms.Button();
      this.BtnCancel = new System.Windows.Forms.Button();
      this.label6 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.BtnTestDatabase);
      this.groupBox1.Controls.Add(this.TxtDataSource);
      this.groupBox1.Controls.Add(this.TxtSaPassword);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Location = new System.Drawing.Point(10, 50);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(656, 100);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Data Source";
      // 
      // BtnTestDatabase
      // 
      this.BtnTestDatabase.Location = new System.Drawing.Point(290, 56);
      this.BtnTestDatabase.Name = "BtnTestDatabase";
      this.BtnTestDatabase.Size = new System.Drawing.Size(75, 23);
      this.BtnTestDatabase.TabIndex = 5;
      this.BtnTestDatabase.Text = "Test Connection";
      this.BtnTestDatabase.UseVisualStyleBackColor = true;
      this.BtnTestDatabase.Click += new System.EventHandler(this.BtnTestDatabase_Click);
      // 
      // TxtDataSource
      // 
      this.TxtDataSource.Location = new System.Drawing.Point(119, 56);
      this.TxtDataSource.Name = "TxtDataSource";
      this.TxtDataSource.Size = new System.Drawing.Size(134, 20);
      this.TxtDataSource.TabIndex = 4;
      // 
      // TxtSaPassword
      // 
      this.TxtSaPassword.Location = new System.Drawing.Point(119, 30);
      this.TxtSaPassword.Name = "TxtSaPassword";
      this.TxtSaPassword.Size = new System.Drawing.Size(134, 20);
      this.TxtSaPassword.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 13);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(460, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "The MassDataHandler sample creates its own test database, and therefore needs a c" +
          "onnection.";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 59);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(62, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Datasource";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 33);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(70, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "SA Password";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.BtnMSBuild);
      this.groupBox2.Controls.Add(this.BtnMSTest);
      this.groupBox2.Controls.Add(this.TxtMsBuild);
      this.groupBox2.Controls.Add(this.TxtMsTest);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Location = new System.Drawing.Point(10, 167);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(656, 100);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "MS Tools";
      // 
      // BtnMSBuild
      // 
      this.BtnMSBuild.Location = new System.Drawing.Point(573, 59);
      this.BtnMSBuild.Name = "BtnMSBuild";
      this.BtnMSBuild.Size = new System.Drawing.Size(75, 23);
      this.BtnMSBuild.TabIndex = 5;
      this.BtnMSBuild.Text = "Test";
      this.BtnMSBuild.UseVisualStyleBackColor = true;
      this.BtnMSBuild.Click += new System.EventHandler(this.BtnMSBuild_Click);
      // 
      // BtnMSTest
      // 
      this.BtnMSTest.Location = new System.Drawing.Point(573, 32);
      this.BtnMSTest.Name = "BtnMSTest";
      this.BtnMSTest.Size = new System.Drawing.Size(75, 23);
      this.BtnMSTest.TabIndex = 4;
      this.BtnMSTest.Text = "Test";
      this.BtnMSTest.UseVisualStyleBackColor = true;
      this.BtnMSTest.Click += new System.EventHandler(this.BtnMSTest_Click);
      // 
      // TxtMsBuild
      // 
      this.TxtMsBuild.Location = new System.Drawing.Point(119, 59);
      this.TxtMsBuild.Name = "TxtMsBuild";
      this.TxtMsBuild.Size = new System.Drawing.Size(447, 20);
      this.TxtMsBuild.TabIndex = 3;
      // 
      // TxtMsTest
      // 
      this.TxtMsTest.Location = new System.Drawing.Point(119, 32);
      this.TxtMsTest.Name = "TxtMsTest";
      this.TxtMsTest.Size = new System.Drawing.Size(447, 20);
      this.TxtMsTest.TabIndex = 2;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 58);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(91, 13);
      this.label5.TabIndex = 1;
      this.label5.Text = "MSBuild Directory";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 32);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(89, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "MSTest Directory";
      // 
      // BtnFinalCheck
      // 
      this.BtnFinalCheck.Location = new System.Drawing.Point(10, 288);
      this.BtnFinalCheck.Name = "BtnFinalCheck";
      this.BtnFinalCheck.Size = new System.Drawing.Size(75, 23);
      this.BtnFinalCheck.TabIndex = 2;
      this.BtnFinalCheck.Text = "Done";
      this.BtnFinalCheck.UseVisualStyleBackColor = true;
      this.BtnFinalCheck.Click += new System.EventHandler(this.BtnFinalCheck_Click);
      // 
      // BtnReset
      // 
      this.BtnReset.Location = new System.Drawing.Point(91, 288);
      this.BtnReset.Name = "BtnReset";
      this.BtnReset.Size = new System.Drawing.Size(75, 23);
      this.BtnReset.TabIndex = 3;
      this.BtnReset.Text = "Reset";
      this.BtnReset.UseVisualStyleBackColor = true;
      this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
      // 
      // BtnCancel
      // 
      this.BtnCancel.Location = new System.Drawing.Point(173, 288);
      this.BtnCancel.Name = "BtnCancel";
      this.BtnCancel.Size = new System.Drawing.Size(75, 23);
      this.BtnCancel.TabIndex = 4;
      this.BtnCancel.Text = "Cancel";
      this.BtnCancel.UseVisualStyleBackColor = true;
      this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.Location = new System.Drawing.Point(7, 9);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(677, 26);
      this.label6.TabIndex = 5;
      this.label6.Text = resources.GetString("label6.Text");
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(684, 319);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.BtnCancel);
      this.Controls.Add(this.BtnReset);
      this.Controls.Add(this.BtnFinalCheck);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Name = "Form1";
      this.Text = "Collect Inputs";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button BtnTestDatabase;
    private System.Windows.Forms.TextBox TxtDataSource;
    private System.Windows.Forms.TextBox TxtSaPassword;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button BtnMSBuild;
    private System.Windows.Forms.Button BtnMSTest;
    private System.Windows.Forms.TextBox TxtMsBuild;
    private System.Windows.Forms.TextBox TxtMsTest;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button BtnFinalCheck;
    private System.Windows.Forms.Button BtnReset;
    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.Label label6;
  }
}

