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
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class TableObjectTest
  {
    public TableObjectTest()
    {

    }

    #region CreateSqlInsert

    [TestMethod]
    public void CreateSqlInsert_1()
    {
      StringDictionary sd = new StringDictionary();
      sd.Add("co", "a");
      sd.Add("costCenterLevels", "1");

      TableTemp1 table1 = new TableTemp1();

      string strActual = SqlTextHelper.CreateSqlInsert(sd, table1);
      string strExpected = "Insert into [Info] (\t[co], \t[costcenterlevels] )\r\nValues(\t'a', \t1 );";
      Assert.AreEqual(strExpected, strActual);
    }

    [TestMethod]
    public void CreateSqlInsert_NULL()
    {
      StringDictionary sd = new StringDictionary();
      sd.Add("co", "a");
      sd.Add("costCenterLevels", "1");
      sd.Add("address3", "NulL");

      TableTemp1 table1 = new TableTemp1();

      string strActual = SqlTextHelper.CreateSqlInsert(sd, table1);
      string strExpected = "Insert into [Info] (\t[address3], \t[co], \t[costcenterlevels] )\r\nValues(\tNULL, \t'a', \t1 );";
      Assert.AreEqual(strExpected, strActual);
    }

    [TestMethod]
    public void CreateSelectString_1()
    {
      string[] astrCol = new string[1] { "col1" };
      string strExpected = "select col1 from myTable";
      string strActual = SqlTextHelper.CreateSelectString(astrCol, "myTable");

      Assert.AreEqual(strExpected, strActual);
    }

    [TestMethod]
    public void CreateSelectString_3()
    {
      string[] astrCol = new string[3] { "col1", "col2", "col3" };
      string strExpected = "select col1, col2, col3 from myTable";
      string strActual = SqlTextHelper.CreateSelectString(astrCol, "myTable");

      Assert.AreEqual(strExpected, strActual);
    }

    public class TableTemp1 : TableBase
    {
      public TableTemp1()
      {
        this.PrimaryKey = "InfoID";
        this.TableName = "Info";
      }

      #region Implement Abstract Methods

      protected override void SetMinimalInsertValues()
      {
        StringDictionary sd = new StringDictionary();
        sd.Add("co", "a");
        sd.Add("costCenterLevels", "1");
      }

      protected override void SetDataColumns()
      {
        DataColumn[] dc = new DataColumn[3];
        dc[0] = CreateDataColumn("co", typeof(string), false, 9);
        dc[1] = CreateDataColumn("address2", typeof(string), true, 30);
        dc[1] = CreateDataColumn("address3", typeof(string), false, 30);
        dc[2] = CreateDataColumn("costCenterLevels", typeof(int), false, 0);

        this.Columns = dc;
      }

      protected override void SetForeignKeyColumns()
      {
        //throw new Exception("The method or operation is not implemented.");
      }

      #endregion

    }

    #endregion
  }
}
