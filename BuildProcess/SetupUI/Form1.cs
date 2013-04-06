using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MassDataHandler.Core;
using System.IO;
using System.Collections.Specialized;

namespace SetupUI
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    #region Buttons

    private void BtnTestDatabase_Click(object sender, EventArgs e)
    {
      string s = IsConnectionValid();
      if (s == null)
        MessageBox.Show("Success - can connect to SQL 2005!");
      else
        MessageBox.Show(s, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void BtnMSTest_Click(object sender, EventArgs e)
    {
      string s = IsMSTestValid();
      if (s == null)
        MessageBox.Show("Success - detected MSTest!");
      else
        MessageBox.Show(s, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void BtnMSBuild_Click(object sender, EventArgs e)
    {
      string s = IsMSBuildValid();
      if (s == null)
        MessageBox.Show("Success - detected MSBuild!");
      else
        MessageBox.Show(s, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void BtnFinalCheck_Click(object sender, EventArgs e)
    {

      try
      {

        StringCollection sc = new StringCollection();
        string s = null;

        //Check each step:
        s = IsConnectionValid();
        if (s != null)
          sc.Add(s);

        s = IsMSTestValid();
        if (s != null)
          sc.Add(s);

        s = IsMSBuildValid();
        if (s != null)
          sc.Add(s);

        //Final result
        if (sc.Count == 0)
        {
          UpdateFiles();
          MessageBox.Show("Success! Everything looks good.");
          this.Close();
        }
        else
        {
          //There were errors!
          StringBuilder sb = new StringBuilder("There are several possible problems, do you want to use these values?\r\n\r\n");
          foreach (string s1 in sc)
          {
            sb.Append("- " + s1 + "\r\n");
          }
          sb.Append("\r\nClick 'No' to change these values.");
          DialogResult dr = MessageBox.Show(sb.ToString(), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

          if (dr == DialogResult.Yes)
          {
            //Check
            DialogResult dr2 = MessageBox.Show("Are you sure? Using bad values here will likely prevent you from running the quickstart", "Warning", MessageBoxButtons.YesNo);
            if (dr2 == DialogResult.No)
              return;

            //Yes - continue
            Program.ReturnCode = Program.ReturnCode_Error;
            Console.WriteLine(sb.ToString());
            Console.WriteLine("You chose to continue despite errors, app may not work correctly.");
            UpdateFiles();
            this.Close();
          }
          else
          {
            //No - don't continue
            return;
          }


        }

      }
      catch (Exception ex)
      {
        MessageBox.Show("There was an error: " + ex.Message, "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
      }


    }

    #endregion

    #region Check Envrionment

    private string IsConnectionValid()
    {
      try
      {
        string strDBCon = @"Data Source=" + this.TxtDataSource.Text + ";Initial Catalog=master;User Id=SA;Password=" + this.TxtSaPassword.Text;
        string s = DataUtilities.IsConnectionValid(strDBCon);
        if (s == null)
        {
          //Success - can connect to DB - Now check that 2005 is installed
          string strResult = Convert.ToString(DataUtilities.ExecuteScalarGivenConString("select @@version", strDBCon));

          if (strResult == null)
            return "Error checking version";
          else if (strResult.IndexOf("Microsoft SQL Server 2005") >= 0)
            return null; //Good!
          else
            return "It does not appear that you have SQL Server 2005 installed.";
        }
        else
          return "Database connection is invalid: " + s;
      }
      catch (Exception ex)
      {
        return "There was an error checking your database info: " + ex.Message;
      }

    }

    private string IsMSTestValid()
    {
      string strDir = this.TxtMsTest.Text;
      //Check for existence of file: MSTest.exe
      string strFile = Path.Combine(strDir, "MSTest.exe");
      if (File.Exists(strFile))
        return null;
      else
        return "Could not locate MSTest.exe. Please make sure that the MSTest directory is valid.";
    }

    private string IsMSBuildValid()
    {
      string strDir = this.TxtMsBuild.Text;
      //Check for existence of file: MSTest.exe
      string strFile = Path.Combine(strDir, "MSBuild.exe");
      if (File.Exists(strFile))
        return null;
      else
        return "Could not locate MSBuild.exe. Please make sure that the MSBuild directory is valid.";
    }

    #endregion

    private void UpdateFiles()
    {
      string strContent = null;
      string strFile = null;

      #region Update MSBuild Script

      //1 - Update MSBuild script
      strContent = @"set msBuildPath=" + this.TxtMsBuild.Text + @"

""%msBuildPath%\MSBuild.exe"" RunTests.msbuild ""/p:basedir=%cd%\.."" /l:FileLogger,Microsoft.Build.Engine;logfile=RunTests.log

Pause
";
      strFile = @"..\..\..\RunTests.bat";
      FileUtilities.WriteFileContents(strContent, strFile);
      UpdateStatus("Modified bat file at: " + Path.GetFullPath(strFile));

      #endregion

      #region Update CommonProperties.msbuild

      //2 - update CommonProperties.msbuild
      strContent = @"<Project DefaultTargets=""EndPoint"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"" >
  <PropertyGroup>
    <!-- Database -->
    <DBConstringSAUsername>SA</DBConstringSAUsername>
    <DBConstringSAPassword>" + this.TxtSaPassword.Text + @"</DBConstringSAPassword>
    <DBConstringSADataSource>" + this.TxtDataSource.Text + @"</DBConstringSADataSource>

    <!-- Applications -->
    <mstestdir>" + this.TxtMsTest.Text + @"</mstestdir>

  </PropertyGroup>
</Project>
";
      strFile = @"..\..\..\CommonProperties.msbuild";
      FileUtilities.WriteFileContents(strContent, strFile);
      UpdateStatus("Modified bat file at: " + Path.GetFullPath(strFile));

      #endregion

    }

    private void UpdateStatus(string strMessage)
    {
      //MessageBox.Show(strMessage);
      //this.toolStripStatusLabel1.Text += strMessage;
    }

    private void SetDefaults()
    {
      this.TxtSaPassword.Text = "myPassword";
      this.TxtDataSource.Text = @".\SQLSERVER2005";
      this.TxtMsBuild.Text = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727";
      this.TxtMsTest.Text = @"C:\Program Files\Microsoft Visual Studio 8\Common7\IDE";
    }

    private void BtnReset_Click(object sender, EventArgs e)
    {
      SetDefaults();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      SetDefaults();
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
      Program.ReturnCode = Program.ReturnCode_Cancel;
      this.Close();
    }


  }
}