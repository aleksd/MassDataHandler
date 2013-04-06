using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassDataHandler.Core;
using System.Collections.Specialized;
using System.Xml;
using System.Data;

namespace MassDataHandler.Tests
{
  /// <summary>
  /// Summary description for UtilityTest
  /// </summary>
  [TestClass]
  public class UtilityTest
  {
    public UtilityTest()
    {

    }

    #region MergeStringDictionaries

    [TestMethod]
    public void MergeStringDictionaries_1()
    {
      StringDictionary sd1 = new StringDictionary();
      sd1.Add("aaa", "111");
      sd1.Add("bbb", "222");
      StringDictionary sd2 = new StringDictionary();
      sd2.Add("aaa", "333");
      sd2.Add("ccc", "444");

      StringDictionary sd3 = Utilities.MergeStringDictionaries(sd1, sd2);
      Assert.AreEqual(3, sd3.Count);
      Assert.AreEqual("333", sd3["aaa"]);
      Assert.AreEqual("222", sd3["bbb"]);
      Assert.AreEqual("444", sd3["ccc"]);
    }

    [TestMethod]
    public void MergeStringDictionaries_Case()
    {
      StringDictionary sd1 = new StringDictionary();
      sd1.Add("AAA", "111");
      sd1.Add("bbb", "222");
      StringDictionary sd2 = new StringDictionary();
      sd2.Add("aaa", "333");
      sd2.Add("ccc", "444");

      StringDictionary sd3 = Utilities.MergeStringDictionaries(sd1, sd2);
      Assert.AreEqual(3, sd3.Count);
      Assert.AreEqual("333", sd3["aaa"]);
      Assert.AreEqual("222", sd3["bbb"]);
      Assert.AreEqual("444", sd3["ccc"]);
    }

    [TestMethod]
    public void MergeStringDictionaries_2()
    {
      StringDictionary sd1 = new StringDictionary();
      sd1.Add("aaa", "111");
      sd1.Add("bbb", "222");
      StringDictionary sd2 = null;

      StringDictionary sd3 = Utilities.MergeStringDictionaries(sd1, sd2);
      Assert.AreEqual(2, sd3.Count);
      Assert.AreEqual("111", sd3["aaa"]);
      Assert.AreEqual("222", sd3["bbb"]);

    }

    [TestMethod]
    public void MergeStringDictionaries_PersistOriginal()
    {
      StringDictionary sd1 = new StringDictionary();
      sd1.Add("aaa", "111");
      sd1.Add("bbb", "222");
      StringDictionary sd2 = new StringDictionary();
      sd2.Add("aaa", "333");
      sd2.Add("ccc", "444");

      StringDictionary sd3 = Utilities.MergeStringDictionaries(sd1, sd2);
      Assert.AreEqual(3, sd3.Count);
      Assert.AreEqual("333", sd3["aaa"]);
      Assert.AreEqual("222", sd3["bbb"]);
      Assert.AreEqual("444", sd3["ccc"]);

      //Persist original:
      Assert.AreEqual(2, sd1.Count);
      Assert.AreEqual(2, sd2.Count);
      Assert.AreEqual("111", sd1["aaa"]);
      Assert.AreEqual("222", sd1["bbb"]);
    }

    #endregion

    [TestMethod]
    public void PopulateSDFromXmlAttributes()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml("<Default BankId=\"@1\" col4=\"xyz\" />");
      XmlNode n = xDoc.ChildNodes[0];

      StringDictionary sd = Utilities.PopulateSDFromXmlAttributes(n);
      Assert.AreEqual(2, sd.Count);
      Assert.AreEqual(sd["BankId"], "@1");
      Assert.AreEqual(sd["col4"], "xyz");
    }

    [TestMethod]
    public void CreateTableSchema()
    {
      string strXml = @"
        <Table name=""BankService"">
          <Row BankServiceID=""1561"" co=""0669"" XServiceID=""1470"" startDate=""5/24/2005 12:00:00 AM"" />
          <Row BankServiceID=""1562"" co=""9Demo"" XServiceID=""1470"" startDate=""3/1/2005 12:00:00 AM"" />
          <Row BankServiceID=""1563"" co=""9Demo"" XServiceID=""1471"" startDate=""10/31/2004 12:00:00 AM"" />
        </Table>";
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(strXml);
      XmlNode xNode = xDoc.ChildNodes[0];

      DataTable dt = Utilities.CreateTableSchemaFromXmlNode(xNode);

      Assert.AreEqual("BankService", dt.TableName);
      Assert.AreEqual(4, dt.Columns.Count);
      Assert.AreEqual("XServiceID", dt.Columns[2].ColumnName);

    }


    [TestMethod]
    public void CreateDataRowFromSD()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add(new DataColumn("aaa", typeof(string)));
      dt.Columns.Add(new DataColumn("bbbb", typeof(string)));

      StringDictionary sd = new StringDictionary();
      sd.Add("aaa", "val1");
      sd.Add("bbbb", "val2");

      DataRow dr = dt.NewRow();
      dr = Utilities.CreateDataRowFromSD(dr, sd);

      Assert.AreEqual("val1", Convert.ToString(dr["aaa"]));
      Assert.AreEqual("val2", Convert.ToString(dr["bbbb"]));

    }

  }
}
