using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MassDataHandler.Core;
using System.Xml;
using System.IO;

namespace MassDataHandler.Gui
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    #region Status Helpers

    private void SetStatus(Exception ex)
    {
      string strMessage = null;
      //if (ex is MDHException)
      //  strMessage = ex.Message;
      //else
      strMessage = ex.ToString();

      SetStatus(ex.Message, false);
      MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void SetStatus(string strMessage)
    {
      SetStatus(strMessage, false);
    }

    private void SetStatus(string strMessage, bool blnShowMsgBox)
    {
      strMessage += " (" + DateTime.Now.ToString() + ")";
      this.toolStripStatusLabel2.Text = strMessage;
      if (blnShowMsgBox)
        MessageBox.Show(strMessage);
    }

    #endregion

    #region Main

    private void Form1_Load(object sender, EventArgs e)
    {
      SetDefaultValues();
      LoadSettings();

    }

    private void LoadSettings()
    {
      string s = null;

      s = Properties.Settings.Default.InsertConString;
      if (s != null && s.Length > 0)
        this.conStringTesterCreateInserts.ConnectionString = s;

      s = Properties.Settings.Default.InsertImportPath;
      if (s != null && s.Length > 0)
        this.TxtImportDirectory.Text = s;

      s = Properties.Settings.Default.UseExistingConString;
      if (s != null && s.Length > 0)
        this.existingDataFilter1.ConnectionString = s;
    }

    private void SetDefaultValues()
    {
      SetDefaultValues_CodeGen();
      SetDefaultValues_UseExisting();
      SetDefaultValues_CreateInserts();
      this.DdConversionOptions.SelectedIndex = 0;
    }

    #endregion

    #region Existing Data

    private void SetDefaultValues_UseExisting()
    {
      existingDataFilter1.SetDetaultValues();
    }

    private void BtnGetExisting_Click(object sender, EventArgs e)
    {
      this.TxtExistingOutput.Text = "";

      ExistingData ed = new ExistingData();
      SelectionStrategy objSS = this.existingDataFilter1.GetSelectionStrategy();
      objSS.EnsureAtLeastOneRow = this.ChkEnsureOneRow.Checked;
      objSS.IncludeIdentityColumn = this.ChkIncludeIdentityColumn.Checked;

      //Validate
      //Rule: If customSQL is selected, ensure that the TableName textbox is filled out.
      if (objSS.IsCustomTableSelect && string.IsNullOrEmpty(objSS.SourceSchemaObject.ObjectName) && this.ChkIncludeIdentityColumn.Checked == false)
      {
        this.existingDataFilter1.SetCustomTableValue("ENTER VALUE!");
        MessageBox.Show("You are entering a custom select statement AND specifying to exclude Identity columns. Please fill in the TableName field so that the system knows why Table's identity column to exclude.");
        return;
      }


      try
      {
        string strXml = ed.CreateXml(objSS);
        strXml = XmlUtilities.FormatXml(strXml);


        if (this.ChkFormatAsCSharp.Checked)
        {
          //Format as C#
          strXml = strXml.Substring(8, strXml.Length - 17); //Remove "<Root>" nodes.
          strXml = strXml.Replace("\"", "\"\"");            //Escape quotes
          strXml = "#region Xml Data\r\n\r\nstring strData = @\"\r\n" + strXml + "\";\r\n\r\n#endregion\r\n";   //Add C# code
        }

        //add breaks after each table
        strXml = strXml.Replace("</Table>", "</Table>\r\n");
        strXml = strXml.Replace("</Meta>", "</Meta>\r\n");

        this.TxtExistingOutput.Text = strXml;

        if (objSS.IsCustomTableSelect)
          this.SetStatus("Success: Got xml inserts for custom table select.");
        else
          this.SetStatus("Success: Got xml inserts for " + objSS.SourceSchemaObject.SchemaType.ToString() + " '" + objSS.SourceSchemaObject.ObjectName + "'.");
      }
      catch (Exception ex)
      {
        this.SetStatus(ex);
      }

    }

    private void BtnGetSpSelects_Click(object sender, EventArgs e)
    {
      SchemaObject so = this.existingDataFilter1.GetSchemaObject();
      string strSelects = so.GetSelects(this.existingDataFilter1.ConnectionString);
      this.TxtExistingOutput.Text = strSelects;

      this.SetStatus("Got select tables for object '" + so.ObjectName + "'.");
    }

    #endregion

    #region Run Inserts

    private void SetDefaultValues_CreateInserts()
    {
      string sXml = @"<Root>

  <Variables>
    <Variable name=""lastName"" value=""Simpson"" />
  </Variables>

  <Table name=""Customer"">
    <Row CustomerName=""Homer $(lastName)"" />
    <Row CustomerName=""Marge $(lastName)"" />
  </Table>

  <Table name=""Product"">
    <Row ProductName=""KrustBurger"" Description=""best burger ever"" />
  </Table>

  <Table name=""Order"">
    <Default LastUpdate=""12/23/1997"" />
    <Row CustomerId=""Customer.@1"" ProductId=""Product.@1"" />
    <Row CustomerId=""Customer.@2"" ProductId=""Product.@1"" />
  </Table>
  
</Root>";

      this.TxtImportDirectory.Text = @"C:\Temp\MyXmlScripts";
      this.folderBrowserDialog1.SelectedPath = this.TxtImportDirectory.Text;

      this.TxtXmlScript.Text = sXml;
      this.conStringTesterCreateInserts.SetFromAppConfig();
      this.conStringTesterCreateInserts.ConnectionReadOnly = false;  //XmlInsert always goes to app config.

    }

    private void BtnDoInsert_Click(object sender, EventArgs e)
    {
      RunInserts();
    }

    private void BtnDoInsertPaste_Click(object sender, EventArgs e)
    {
      //get from clipboard
      string s = Clipboard.GetText();
      this.TxtXmlScript.Text = s;
      RunInserts();
    }

    private void RunInserts()
    {
      //Get xml and do insert
      InsertManager m = null;
      ImportStrategyFile objImport = new ImportStrategyFile(this.TxtImportDirectory.Text);
      XmlDocument xDoc = new XmlDocument();
      try
      {
        xDoc.LoadXml(this.TxtXmlScript.Text);

        InsertStrategy objStrategy = new InsertStrategy();
        objStrategy.UseDatabaseForTableSchema = this.ChkInsertLiveTableSchema.Checked;
        objStrategy.AllowIdentityInserts = this.chkInsertIdentityOn.Checked;
        objStrategy.ImportStrategyData = objImport;

        m = new InsertManager(objStrategy);
        ResultSql objResultSql = m.RunInserts(xDoc, this.conStringTesterCreateInserts.ConnectionString);

        //success, set resultsobjImport
        SetStatus("Success: Inserted " + objResultSql.TotalRowsInserted.ToString() + " row(s). Total time=" + objResultSql.TotalTime.Milliseconds.ToString() + " millisecond(s).");
        this.TxtResultSql.Text = objResultSql.GetSqlScript();
      }
      catch (Exception ex)
      {
        SetStatus(ex);
      }
    }

    private void BtnBrowse_Click(object sender, EventArgs e)
    {
      this.folderBrowserDialog1.ShowDialog();

      string strDir = this.folderBrowserDialog1.SelectedPath;

      this.TxtImportDirectory.Text = strDir;
    }

    private void BtnApplyImports_Click(object sender, EventArgs e)
    {
      InsertManager m = null;
      ImportStrategyFile objImport = new ImportStrategyFile(this.TxtImportDirectory.Text);
      XmlDocument xDoc = new XmlDocument();
      try
      {
        xDoc.LoadXml(this.TxtXmlScript.Text);

        InsertStrategy objStrategy = new InsertStrategy();
        objStrategy.ImportStrategyData = objImport;

        m = new InsertManager(objStrategy);
        XmlDocument xNew = m.ApplyImports(xDoc);

        this.TxtResultSql.Text = "";
        this.TxtXmlScript.Text = XmlUtilities.FormatXml(xNew.OuterXml);

        //success, set resultsobjImport
        SetStatus("Success: Applied all Import commands; nothing inserted yet.");
        
      }
      catch (Exception ex)
      {
        SetStatus(ex);
      }
    }

    #endregion

    #region Code Gen

    private string GetAppRoot()
    {
      try
      {
        //TODO: implement
        return System.Reflection.Assembly.GetCallingAssembly().Location;

        //string strCaller = System.Reflection.Assembly.GetCallingAssembly().Location;
        ////get first two parts of path
        //string[] s = strCaller.Split('\\');
        //string strRoot = s[0] + @"\" + s[1] + @"\" + s[2];
        //return strRoot;
      }
      catch
      {
        return "";
      }
    }

    private void SetDefaultValues_CodeGen()
    {
      string strAssemblyName = GetAppRoot(); // +@"\MassDataHandler.Core.dll";  //default to SystemFrameworks
      string strNamespace = "MassDataHandler.Gui";
      string strClassName = "EmployeeDemo"; //default to something that already exists
      string strObjectName = "input";

      this.TxtCodeGenAssembly.Text = strAssemblyName;
      this.TxtCodeGenNamespace.Text = strNamespace;
      this.TxtCodeGenClassName.Text = strClassName;
      this.TxtCodeGenObjectName.Text = strObjectName;
    }

    private void BtnCodeGenerate_Click(object sender, EventArgs e)
    {
      //get info, create class
      string strAssemblyName = this.TxtCodeGenAssembly.Text;
      string strNamespace = this.TxtCodeGenNamespace.Text;
      string strClassName = this.TxtCodeGenClassName.Text;
      string strObjectName = this.TxtCodeGenObjectName.Text;
      

      //ensure that all exist
      string strError = null;
      if (strAssemblyName.Length == 0)
        strError = "ERROR: Assembly Name must be entered.";
      else if (strClassName.Length == 0)
        strError = "ERROR: Class Name must be entered.";
      else if (strObjectName.Length == 0)
        strError = "ERROR: Object Name must be entered.";
      else if (strNamespace.Length == 0)
        strError = "ERROR: Namespace must be entered.";
      else if (!File.Exists(strAssemblyName))
        strError = "ERROR: Assembly '" + strAssemblyName + "' doesn't exist.";
      if (strError != null)
      {
        MessageBox.Show(strError);
        return;
      }

      string strCode = "";
      try
      {
        string strFullClassName = strNamespace + "." + strClassName;
        strCode = CodeGen.CreateObjectInstantiation(strAssemblyName, strFullClassName, strObjectName);
      }
      catch (Exception ex)
      {
        this.SetStatus(ex);
      }

      this.TxtCodeGenOutput.Text = strCode;
    }

    #endregion

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      SaveSettings();
    }

    private void SaveSettings()
    {
      //save data to settings
      Properties.Settings.Default.InsertConString = this.conStringTesterCreateInserts.ConnectionString;
      Properties.Settings.Default.InsertImportPath = this.TxtImportDirectory.Text;
      Properties.Settings.Default.UseExistingConString = this.existingDataFilter1.ConnectionString;

      Properties.Settings.Default.Save();
    }

    #region Menubar

    private void tutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      //open up IE for MDH tutorial
      string strUrl = System.Configuration.ConfigurationManager.AppSettings["WikiTutorialPage"].ToString();
      System.Diagnostics.Process.Start(strUrl);
    }

    private void restoreInitialSettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetDefaultValues();
    }

    private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveSettings();
    }

    #endregion

    #region Conversion

    private void BtnConvert_Click(object sender, EventArgs e)
    {
      /* Escaped format --> literal C# string with alias '@'
       * File format --> No escape characters, just as you'd open up a file.
       * 
       */
      int intIndex = this.DdConversionOptions.SelectedIndex;
      if (intIndex < 0)
      {
        MessageBox.Show("Please first select a conversion method.");
        return;
      }

      string strOriginal = this.TxtEscapedInitial.Text;
      string strFinal = null;
      string strConvertMessage = null;
      switch (intIndex)
      {
        case 0:
          strFinal = ConvertEscapedToFile(strOriginal);
          strConvertMessage = "Converted text.";
          break;
        case 1:
          strFinal = ConvertFileToEscaped(strOriginal);
          strConvertMessage = "Converted text.";
          break;
        default:
          strFinal = strOriginal;
          strConvertMessage = "Nothing converted";
          break;
      }

      this.TxtEscapedFinal.Text = strFinal;
      this.SetStatus(strConvertMessage);
    }

    private string ConvertFileToEscaped(string strOriginal)
    {
      strOriginal = strOriginal.Replace("\"", "\"\"");
      return strOriginal;
    }

    private string ConvertEscapedToFile(string strOriginal)
    {
      strOriginal = strOriginal.Replace("\"\"", "\"");
      return strOriginal;
    }


    #endregion

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AboutBox1 frm = new AboutBox1();
      frm.ShowDialog();
    }


  }
}