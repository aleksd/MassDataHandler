using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassDataHandler.Core;

namespace MassDataHandler.Tests
{
  /// <summary>
  /// Summary description for SelectionStrategyTest
  /// </summary>
  [TestClass]
  public class SelectionStrategyTest
  {
    public SelectionStrategyTest()
    {

    }

    #region Helper Methods

    private string _strDbCon = "my connection string";
    private string _strCustomSql = "select * from Info";

    private SelectionStrategy GetSelectionStrategy_Advanced()
    {
      SelectionStrategy objSS = new SelectionStrategy();
      SchemaObject so = new SchemaObject();
      objSS.SourceSchemaObject = so;
      objSS.DatabaseConnectionString = _strDbCon;

      objSS.CustomTableFilter = _strCustomSql;

      return objSS;
    }

    private SelectionStrategy GetSelectionStrategy_Basic()
    {
      SelectionStrategy objSS = new SelectionStrategy();
      objSS.DatabaseConnectionString = _strDbCon;
      SchemaObject so = new SchemaObject();
      so.SchemaType = SchemaObject.SchemaObjectType.StoredProcedure;
      so.ObjectName = "xxx";

      objSS.SourceSchemaObject = so;
      objSS.MaxRowsPerTable = Convert.ToInt32(20);

      return objSS;
    }

    #endregion

    [TestMethod]
    public void CreateSqlSelect_Custom()
    {
      SelectionStrategy objSS = GetSelectionStrategy_Advanced();
      objSS.CustomTableFilter = _strCustomSql;
      string strSqlActual = objSS.CreateSqlSelect("xxx", true); //Both params irrelevant for this
      string strSqlExpected = _strCustomSql;
      Assert.AreEqual(strSqlExpected, strSqlActual);
    }

    [TestMethod]
    public void CreateSqlSelect_TopN()
    {
      SelectionStrategy objSS = GetSelectionStrategy_Advanced();
      objSS.MaxRowsPerTable = 5;
      objSS.SourceSchemaObject.ObjectName = "yyy";  //table irrelevant for this
      objSS.CustomTableFilter = "";
      string strSqlActual = objSS.CreateSqlSelect("MyTable", false);
      string strSqlExpected = "select top 5 * from MyTable";
      Assert.AreEqual(strSqlExpected, strSqlActual);
    }

    [TestMethod]
    public void CreateSqlSelect_CoFilter_1()
    {
      SelectionStrategy objSS = GetSelectionStrategy_Advanced();
      objSS.Companies = new string[] { "co1", "co2" };
      objSS.CustomTableFilter = "";
      string strSqlActual = objSS.CreateSqlSelect("MyTable", true);
      string strSqlExpected = "select * from MyTable where co in ('co1', 'co2')";
      Assert.AreEqual(strSqlExpected, strSqlActual);
    }

    [TestMethod]
    public void CreateSqlSelect_CoFilter_2()
    {
      SelectionStrategy objSS = GetSelectionStrategy_Advanced();
      objSS.Companies = new string[] { "co1", "co2" };
      objSS.CustomTableFilter = "";
      string strSqlActual = objSS.CreateSqlSelect("MyTable", false);    //table doesn't have a co role
      string strSqlExpected = "select * from MyTable";
      Assert.AreEqual(strSqlExpected, strSqlActual);
    }

    [TestMethod]
    public void GetSerializedCustomStrategy_1()
    {
      SelectionStrategy objSS = GetSelectionStrategy_Advanced();
      objSS.CustomTableFilter = _strCustomSql;
      objSS.SourceSchemaObject.ObjectName = "a,b,c";

      string strActual = objSS.GetSerializedCustomStrategy();
      string strExpected = "This xml snippet was created with the CustomSql section of the 'Use Existing Data' tab. Here are the values used to get this data:\r\n\r\n--TableName(s):\r\na,b,c\r\n\r\n--Custom Sql:\r\nselect * from Info\r\n";

      Assert.AreEqual(strExpected, strActual);

    }

  }
}
