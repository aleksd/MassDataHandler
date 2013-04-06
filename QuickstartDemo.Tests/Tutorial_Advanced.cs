using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassDataHandler.Core;
using System.Xml;
using System.Collections.Specialized;
using System.Data;

namespace QuickstartDemo.Tests
{
  /// <summary>
  /// Summary description for Tutorial_Advanced
  /// </summary>
  [TestClass]
  public class Tutorial_Advanced
  {
    public Tutorial_Advanced()
    {
    }

    #region Round-Tripping

    /* The MassDataHandler framework roundtrips between selects and inserts. 
     * GOAL:   SqlData <--> XmlFragments <--> DataTable
     * 
     * This means that you can convert from:
     *  XmlFragments <--> DataTable
     *    XmlFragements --> DataTable [via InsertManager.CreateDataTable  ]
     *    DataTable --> XmlFragment   [via ExistingData.CreateTableXml(DataTable)  ]
     * 
     *  XmlFragments <--> SqlData
     *    XmlFragements --> SqlData   [via InsertManager.RunInserts  ]
     *    SqlData --> XmlFragment     [via ExistingData.CreateTableXml(string, string)  ]
     * 
     * You can of course also use DataUtilities to create ADO.Net objects from Sql strings, like:
     *    SqlData --> DataTable       (via DataUtilities.ExecuteDataTable )
     */

    /// <summary>
    /// Another big issue in database testing is how to check many values, such as an entire datatable result set.
    /// It's easy to check a single scalar, but this sometimes is not sufficient.
    /// </summary>
    [TestMethod]
    public void RoundTrip_DataTableSelect()
    {
      string @strData = @"
        <Table name=""Customer"">
          <Row CustomerName=""Homer Simpson"" Notes=""Nuclear Engineer"" />
          <Row CustomerName=""Marge Simpson"" Notes=""Notes about Marge here"" />
        </Table>
      ";

      //Insert xml script
      InsertManager im = new InsertManager();
      im.RunInserts(strData);

      //Do dataTable select --> returns DataTable
      DataTable dtSql = DataUtilities.ExecuteDataTable("select CustomerName, Notes from Customer");

      //Convert initial Xml to dataTable
      DataTable dtXml = im.CreateDataTable(strData);

      //Compare
      AssertDataTables(dtXml, dtSql);

    }

    [TestMethod]
    public void RoundTrip_XmlFragment()
    {
      string @strData = @"
        <Table name=""Customer"">
          <Row CustomerName=""Homer Simpson"" Notes=""Nuclear Engineer"" />
          <Row CustomerName=""Marge Simpson"" Notes=""Notes about Marge here"" />
        </Table>
      ";

      //Insert xml script
      InsertManager im = new InsertManager();
      im.RunInserts(strData);

      //Do existing data select --> return xmlFragment
      ExistingData ed = new ExistingData();
      string strXmlExisting = ed.CreateTableXml("Customer", "select CustomerName, Notes from Customer");

      //Compare --> need to remove white space
      AssertXmlEqual(strData, strXmlExisting);

    }

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

    #region Configuring with a strategy

    /// <summary>
    /// There are several ways to configure the InsertManager. If you want to configure it to be different than the
    /// default, you can create an InsertStrategy and pass that in. The Strategy will be very important as you do use
    /// more complex features.
    /// </summary>
    [TestMethod]
    public void Strategy()
    {
      //The default is to clear all data with each run. Lets modify that default.

      //Start this test with a clean slate.
      DataUtilities.DeleteTable("Product");  

      //Create a strategy and use it to instantiate the InsertManager.
      InsertStrategy objStrategy = new InsertStrategy();
      objStrategy.DeleteAllTableData = false;
      InsertManager im = new InsertManager(objStrategy);

      string strXml = @"
        <Table name=""Product"">
          <Row ProductName=""KrustBurger""  />
        </Table>
      ";

      //Ensure that there are no rows yet.
      Assert.AreEqual(0, DataUtilities.GetTableRowCount("Product"));

      im.RunInserts(strXml);

      //We just inserted a row.
      Assert.AreEqual(1, DataUtilities.GetTableRowCount("Product"));

      im.RunInserts(strXml);

      //We just inserted again. By default each insert clears the table. But we overrode that default.
      //  So now there are two rows, not just one.
      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Product"));


    }

    #endregion

    #region Include Statements and Aliases

    /// <summary>
    /// You can import scripts within other scripts using the "Import" element. Any script can 
    /// import multiple children. You can also next import statements such that an imported 
    /// script can itself import its own child. However, be careful to avoid circular references. 
    /// 
    /// Import scripts are great to refactor out reusable data, such as lookup values. 
    /// Importing requires setting the Insert Strategy to tell where the script is getting 
    /// imported from (such as an embedded resource).
    /// </summary>
    [TestMethod]
    public void Use_Import_Statement()
    {
      //Assemble the Strategy:
      ImportStrategyResource stratResource = new ImportStrategyResource();
      stratResource.EmbeddedResourceRoot = "XmlScripts.ImportSamples";
      //We're storing the resource files in this assembly:
      stratResource.ResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly();

      InsertStrategy strat = new InsertStrategy();
      strat.ImportStrategyData = stratResource;

      InsertManager im = new InsertManager(strat);

      //Call resource file:
      string strXml = @"
        <Import resource=""BaseData2.xml"" />
        <Import resource=""BaseData3.xml"" />

        <Table name=""Customer"">
          <Row CustomerName=""Homer Simpson"" Notes=""From Main"" />
        </Table>
      ";

      im.RunInserts(strXml);

      //Check results. Note that there are several more rows than just the one we inserted here.
      Assert.AreEqual(5, DataUtilities.GetTableRowCount("Customer"));

    }


    /// <summary>
    /// So far we've seen each table section being named after a specific database table. 
    /// This limits us to only having one section for each table. However, sometimes for
    /// identity lookups, you'll want to have multiple sections referring to the same database
    /// table:
    /// (1) You'll import a script that references the same table as the parent script. 
    /// (2) You'll have a huge table and you'll want to split it 
    /// into separate, smaller, tables so that it's easier to manage (especially is you're 
    /// doing identity lookups on it). 
    /// To handle this, the MDH has "Aliases". 
    /// </summary>
    [TestMethod]
    public void Use_Aliases()
    {
      string strXml = @"
        <Table name=""Customer"" alias=""Customer_Frequent"">
          <Row CustomerName=""Homer Simpson"" />
        </Table>

        <Table name=""Customer"" alias=""Customer_Rare"">
          <Row CustomerName=""Lisa Simpson"" />
        </Table>

        <Table name=""Product"">
          <Row ProductName=""KrustBurger"" Description=""best burger ever"" />
        </Table>

        <Table name=""Order"">
          <Row CustomerId=""Customer_Frequent.@1"" ProductId=""Product.@1"" />
          <Row CustomerId=""Customer_Rare.@1"" ProductId=""Product.@1"" />
        </Table>
      ";

      //NOTE: For the Order table, the identity lookup references the alias, not 
      //  the actual table name, i.e. "Customer_Frequent" not just "Customer".
      InsertManager im = new InsertManager();
      im.RunInserts(strXml);

      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Order"));
    }

    #endregion

    #region Insert Identity

    /// <summary>
    /// While the MDH framework provides Identity Lookups (via the "TableName.@N" expression)
    /// to simplify working with database-created identity values, sometimes you just want to
    /// directly insert those values – not use a lookup. You can configure the InsertStrategy
    /// to allow that.
    /// </summary>
    [TestMethod]
    public void Insert_Identity_On()
    {
      //Need to set strategy to allow for Identity Inserts
      InsertStrategy strat = new InsertStrategy();
      strat.AllowIdentityInserts = true;
      InsertManager im = new InsertManager(strat);

      string strXml = @"
        <Table name=""Customer"">
          <Row CustomerId=""64"" CustomerName=""Homer"" Notes=""NULL"" />
          <Row CustomerId=""65"" CustomerName=""Lisa"" Notes=""NULL"" />
        </Table>
      ";

      //This will directly insert the identity values (usually the Primary Key).
      //  By default, the database automatically generates these PK values.

      ResultSql objResult = im.RunInserts(strXml);

      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Customer"));

      //Saved Identity will be what we inserted it as.
      int intId = objResult.LookupIdentityValue("Customer",1);
      Assert.AreEqual(64, intId);

      //Database value will be this too
      int intDbId = Convert.ToInt32(DataUtilities.ExecuteScalar("select CustomerId from Customer where CustomerName='Homer'"));
      Assert.AreEqual(64, intDbId);


    }

    #endregion

    #region Triggers and Insert Identity

    /// <summary>
    /// Even with triggers on a table, the identity insert should still function intuitively.
    /// Also, you can override the identityInsert on a table-by-table basis using the attribute 'allowIdentityInserts'.
    /// </summary>
    [TestMethod]
    public void Insert_Identity_On_WithTrigger()
    {
      InsertManager im = new InsertManager();

      //NOTE: there is a trigger on the Custom table.
      string strXml = @"
        <Variables>
          <Variable name=""lastName"" value=""Simpson"" />
        </Variables>

        <Table name=""AuditTrail"">
        </Table>

        <Table name=""Customer"" allowIdentityInserts=""true"">
          <Row CustomerId=""28"" CustomerName=""Homer $(lastName)"" />
          <Row CustomerId=""29"" CustomerName=""Marge $(lastName)"" />
        </Table>

        <Table name=""Product"">
          <Row ProductName=""KrustBurger"" Description=""best burger ever"" />
        </Table>

        <Table name=""Order"">
          <Default LastUpdate=""12/23/1997"" />
          <Row CustomerId=""28"" ProductId=""Product.@1"" />
          <Row CustomerId=""Customer.@2"" ProductId=""Product.@1"" />
        </Table>
      ";

      ResultSql objResult = im.RunInserts(strXml);
      
      //For IdentityInsert=On, we still get the values we expected in the table.
      
      //ensure that trigger worked (adding one row for each Customer row)
      Assert.AreEqual(2, DataUtilities.GetTableRowCount("AuditTrail"));

      //Saved Identity will be what we inserted it as.
      int intId = objResult.LookupIdentityValue("Customer", 1);
      Assert.AreEqual(28, intId);

      //Lookup identity 'Customer.@2' will be what we expect
      //  i.e., we can reference lookups two ways: (1) using the hard-coded value, (2) always using the identity lookup.
      int intId2 = (Int32)DataUtilities.ExecuteScalar("select CustomerId from [Order] where CustomerId <> 28");
      Assert.AreEqual(29, intId2);

    }

    #endregion

    #region Misc Helpers

    [TestInitialize()]
    public void MyTestInitialize()
    {
      //Always clear audit table so that it doesn't get too big.
      DataUtilities.ExecuteNonQuery("delete from [AuditTrail]");
    }

    #endregion

  }
}
