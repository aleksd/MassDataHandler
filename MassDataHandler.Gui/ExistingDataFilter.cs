using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MassDataHandler.Core;

namespace MassDataHandler.Gui
{
  public partial class ExistingDataFilter : UserControl
  {
    public ExistingDataFilter()
    {
      InitializeComponent();
    }

    private void ExistingDataFilter_Load(object sender, EventArgs e)
    {
      if (this.conStringTester1.ConnectionString.Length == 0 )
        this.conStringTester1.SetFromAppConfig();
      this.TxtSchemaObjectName.Text = "Customer";
      SetDisabledControls(true);
    }

    public void SetDetaultValues()
    {
      this.conStringTester1.SetFromAppConfig();
      this.TxtSchemaObjectName.Text = "Customer";
      SetDisabledControls(true);
    }

    public string ConnectionString
    {
      get
      {
        return this.conStringTester1.ConnectionString;
      }
      set
      {
        this.conStringTester1.ConnectionString = value;
      }
    }

    public SchemaObject GetSchemaObject()
    {
      SchemaObject so = new SchemaObject();
      so.ObjectName = this.TxtSchemaObjectName.Text.Trim();
      if (this.RdObjectSP.Checked)
        so.SchemaType = SchemaObject.SchemaObjectType.StoredProcedure;
      else
        so.SchemaType = SchemaObject.SchemaObjectType.Table;

      return so;
    }

    public SelectionStrategy GetSelectionStrategy()
    {
      SelectionStrategy objSS = new SelectionStrategy();
      objSS.DatabaseConnectionString = this.conStringTester1.ConnectionString;

      if (this.RdMainAdvanced.Checked)
      {
        objSS.CustomTableFilter = this.TxtCustomSql.Text;
        SchemaObject so = new SchemaObject();
        objSS.SourceSchemaObject = so;
        objSS.SourceSchemaObject.ObjectName = this.TxtCustomTable.Text.Trim();
        objSS.SourceSchemaObject.SchemaType = SchemaObject.SchemaObjectType.Table;
      }
      else if (this.RdMainBasic.Checked)
      {
        SchemaObject so = GetSchemaObject();
        //SchemaObject so = new SchemaObject();
        //so.ObjectName = this.TxtSchemaObjectName.Text;
        //if (this.RdObjectSP.Checked)
        //  so.SchemaType = SchemaObject.SchemaObjectType.StoredProcedure;
        //else
        //  so.SchemaType = SchemaObject.SchemaObjectType.Table;

        objSS.SourceSchemaObject = so;
        objSS.MaxRowsPerTable = Convert.ToInt32(this.NumRowCount.Value);
        //objSS.Companies = ArrayUtilities.SplitCSVToArray(this.TxtCoFilter.Text);
      }

      return objSS;
    }

    #region Main Radio Buttons

    private void RdMainAdvanced_CheckedChanged(object sender, EventArgs e)
    {
      if (this.RdMainAdvanced.Checked)
        SetDisabledControls(false);
    }

    private void RdMainBasic_CheckedChanged(object sender, EventArgs e)
    {
      if (this.RdMainBasic.Checked)
        SetDisabledControls(true);
    }

    private void SetDisabledControls(bool blnBasicEnabled)
    {
      //Disable
      this.GrpBasic.Enabled = blnBasicEnabled;
      this.GrpAdvanced.Enabled = !blnBasicEnabled;
    }

    #endregion

    private void BtnGetColumns_Click(object sender, EventArgs e)
    {
      //GetAllColumnsForTable
      try
      {
        string strTable = this.TxtCustomTable.Text;
        if (strTable.Length == 0)
          return;

        string[] astrColumns = DataUtilities.Schema.GetAllColumnsForTable(strTable, this.conStringTester1.ConnectionString);
        string strSelect = SqlTextHelper.CreateSelectString(astrColumns, strTable);

        this.TxtCustomSql.Text = strSelect;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error: " + ex.Message);
      }
    }

    public void SetCustomTableValue(string strVal)
    {
      this.TxtCustomTable.Text = strVal;
    }

  }
}
