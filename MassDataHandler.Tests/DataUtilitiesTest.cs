using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassDataHandler.Core;
using System.Data;
using System.Reflection;
using System.Collections.Specialized;

namespace MassDataHandler.Tests
{
  /// <summary>
  /// Summary description for DataUtilitiesTest
  /// </summary>
  [TestClass]
  public class DataUtilitiesTest
  {
    public DataUtilitiesTest()
    {

    }

    #region Generation DataTables

    private DataTable CreateDataTable_2x1()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(new DataColumn("col1", typeof(string)));
      dt.Columns.Add(new DataColumn("col2", typeof(string)));

      DataRow dr = null;
      dr = dt.NewRow();
      dr["col1"] = "aaa";
      dr["col2"] = "bbb";
      dt.Rows.Add(dr);

      return dt;
    }

    private DataTable CreateDataTable_2x2()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(new DataColumn("col1", typeof(string)));
      dt.Columns.Add(new DataColumn("col2", typeof(string)));

      DataRow dr = null;
      dr = dt.NewRow();
      dr["col1"] = "aaa";
      dr["col2"] = "bbb";
      dt.Rows.Add(dr);

      dr = dt.NewRow();
      dr["col1"] = "cccc";
      dr["col2"] = "dddd";
      dt.Rows.Add(dr);

      return dt;
    }

    private DataTable CreateDataTable_2x2int()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(new DataColumn("col1", typeof(string)));
      dt.Columns.Add(new DataColumn("col2", typeof(int)));

      DataRow dr = null;
      dr = dt.NewRow();
      dr["col1"] = "aaa";
      dr["col2"] = 111;
      dt.Rows.Add(dr);

      dr = dt.NewRow();
      dr["col1"] = "cccc";
      dr["col2"] = 222;
      dt.Rows.Add(dr);

      return dt;
    }


    private DataTable CreateDataTable_3x2()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(new DataColumn("col1", typeof(string)));
      dt.Columns.Add(new DataColumn("col2", typeof(int)));
      dt.Columns.Add(new DataColumn("col3", typeof(DateTime)));

      DataRow dr = null;
      dr = dt.NewRow();
      dr["col1"] = "aaa";
      dr["col2"] = 1111;
      dr["col3"] = new DateTime(2001, 1, 1);
      dt.Rows.Add(dr);

      dr = dt.NewRow();
      dr["col1"] = "bbb";
      dr["col2"] = 222;
      dr["col3"] = new DateTime(2002, 2, 2);
      dt.Rows.Add(dr);

      dr = dt.NewRow();
      dr["col1"] = "cccc";
      dr["col2"] = 3333;
      dr["col3"] = new DateTime(2003, 3, 3);
      dt.Rows.Add(dr);

      return dt;
    }

    #endregion

    #region CompareDataTableAsString

    [TestMethod]
    public void CompareDataTableAsString_Fail_Null()
    {
      DataTable dt1 = CreateDataTable_2x1();
      DataTable dt2 = null;
      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc.Count > 0);
    }

    [TestMethod]
    public void CompareDataTableAsString_Fail_RowCol()
    {
      DataTable dt1 = CreateDataTable_2x1();
      DataTable dt2 = CreateDataTable_3x2();
      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc.Count > 0);
    }

    [TestMethod]
    public void CompareDataTableAsString_Fail_ColNames()
    {
      DataTable dt1 = CreateDataTable_2x2();
      dt1.Columns[0].ColumnName = "newCol";
      DataTable dt2 = CreateDataTable_2x2();

      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc.Count > 0);
    }

    [TestMethod]
    public void CompareDataTableAsString_Fail_CellValues()
    {
      DataTable dt1 = CreateDataTable_2x2();
      dt1.Rows[0][0] = "new_xyz";
      DataTable dt2 = CreateDataTable_2x2();

      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc.Count > 0);
    }

    [TestMethod]
    public void CompareDataTableAsString_Pass_Case()
    {
      DataTable dt1 = CreateDataTable_2x2();
      dt1.Rows[0][0] = "AAA";
      DataTable dt2 = CreateDataTable_2x2();
      dt1.Rows[0][0] = "aaa";

      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc == null);
    }

    [TestMethod]
    public void CompareDataTableAsString_Fail_Trim()
    {
      DataTable dt1 = CreateDataTable_2x2();
      dt1.Rows[0][0] = "AAA";
      DataTable dt2 = CreateDataTable_2x2();
      dt2.Rows[0][0] = "   AAA   ";

      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc.Count > 0);
    }

    [TestMethod]
    public void CompareDataTableAsString_Pass_Same()
    {
      DataTable dt1 = CreateDataTable_2x2();
      DataTable dt2 = CreateDataTable_2x2();

      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc == null);
    }

    [TestMethod]
    public void CompareDataTableAsString_Pass_Type()
    {
      DataTable dt1 = CreateDataTable_2x2();
      dt1.Rows[0][1] = "123";
      dt1.Rows[1][1] = "4444";
      DataTable dt2 = CreateDataTable_2x2int();
      dt2.Rows[0][1] = 123;
      dt2.Rows[1][1] = 4444;

      StringCollection sc = DataUtilities.CompareDataTableAsString(dt1, dt2);
      Assert.IsTrue(sc == null);
    }

    #endregion

  }
}
