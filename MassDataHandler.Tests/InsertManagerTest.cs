using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using MassDataHandler.Core;
using MassDataHandler.Core.Table;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using System.Reflection;

namespace MassDataHandler.Tests
{
  /// <summary>
  /// Summary description for InsertManagerTest
  /// </summary>
  [TestClass]
  public class InsertManagerTest
  {
    public InsertManagerTest()
    {

    }

    #region GetVariables

    [TestMethod]
    public void InsertManager_GetVariables_1()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(@"
          <Root>
            <Variables>
              <Variable name='co' value='0669' />
              <Variable name='username' value='Mr.Smith' />
            </Variables>
          </Root>
          ");

      StringDictionary sd = InsertManager.GetVariables(xDoc);
      Assert.AreEqual(2, sd.Count);
      Assert.AreEqual(sd["co"], "0669");
      Assert.AreEqual(sd["username"], "Mr.Smith");

    }

    [TestMethod]
    public void InsertManager_GetVariables_2()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(@"
          <Root>
            <Variables>
            </Variables>
          </Root>
          ");

      StringDictionary sd = InsertManager.GetVariables(xDoc);
      Assert.AreEqual(0, sd.Count);
    }

    [TestMethod]
    public void InsertManager_GetVariables_3()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(@"
          <Root>
          </Root>
          ");

      StringDictionary sd = InsertManager.GetVariables(xDoc);
      Assert.AreEqual(0, sd.Count);
    }

    #endregion

    #region CreateDataTable

    [TestMethod]
    public void CreateDataTable_Basic()
    {
      string strXml = @"
        <Root>
          <Table name=""Customer"">
            <Row CustomerName=""Homer Simpson"" Notes=""Nuclear Engineer"" />
            <Row CustomerName=""Marge Simpson"" Notes=""Notes about Marge here"" />
          </Table>
        </Root>";
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(strXml);

      InsertManager im = new InsertManager();
      DataTable dt = im.CreateDataTable(xDoc);

      Assert.AreEqual(2, dt.Columns.Count);
      Assert.AreEqual(2, dt.Rows.Count);
      Assert.AreEqual("Marge Simpson", dt.Rows[1]["CustomerName"]);

    }

    private TableOutputs GetInstanceTableOutputs()
    {
      TableOutputs tOutputs = new TableOutputs();

      TableOutput t1 = new TableOutput("Service", null);
      t1.Rows.Add(CreateSingleSD("ServiceId", "111"));
      t1.Rows.Add(CreateSingleSD("ServiceId", "222"));

      tOutputs.Tables.Add(t1);

      return tOutputs;
    }

    private StringDictionary CreateSingleSD(string strKey, string strVal)
    {
      StringDictionary sd = new StringDictionary();
      sd.Add(strKey, strVal);
      return sd;

    }

    #endregion

    #region RoundTrip Demos

    #region Helper Methods

    private static void AssertXmlEqual(string str1, string str2)
    {
      XmlDocument xDoc1 = new XmlDocument();
      xDoc1.LoadXml(str1);

      XmlDocument xDoc2 = new XmlDocument();
      xDoc2.LoadXml(str2);

      Assert.AreEqual(xDoc1.OuterXml, xDoc2.OuterXml);

    }

    public void AssertDataTables(DataTable dtExpected, DataTable dtActual)
    {
      StringCollection sc = MassDataHandler.Core.DataUtilities.CompareDataTableAsString(dtExpected, dtActual);
      Assert.IsTrue(sc == null);
    }

    #endregion

    #endregion

    [TestMethod]
    public void AddRootNodes_1()
    {
      string strXml = "  <Root><node1>abc</node1></Root> ";
      XmlDocument xDoc = InsertManager.AddRootNodes(strXml);
      Assert.AreEqual("<Root><node1>abc</node1></Root>", xDoc.OuterXml);
    }

    [TestMethod]
    public void AddRootNodes_2()
    {
      string strXml = "  <node1>abc</node1> ";
      XmlDocument xDoc = InsertManager.AddRootNodes(strXml);
      Assert.AreEqual("<Root><node1>abc</node1></Root>", xDoc.OuterXml);
    }


  }
}
