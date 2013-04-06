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
  public partial class ConStringTester : UserControl
  {
    public ConStringTester()
    {
      InitializeComponent();
    }

    private void BtnTestCon_Click(object sender, EventArgs e)
    {
      string strMsg = DataUtilities.IsConnectionValid(this.TxtConString.Text);
      if (strMsg == null)
        MessageBox.Show("Success: Connected to Database.");
      else
        MessageBox.Show("Error, Could not open connection: " + strMsg);
    }

    public string ConnectionString
    {
      get { return this.TxtConString.Text; }
      set { this.TxtConString.Text = value; }
    }

    private bool _blnConnectionReadOnly;
    public bool ConnectionReadOnly
    {
      get { return this.TxtConString.ReadOnly; }
      set { this.TxtConString.ReadOnly = value; }
    }
	
    public void SetFromAppConfig()
    {
      this.TxtConString.Text = CreateConnectionString();
    }

    private string CreateConnectionString()
    {
      try
      {
        return DataUtilities.CreateConnectionStringFromAppConfig();
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error in creating connection string. " + ex.Message);
        return "";
      }

    }


  }
}
