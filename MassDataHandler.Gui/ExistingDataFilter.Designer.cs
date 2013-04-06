namespace MassDataHandler.Gui
{
  partial class ExistingDataFilter
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
      this.components = new System.ComponentModel.Container();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.RdMainAdvanced = new System.Windows.Forms.RadioButton();
      this.RdMainBasic = new System.Windows.Forms.RadioButton();
      this.GrpAdvanced = new System.Windows.Forms.GroupBox();
      this.BtnGetColumns = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this.TxtCustomTable = new System.Windows.Forms.TextBox();
      this.TxtCustomSql = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.GrpBasic = new System.Windows.Forms.GroupBox();
      this.label6 = new System.Windows.Forms.Label();
      this.RdObjectTable = new System.Windows.Forms.RadioButton();
      this.RdObjectSP = new System.Windows.Forms.RadioButton();
      this.NumRowCount = new System.Windows.Forms.NumericUpDown();
      this.TxtSchemaObjectName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.conStringTester1 = new MassDataHandler.Gui.ConStringTester();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.groupBox1.SuspendLayout();
      this.GrpAdvanced.SuspendLayout();
      this.GrpBasic.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumRowCount)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.RdMainAdvanced);
      this.groupBox1.Controls.Add(this.RdMainBasic);
      this.groupBox1.Controls.Add(this.GrpAdvanced);
      this.groupBox1.Controls.Add(this.GrpBasic);
      this.groupBox1.Controls.Add(this.conStringTester1);
      this.groupBox1.Location = new System.Drawing.Point(3, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(634, 274);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Filter";
      // 
      // RdMainAdvanced
      // 
      this.RdMainAdvanced.AutoSize = true;
      this.RdMainAdvanced.Location = new System.Drawing.Point(7, 165);
      this.RdMainAdvanced.Name = "RdMainAdvanced";
      this.RdMainAdvanced.Size = new System.Drawing.Size(14, 13);
      this.RdMainAdvanced.TabIndex = 4;
      this.RdMainAdvanced.UseVisualStyleBackColor = true;
      this.RdMainAdvanced.CheckedChanged += new System.EventHandler(this.RdMainAdvanced_CheckedChanged);
      // 
      // RdMainBasic
      // 
      this.RdMainBasic.AutoSize = true;
      this.RdMainBasic.Checked = true;
      this.RdMainBasic.Location = new System.Drawing.Point(6, 46);
      this.RdMainBasic.Name = "RdMainBasic";
      this.RdMainBasic.Size = new System.Drawing.Size(14, 13);
      this.RdMainBasic.TabIndex = 3;
      this.RdMainBasic.TabStop = true;
      this.RdMainBasic.UseVisualStyleBackColor = true;
      this.RdMainBasic.CheckedChanged += new System.EventHandler(this.RdMainBasic_CheckedChanged);
      // 
      // GrpAdvanced
      // 
      this.GrpAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.GrpAdvanced.Controls.Add(this.BtnGetColumns);
      this.GrpAdvanced.Controls.Add(this.label7);
      this.GrpAdvanced.Controls.Add(this.TxtCustomTable);
      this.GrpAdvanced.Controls.Add(this.TxtCustomSql);
      this.GrpAdvanced.Controls.Add(this.label1);
      this.GrpAdvanced.Location = new System.Drawing.Point(26, 165);
      this.GrpAdvanced.Name = "GrpAdvanced";
      this.GrpAdvanced.Size = new System.Drawing.Size(590, 103);
      this.GrpAdvanced.TabIndex = 2;
      this.GrpAdvanced.TabStop = false;
      this.GrpAdvanced.Text = "Advanced Table";
      // 
      // BtnGetColumns
      // 
      this.BtnGetColumns.Location = new System.Drawing.Point(509, 77);
      this.BtnGetColumns.Name = "BtnGetColumns";
      this.BtnGetColumns.Size = new System.Drawing.Size(75, 23);
      this.BtnGetColumns.TabIndex = 4;
      this.BtnGetColumns.Text = "Get Columns";
      this.toolTip1.SetToolTip(this.BtnGetColumns, "Create a full Sql select statement (for all columns) from this table.");
      this.BtnGetColumns.UseVisualStyleBackColor = true;
      this.BtnGetColumns.Click += new System.EventHandler(this.BtnGetColumns_Click);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(1, 77);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(73, 13);
      this.label7.TabIndex = 3;
      this.label7.Text = "TableName(s)";
      this.toolTip1.SetToolTip(this.label7, "The table to appear in the Xml fragment\'s \'name\' property. You can specify multip" +
              "le tables as a CSV string.");
      // 
      // TxtCustomTable
      // 
      this.TxtCustomTable.Location = new System.Drawing.Point(74, 77);
      this.TxtCustomTable.Name = "TxtCustomTable";
      this.TxtCustomTable.Size = new System.Drawing.Size(429, 20);
      this.TxtCustomTable.TabIndex = 2;
      // 
      // TxtCustomSql
      // 
      this.TxtCustomSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.TxtCustomSql.Location = new System.Drawing.Point(74, 20);
      this.TxtCustomSql.Multiline = true;
      this.TxtCustomSql.Name = "TxtCustomSql";
      this.TxtCustomSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.TxtCustomSql.Size = new System.Drawing.Size(510, 51);
      this.TxtCustomSql.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(1, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(60, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Custom Sql";
      this.toolTip1.SetToolTip(this.label1, "Enter any Sql statement here, such as a multi-table join or exec SP call. You can" +
              " enter multiple SQL statements.");
      // 
      // GrpBasic
      // 
      this.GrpBasic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.GrpBasic.Controls.Add(this.label6);
      this.GrpBasic.Controls.Add(this.RdObjectTable);
      this.GrpBasic.Controls.Add(this.RdObjectSP);
      this.GrpBasic.Controls.Add(this.NumRowCount);
      this.GrpBasic.Controls.Add(this.TxtSchemaObjectName);
      this.GrpBasic.Controls.Add(this.label3);
      this.GrpBasic.Controls.Add(this.label2);
      this.GrpBasic.Location = new System.Drawing.Point(26, 46);
      this.GrpBasic.Name = "GrpBasic";
      this.GrpBasic.Size = new System.Drawing.Size(591, 107);
      this.GrpBasic.TabIndex = 1;
      this.GrpBasic.TabStop = false;
      this.GrpBasic.Text = "Basic";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(180, 50);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(147, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "(per Table; 0 = return all rows)";
      // 
      // RdObjectTable
      // 
      this.RdObjectTable.AutoSize = true;
      this.RdObjectTable.Checked = true;
      this.RdObjectTable.Location = new System.Drawing.Point(391, 19);
      this.RdObjectTable.Name = "RdObjectTable";
      this.RdObjectTable.Size = new System.Drawing.Size(52, 17);
      this.RdObjectTable.TabIndex = 9;
      this.RdObjectTable.TabStop = true;
      this.RdObjectTable.Text = "Table";
      this.toolTip1.SetToolTip(this.RdObjectTable, "A table, or CSV list of tables");
      this.RdObjectTable.UseVisualStyleBackColor = true;
      // 
      // RdObjectSP
      // 
      this.RdObjectSP.AutoSize = true;
      this.RdObjectSP.Location = new System.Drawing.Point(277, 20);
      this.RdObjectSP.Name = "RdObjectSP";
      this.RdObjectSP.Size = new System.Drawing.Size(108, 17);
      this.RdObjectSP.TabIndex = 8;
      this.RdObjectSP.Text = "Stored Procedure";
      this.toolTip1.SetToolTip(this.RdObjectSP, "A single stored procedure");
      this.RdObjectSP.UseVisualStyleBackColor = true;
      // 
      // NumRowCount
      // 
      this.NumRowCount.Location = new System.Drawing.Point(74, 50);
      this.NumRowCount.Name = "NumRowCount";
      this.NumRowCount.Size = new System.Drawing.Size(100, 20);
      this.NumRowCount.TabIndex = 7;
      this.NumRowCount.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
      // 
      // TxtSchemaObjectName
      // 
      this.TxtSchemaObjectName.Location = new System.Drawing.Point(74, 20);
      this.TxtSchemaObjectName.Name = "TxtSchemaObjectName";
      this.TxtSchemaObjectName.Size = new System.Drawing.Size(196, 20);
      this.TxtSchemaObjectName.TabIndex = 4;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(7, 52);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(67, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Top N Rows";
      this.toolTip1.SetToolTip(this.label3, "For each table, return the top N rows (leave empty or \'0\' to return all rows).");
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 26);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(38, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Object";
      this.toolTip1.SetToolTip(this.label2, "The Stored Procedure or Table to select data from.");
      // 
      // conStringTester1
      // 
      this.conStringTester1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.conStringTester1.ConnectionReadOnly = false;
      this.conStringTester1.ConnectionString = "";
      this.conStringTester1.Location = new System.Drawing.Point(6, 19);
      this.conStringTester1.Name = "conStringTester1";
      this.conStringTester1.Size = new System.Drawing.Size(621, 30);
      this.conStringTester1.TabIndex = 0;
      // 
      // ExistingDataFilter
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "ExistingDataFilter";
      this.Size = new System.Drawing.Size(640, 286);
      this.Load += new System.EventHandler(this.ExistingDataFilter_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.GrpAdvanced.ResumeLayout(false);
      this.GrpAdvanced.PerformLayout();
      this.GrpBasic.ResumeLayout(false);
      this.GrpBasic.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumRowCount)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton RdMainAdvanced;
    private System.Windows.Forms.RadioButton RdMainBasic;
    private System.Windows.Forms.GroupBox GrpAdvanced;
    private System.Windows.Forms.GroupBox GrpBasic;
    private ConStringTester conStringTester1;
    private System.Windows.Forms.TextBox TxtCustomSql;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown NumRowCount;
    private System.Windows.Forms.TextBox TxtSchemaObjectName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.RadioButton RdObjectTable;
    private System.Windows.Forms.RadioButton RdObjectSP;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox TxtCustomTable;
    private System.Windows.Forms.Button BtnGetColumns;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}
