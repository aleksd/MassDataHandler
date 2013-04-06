namespace MassDataHandler.Gui
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
      this.components = new System.ComponentModel.Container();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.BtnBrowse = new System.Windows.Forms.Button();
      this.TxtImportDirectory = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.conStringTesterCreateInserts = new MassDataHandler.Gui.ConStringTester();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.TabgCreateInserts = new System.Windows.Forms.TabControl();
      this.tabPage4 = new System.Windows.Forms.TabPage();
      this.chkInsertIdentityOn = new System.Windows.Forms.CheckBox();
      this.ChkInsertLiveTableSchema = new System.Windows.Forms.CheckBox();
      this.BtnApplyImports = new System.Windows.Forms.Button();
      this.BtnDoInsertPaste = new System.Windows.Forms.Button();
      this.TxtXmlScript = new System.Windows.Forms.TextBox();
      this.BtnDoInsert = new System.Windows.Forms.Button();
      this.tabPage5 = new System.Windows.Forms.TabPage();
      this.TxtResultSql = new System.Windows.Forms.TextBox();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.ChkIncludeIdentityColumn = new System.Windows.Forms.CheckBox();
      this.ChkEnsureOneRow = new System.Windows.Forms.CheckBox();
      this.ChkFormatAsCSharp = new System.Windows.Forms.CheckBox();
      this.BtnGetSpSelects = new System.Windows.Forms.Button();
      this.BtnGetExisting = new System.Windows.Forms.Button();
      this.TxtExistingOutput = new System.Windows.Forms.TextBox();
      this.existingDataFilter1 = new MassDataHandler.Gui.ExistingDataFilter();
      this.tabCodeGen = new System.Windows.Forms.TabPage();
      this.TxtCodeGenOutput = new System.Windows.Forms.TextBox();
      this.BtnCodeGenerate = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.TxtCodeGenNamespace = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.TxtCodeGenObjectName = new System.Windows.Forms.TextBox();
      this.TxtCodeGenClassName = new System.Windows.Forms.TextBox();
      this.TxtCodeGenAssembly = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tabEscStr = new System.Windows.Forms.TabPage();
      this.panel1 = new System.Windows.Forms.Panel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.TxtEscapedInitial = new System.Windows.Forms.TextBox();
      this.TxtEscapedFinal = new System.Windows.Forms.TextBox();
      this.BtnConvert = new System.Windows.Forms.Button();
      this.DdConversionOptions = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.restoreInitialSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.groupBox1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.TabgCreateInserts.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tabPage5.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabCodeGen.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabEscStr.SuspendLayout();
      this.panel1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.BtnBrowse);
      this.groupBox1.Controls.Add(this.TxtImportDirectory);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.conStringTesterCreateInserts);
      this.groupBox1.Location = new System.Drawing.Point(8, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(693, 84);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "General";
      // 
      // BtnBrowse
      // 
      this.BtnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.BtnBrowse.Location = new System.Drawing.Point(599, 49);
      this.BtnBrowse.Name = "BtnBrowse";
      this.BtnBrowse.Size = new System.Drawing.Size(75, 23);
      this.BtnBrowse.TabIndex = 3;
      this.BtnBrowse.Text = "Browse";
      this.BtnBrowse.UseVisualStyleBackColor = true;
      this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
      // 
      // TxtImportDirectory
      // 
      this.TxtImportDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtImportDirectory.Location = new System.Drawing.Point(80, 49);
      this.TxtImportDirectory.Name = "TxtImportDirectory";
      this.TxtImportDirectory.Size = new System.Drawing.Size(512, 20);
      this.TxtImportDirectory.TabIndex = 2;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(11, 49);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 13);
      this.label5.TabIndex = 1;
      this.label5.Text = "Import Path";
      this.toolTip1.SetToolTip(this.label5, "(Optional) The root directory to base any <Import> commands from.");
      // 
      // conStringTesterCreateInserts
      // 
      this.conStringTesterCreateInserts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.conStringTesterCreateInserts.ConnectionReadOnly = false;
      this.conStringTesterCreateInserts.ConnectionString = "";
      this.conStringTesterCreateInserts.Location = new System.Drawing.Point(4, 19);
      this.conStringTesterCreateInserts.Name = "conStringTesterCreateInserts";
      this.conStringTesterCreateInserts.Size = new System.Drawing.Size(680, 30);
      this.conStringTesterCreateInserts.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Controls.Add(this.tabCodeGen);
      this.tabControl1.Controls.Add(this.tabEscStr);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 24);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(715, 581);
      this.tabControl1.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.TabgCreateInserts);
      this.tabPage1.Controls.Add(this.groupBox1);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(707, 555);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Create Inserts";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // TabgCreateInserts
      // 
      this.TabgCreateInserts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TabgCreateInserts.Controls.Add(this.tabPage4);
      this.TabgCreateInserts.Controls.Add(this.tabPage5);
      this.TabgCreateInserts.Location = new System.Drawing.Point(8, 96);
      this.TabgCreateInserts.Name = "TabgCreateInserts";
      this.TabgCreateInserts.SelectedIndex = 0;
      this.TabgCreateInserts.Size = new System.Drawing.Size(693, 453);
      this.TabgCreateInserts.TabIndex = 3;
      // 
      // tabPage4
      // 
      this.tabPage4.Controls.Add(this.chkInsertIdentityOn);
      this.tabPage4.Controls.Add(this.ChkInsertLiveTableSchema);
      this.tabPage4.Controls.Add(this.BtnApplyImports);
      this.tabPage4.Controls.Add(this.BtnDoInsertPaste);
      this.tabPage4.Controls.Add(this.TxtXmlScript);
      this.tabPage4.Controls.Add(this.BtnDoInsert);
      this.tabPage4.Location = new System.Drawing.Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage4.Size = new System.Drawing.Size(685, 427);
      this.tabPage4.TabIndex = 0;
      this.tabPage4.Text = "Xml Insert File";
      this.tabPage4.UseVisualStyleBackColor = true;
      // 
      // chkInsertIdentityOn
      // 
      this.chkInsertIdentityOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.chkInsertIdentityOn.AutoSize = true;
      this.chkInsertIdentityOn.Location = new System.Drawing.Point(286, 409);
      this.chkInsertIdentityOn.Name = "chkInsertIdentityOn";
      this.chkInsertIdentityOn.Size = new System.Drawing.Size(122, 17);
      this.chkInsertIdentityOn.TabIndex = 5;
      this.chkInsertIdentityOn.Text = "Allow Identity Inserts";
      this.toolTip1.SetToolTip(this.chkInsertIdentityOn, "True = wrap each insert with \"SET IDENTITY_INSERT XService ON\".");
      this.chkInsertIdentityOn.UseVisualStyleBackColor = true;
      // 
      // ChkInsertLiveTableSchema
      // 
      this.ChkInsertLiveTableSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ChkInsertLiveTableSchema.AutoSize = true;
      this.ChkInsertLiveTableSchema.Checked = true;
      this.ChkInsertLiveTableSchema.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ChkInsertLiveTableSchema.Location = new System.Drawing.Point(286, 394);
      this.ChkInsertLiveTableSchema.Name = "ChkInsertLiveTableSchema";
      this.ChkInsertLiveTableSchema.Size = new System.Drawing.Size(290, 17);
      this.ChkInsertLiveTableSchema.TabIndex = 4;
      this.ChkInsertLiveTableSchema.Text = "Use live database for Table Schema (slow performance)";
      this.ChkInsertLiveTableSchema.UseVisualStyleBackColor = true;
      // 
      // BtnApplyImports
      // 
      this.BtnApplyImports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.BtnApplyImports.Location = new System.Drawing.Point(192, 398);
      this.BtnApplyImports.Name = "BtnApplyImports";
      this.BtnApplyImports.Size = new System.Drawing.Size(79, 23);
      this.BtnApplyImports.TabIndex = 3;
      this.BtnApplyImports.Text = "Apply Imports";
      this.toolTip1.SetToolTip(this.BtnApplyImports, "Show what the script looks like if all <Import> commands are applied.");
      this.BtnApplyImports.UseVisualStyleBackColor = true;
      this.BtnApplyImports.Click += new System.EventHandler(this.BtnApplyImports_Click);
      // 
      // BtnDoInsertPaste
      // 
      this.BtnDoInsertPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.BtnDoInsertPaste.Location = new System.Drawing.Point(88, 398);
      this.BtnDoInsertPaste.Name = "BtnDoInsertPaste";
      this.BtnDoInsertPaste.Size = new System.Drawing.Size(98, 23);
      this.BtnDoInsertPaste.TabIndex = 2;
      this.BtnDoInsertPaste.Text = "Paste then Insert";
      this.toolTip1.SetToolTip(this.BtnDoInsertPaste, "Paste from the Clipboard and insert.");
      this.BtnDoInsertPaste.UseVisualStyleBackColor = true;
      this.BtnDoInsertPaste.Click += new System.EventHandler(this.BtnDoInsertPaste_Click);
      // 
      // TxtXmlScript
      // 
      this.TxtXmlScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtXmlScript.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtXmlScript.Location = new System.Drawing.Point(3, 3);
      this.TxtXmlScript.MaxLength = 327670;
      this.TxtXmlScript.Multiline = true;
      this.TxtXmlScript.Name = "TxtXmlScript";
      this.TxtXmlScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtXmlScript.Size = new System.Drawing.Size(679, 389);
      this.TxtXmlScript.TabIndex = 1;
      this.TxtXmlScript.WordWrap = false;
      // 
      // BtnDoInsert
      // 
      this.BtnDoInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.BtnDoInsert.Location = new System.Drawing.Point(6, 398);
      this.BtnDoInsert.Name = "BtnDoInsert";
      this.BtnDoInsert.Size = new System.Drawing.Size(75, 23);
      this.BtnDoInsert.TabIndex = 0;
      this.BtnDoInsert.Text = "Insert Now";
      this.BtnDoInsert.UseVisualStyleBackColor = true;
      this.BtnDoInsert.Click += new System.EventHandler(this.BtnDoInsert_Click);
      // 
      // tabPage5
      // 
      this.tabPage5.Controls.Add(this.TxtResultSql);
      this.tabPage5.Location = new System.Drawing.Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage5.Size = new System.Drawing.Size(685, 427);
      this.tabPage5.TabIndex = 1;
      this.tabPage5.Text = "Resulting Sql Inserts";
      this.tabPage5.UseVisualStyleBackColor = true;
      // 
      // TxtResultSql
      // 
      this.TxtResultSql.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TxtResultSql.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtResultSql.Location = new System.Drawing.Point(3, 3);
      this.TxtResultSql.MaxLength = 327670;
      this.TxtResultSql.Multiline = true;
      this.TxtResultSql.Name = "TxtResultSql";
      this.TxtResultSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtResultSql.Size = new System.Drawing.Size(679, 421);
      this.TxtResultSql.TabIndex = 5;
      this.TxtResultSql.WordWrap = false;
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.ChkIncludeIdentityColumn);
      this.tabPage3.Controls.Add(this.ChkEnsureOneRow);
      this.tabPage3.Controls.Add(this.ChkFormatAsCSharp);
      this.tabPage3.Controls.Add(this.BtnGetSpSelects);
      this.tabPage3.Controls.Add(this.BtnGetExisting);
      this.tabPage3.Controls.Add(this.TxtExistingOutput);
      this.tabPage3.Controls.Add(this.existingDataFilter1);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new System.Drawing.Size(707, 555);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Use Existing Data";
      this.tabPage3.UseVisualStyleBackColor = true;
      // 
      // ChkIncludeIdentityColumn
      // 
      this.ChkIncludeIdentityColumn.AutoSize = true;
      this.ChkIncludeIdentityColumn.Location = new System.Drawing.Point(369, 290);
      this.ChkIncludeIdentityColumn.Name = "ChkIncludeIdentityColumn";
      this.ChkIncludeIdentityColumn.Size = new System.Drawing.Size(136, 17);
      this.ChkIncludeIdentityColumn.TabIndex = 12;
      this.ChkIncludeIdentityColumn.Text = "Include Identity Column";
      this.toolTip1.SetToolTip(this.ChkIncludeIdentityColumn, "True to include the Identity (normally the Primary Key) column, else False.");
      this.ChkIncludeIdentityColumn.UseVisualStyleBackColor = true;
      // 
      // ChkEnsureOneRow
      // 
      this.ChkEnsureOneRow.AutoSize = true;
      this.ChkEnsureOneRow.Location = new System.Drawing.Point(237, 289);
      this.ChkEnsureOneRow.Name = "ChkEnsureOneRow";
      this.ChkEnsureOneRow.Size = new System.Drawing.Size(125, 17);
      this.ChkEnsureOneRow.TabIndex = 11;
      this.ChkEnsureOneRow.Text = "Ensure at least 1 row";
      this.ChkEnsureOneRow.UseVisualStyleBackColor = true;
      // 
      // ChkFormatAsCSharp
      // 
      this.ChkFormatAsCSharp.AutoSize = true;
      this.ChkFormatAsCSharp.Location = new System.Drawing.Point(102, 290);
      this.ChkFormatAsCSharp.Name = "ChkFormatAsCSharp";
      this.ChkFormatAsCSharp.Size = new System.Drawing.Size(117, 17);
      this.ChkFormatAsCSharp.TabIndex = 10;
      this.ChkFormatAsCSharp.Text = "Format as C# string";
      this.toolTip1.SetToolTip(this.ChkFormatAsCSharp, "True= Format as a literal string that you can dump into C# code.\r\nFalse= Format a" +
              "s a valid Xml file.");
      this.ChkFormatAsCSharp.UseVisualStyleBackColor = true;
      // 
      // BtnGetSpSelects
      // 
      this.BtnGetSpSelects.Location = new System.Drawing.Point(482, 69);
      this.BtnGetSpSelects.Name = "BtnGetSpSelects";
      this.BtnGetSpSelects.Size = new System.Drawing.Size(75, 23);
      this.BtnGetSpSelects.TabIndex = 9;
      this.BtnGetSpSelects.Text = "Get Selects";
      this.toolTip1.SetToolTip(this.BtnGetSpSelects, "Create select and delete Sql statements for every table in this object.");
      this.BtnGetSpSelects.UseVisualStyleBackColor = true;
      this.BtnGetSpSelects.Click += new System.EventHandler(this.BtnGetSpSelects_Click);
      // 
      // BtnGetExisting
      // 
      this.BtnGetExisting.Location = new System.Drawing.Point(8, 286);
      this.BtnGetExisting.Name = "BtnGetExisting";
      this.BtnGetExisting.Size = new System.Drawing.Size(75, 23);
      this.BtnGetExisting.TabIndex = 7;
      this.BtnGetExisting.Text = "Get Existing";
      this.toolTip1.SetToolTip(this.BtnGetExisting, "Get an Xml fragement based on existing data.");
      this.BtnGetExisting.UseVisualStyleBackColor = true;
      this.BtnGetExisting.Click += new System.EventHandler(this.BtnGetExisting_Click);
      // 
      // TxtExistingOutput
      // 
      this.TxtExistingOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtExistingOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtExistingOutput.Location = new System.Drawing.Point(8, 315);
      this.TxtExistingOutput.MaxLength = 327670;
      this.TxtExistingOutput.Multiline = true;
      this.TxtExistingOutput.Name = "TxtExistingOutput";
      this.TxtExistingOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtExistingOutput.Size = new System.Drawing.Size(691, 240);
      this.TxtExistingOutput.TabIndex = 6;
      this.TxtExistingOutput.WordWrap = false;
      // 
      // existingDataFilter1
      // 
      this.existingDataFilter1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.existingDataFilter1.ConnectionString = "";
      this.existingDataFilter1.Location = new System.Drawing.Point(3, 3);
      this.existingDataFilter1.Name = "existingDataFilter1";
      this.existingDataFilter1.Size = new System.Drawing.Size(696, 281);
      this.existingDataFilter1.TabIndex = 8;
      // 
      // tabCodeGen
      // 
      this.tabCodeGen.Controls.Add(this.TxtCodeGenOutput);
      this.tabCodeGen.Controls.Add(this.BtnCodeGenerate);
      this.tabCodeGen.Controls.Add(this.groupBox2);
      this.tabCodeGen.Location = new System.Drawing.Point(4, 22);
      this.tabCodeGen.Name = "tabCodeGen";
      this.tabCodeGen.Size = new System.Drawing.Size(707, 555);
      this.tabCodeGen.TabIndex = 3;
      this.tabCodeGen.Text = "Code Generation";
      this.tabCodeGen.UseVisualStyleBackColor = true;
      // 
      // TxtCodeGenOutput
      // 
      this.TxtCodeGenOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtCodeGenOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtCodeGenOutput.Location = new System.Drawing.Point(9, 169);
      this.TxtCodeGenOutput.Multiline = true;
      this.TxtCodeGenOutput.Name = "TxtCodeGenOutput";
      this.TxtCodeGenOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtCodeGenOutput.Size = new System.Drawing.Size(690, 397);
      this.TxtCodeGenOutput.TabIndex = 2;
      // 
      // BtnCodeGenerate
      // 
      this.BtnCodeGenerate.Location = new System.Drawing.Point(9, 140);
      this.BtnCodeGenerate.Name = "BtnCodeGenerate";
      this.BtnCodeGenerate.Size = new System.Drawing.Size(75, 23);
      this.BtnCodeGenerate.TabIndex = 1;
      this.BtnCodeGenerate.Text = "Generate";
      this.BtnCodeGenerate.UseVisualStyleBackColor = true;
      this.BtnCodeGenerate.Click += new System.EventHandler(this.BtnCodeGenerate_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.TxtCodeGenNamespace);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.TxtCodeGenObjectName);
      this.groupBox2.Controls.Add(this.TxtCodeGenClassName);
      this.groupBox2.Controls.Add(this.TxtCodeGenAssembly);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(9, 4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(690, 130);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Object Instantiation";
      // 
      // TxtCodeGenNamespace
      // 
      this.TxtCodeGenNamespace.Location = new System.Drawing.Point(130, 42);
      this.TxtCodeGenNamespace.Name = "TxtCodeGenNamespace";
      this.TxtCodeGenNamespace.Size = new System.Drawing.Size(240, 20);
      this.TxtCodeGenNamespace.TabIndex = 7;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 42);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(64, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Namespace";
      // 
      // TxtCodeGenObjectName
      // 
      this.TxtCodeGenObjectName.Location = new System.Drawing.Point(130, 94);
      this.TxtCodeGenObjectName.Name = "TxtCodeGenObjectName";
      this.TxtCodeGenObjectName.Size = new System.Drawing.Size(126, 20);
      this.TxtCodeGenObjectName.TabIndex = 5;
      // 
      // TxtCodeGenClassName
      // 
      this.TxtCodeGenClassName.Location = new System.Drawing.Point(130, 68);
      this.TxtCodeGenClassName.Name = "TxtCodeGenClassName";
      this.TxtCodeGenClassName.Size = new System.Drawing.Size(240, 20);
      this.TxtCodeGenClassName.TabIndex = 4;
      // 
      // TxtCodeGenAssembly
      // 
      this.TxtCodeGenAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtCodeGenAssembly.Location = new System.Drawing.Point(130, 16);
      this.TxtCodeGenAssembly.Name = "TxtCodeGenAssembly";
      this.TxtCodeGenAssembly.Size = new System.Drawing.Size(543, 20);
      this.TxtCodeGenAssembly.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 94);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(113, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Object Instance Name";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 68);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(63, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Class Name";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(76, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Assembly Path";
      // 
      // tabEscStr
      // 
      this.tabEscStr.Controls.Add(this.panel1);
      this.tabEscStr.Controls.Add(this.BtnConvert);
      this.tabEscStr.Controls.Add(this.DdConversionOptions);
      this.tabEscStr.Controls.Add(this.label6);
      this.tabEscStr.Location = new System.Drawing.Point(4, 22);
      this.tabEscStr.Name = "tabEscStr";
      this.tabEscStr.Size = new System.Drawing.Size(707, 555);
      this.tabEscStr.TabIndex = 4;
      this.tabEscStr.Text = "Escape String Conversion";
      this.tabEscStr.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.Controls.Add(this.splitContainer1);
      this.panel1.Location = new System.Drawing.Point(3, 55);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(701, 500);
      this.panel1.TabIndex = 6;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.TxtEscapedInitial);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.TxtEscapedFinal);
      this.splitContainer1.Size = new System.Drawing.Size(701, 500);
      this.splitContainer1.SplitterDistance = 314;
      this.splitContainer1.TabIndex = 0;
      // 
      // TxtEscapedInitial
      // 
      this.TxtEscapedInitial.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TxtEscapedInitial.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtEscapedInitial.Location = new System.Drawing.Point(0, 0);
      this.TxtEscapedInitial.Multiline = true;
      this.TxtEscapedInitial.Name = "TxtEscapedInitial";
      this.TxtEscapedInitial.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtEscapedInitial.Size = new System.Drawing.Size(314, 500);
      this.TxtEscapedInitial.TabIndex = 0;
      this.TxtEscapedInitial.WordWrap = false;
      // 
      // TxtEscapedFinal
      // 
      this.TxtEscapedFinal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TxtEscapedFinal.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TxtEscapedFinal.Location = new System.Drawing.Point(0, 0);
      this.TxtEscapedFinal.Multiline = true;
      this.TxtEscapedFinal.Name = "TxtEscapedFinal";
      this.TxtEscapedFinal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtEscapedFinal.Size = new System.Drawing.Size(383, 500);
      this.TxtEscapedFinal.TabIndex = 1;
      this.TxtEscapedFinal.WordWrap = false;
      // 
      // BtnConvert
      // 
      this.BtnConvert.Location = new System.Drawing.Point(193, 26);
      this.BtnConvert.Name = "BtnConvert";
      this.BtnConvert.Size = new System.Drawing.Size(75, 23);
      this.BtnConvert.TabIndex = 5;
      this.BtnConvert.Text = "Convert";
      this.BtnConvert.UseVisualStyleBackColor = true;
      this.BtnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
      // 
      // DdConversionOptions
      // 
      this.DdConversionOptions.FormattingEnabled = true;
      this.DdConversionOptions.Items.AddRange(new object[] {
            "C# Escaped string --> File Format",
            "File Format --> C# Escaped string"});
      this.DdConversionOptions.Location = new System.Drawing.Point(3, 26);
      this.DdConversionOptions.Name = "DdConversionOptions";
      this.DdConversionOptions.Size = new System.Drawing.Size(184, 21);
      this.DdConversionOptions.TabIndex = 3;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(0, 3);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(606, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "This tab converts from an Escaped C# literal string (declared like string s = @\"a" +
          "bc\"; ) to formatted text as you\'d see in a text file.";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
      this.statusStrip1.Location = new System.Drawing.Point(0, 605);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(715, 22);
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(715, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSettingsToolStripMenuItem,
            this.restoreInitialSettingsToolStripMenuItem});
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
      this.optionsToolStripMenuItem.Text = "Options";
      // 
      // saveSettingsToolStripMenuItem
      // 
      this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
      this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
      this.saveSettingsToolStripMenuItem.Text = "Save Settings";
      this.saveSettingsToolStripMenuItem.ToolTipText = "Settings are also automatically saved when you close the app.";
      this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
      // 
      // restoreInitialSettingsToolStripMenuItem
      // 
      this.restoreInitialSettingsToolStripMenuItem.Name = "restoreInitialSettingsToolStripMenuItem";
      this.restoreInitialSettingsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
      this.restoreInitialSettingsToolStripMenuItem.Text = "Restore Initial Settings";
      this.restoreInitialSettingsToolStripMenuItem.Click += new System.EventHandler(this.restoreInitialSettingsToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tutToolStripMenuItem,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // tutToolStripMenuItem
      // 
      this.tutToolStripMenuItem.Name = "tutToolStripMenuItem";
      this.tutToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
      this.tutToolStripMenuItem.Text = "MassDataHandler Website";
      this.tutToolStripMenuItem.Click += new System.EventHandler(this.tutToolStripMenuItem_Click);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
      this.aboutToolStripMenuItem.Text = "About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(715, 627);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.MinimumSize = new System.Drawing.Size(400, 300);
      this.Name = "Form1";
      this.Text = "MassDataHandler";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.TabgCreateInserts.ResumeLayout(false);
      this.tabPage4.ResumeLayout(false);
      this.tabPage4.PerformLayout();
      this.tabPage5.ResumeLayout(false);
      this.tabPage5.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      this.tabCodeGen.ResumeLayout(false);
      this.tabCodeGen.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabEscStr.ResumeLayout(false);
      this.tabEscStr.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TextBox TxtXmlScript;
    private System.Windows.Forms.Button BtnDoInsert;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.Button BtnGetExisting;
    private System.Windows.Forms.TextBox TxtExistingOutput;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private ConStringTester conStringTesterCreateInserts;
    private System.Windows.Forms.TabControl TabgCreateInserts;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.TabPage tabPage5;
    private System.Windows.Forms.TextBox TxtResultSql;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    private ExistingDataFilter existingDataFilter1;
    private System.Windows.Forms.Button BtnGetSpSelects;
    private System.Windows.Forms.Button BtnDoInsertPaste;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.CheckBox ChkFormatAsCSharp;
    private System.Windows.Forms.TabPage tabCodeGen;
    private System.Windows.Forms.TextBox TxtCodeGenOutput;
    private System.Windows.Forms.Button BtnCodeGenerate;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TextBox TxtCodeGenObjectName;
    private System.Windows.Forms.TextBox TxtCodeGenClassName;
    private System.Windows.Forms.TextBox TxtCodeGenAssembly;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TxtCodeGenNamespace;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox TxtImportDirectory;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button BtnBrowse;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private System.Windows.Forms.CheckBox ChkEnsureOneRow;
    private System.Windows.Forms.Button BtnApplyImports;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem restoreInitialSettingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
    private System.Windows.Forms.TabPage tabEscStr;
    private System.Windows.Forms.TextBox TxtEscapedFinal;
    private System.Windows.Forms.TextBox TxtEscapedInitial;
    private System.Windows.Forms.ComboBox DdConversionOptions;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button BtnConvert;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.CheckBox ChkInsertLiveTableSchema;
    private System.Windows.Forms.CheckBox chkInsertIdentityOn;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.CheckBox ChkIncludeIdentityColumn;
  }
}

