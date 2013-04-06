using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MassDataHandler.Core;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace QuickstartDemo.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class Tutorial_Basic
  {
    public Tutorial_Basic()
    {

    }

    #region Basics

    /// <summary>
    /// Insert base data into the database and then call either a custom SQL select statement, or
    /// stored procedure that uses that data.
    /// </summary>
    [TestMethod]
    public void Sql_Select()
    {
      //STEP 1: Insert base data (this will automatically delete any existing data in the table)
      string @strXml = @"
        <Table name=""Customer"">
          <Row CustomerName=""Homer Simpson"" Notes=""Nuclear Engineer"" />
          <!-- This is just an xml comment. Insert comments wherever you want. -->
          <Row CustomerName=""Marge Simpson"" Notes=""Notes about Marge here"" />
        </Table>
      ";

      //Database connection string is stored in app.config.
      //  We could explicity specify it here if we wanted to.
      InsertManager im = new InsertManager();
      ResultSql objResult = im.RunInserts(strXml);

      //STEP 2: Do a simple data-retrieval test and check the results.

      //OPTION 1: do any SQL select you want:
      DataTable dt1 = DataUtilities.ExecuteDataTable("select * from Customer");
      Assert.AreEqual(2, dt1.Rows.Count);

      //OPTION 2: call a SP (NOTE: NEVER call a SP using the exec in product code. This is just an illustration. 
      //    You should use data-access code, like the Microsoft Data Access Block, to do this in a more robust way.

      //When we ran InsertManger.RunInserts(), it automatically saves all identity values for us (which is usually the primary key). 
      //  We easily reference those by specifying the table and index (1-based) using the LookupIdentityValue method
      //  "Homer" was the first row we inserted, therefore this will get Homer's identity value.

      int intCustomerId = objResult.LookupIdentityValue("Customer", 1);
      DataTable dt2 = DataUtilities.ExecuteDataTable("exec Customer_Get " + intCustomerId.ToString());
      Assert.AreEqual("Homer Simpson", Convert.ToString(dt2.Rows[0]["CustomerName"]));

    }

    /// <summary>
    /// Insert base data, and then call a stored proc that inserts a new row that would conflict with a unique index
    /// on Customer.CustomerName. Besides unique-key violations, you could also test transactions, logic that required 
    /// existing data (like lookups), etc...
    /// </summary>
    [TestMethod]
    public void Sql_Insert()
    {
      //STEP 1: Insert base data (this will automatically delete any existing data in the table)
      string @strXml = @"
        <Table name=""Customer"">
          <Row CustomerName=""Homer Simpson"" Notes=""Nuclear Engineer"" />
        </Table>
      ";

      InsertManager im = new InsertManager();
      im.RunInserts(strXml);

      //There is only 1 row, the one we just inserted.
      Assert.AreEqual(1, DataUtilities.GetTableRowCount("Customer"));

      //STEP 2: Insert data --> This will work because it doesn't conflict yet.
      DataUtilities.ExecuteNonQuery("exec Customer_Save 'Lisa Simpson', 'notes here'");

      //There is now 2 rows
      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Customer"));

      //Insert a customer with the name "Homer Simpson". It will fail because
      //that name already exist. 
      try
      {
        DataUtilities.ExecuteNonQuery("exec Customer_Save 'Homer Simpson', 'notes here'");
        Assert.Fail("This line should never be hit.");
      }
      catch (SqlException ex)
      {
        //There are still just 2 rows.
        Assert.AreEqual(2, DataUtilities.GetTableRowCount("Customer"));
      }

    }

    #endregion

    #region Xml Tricks

    /// <summary>
    /// You can specify variables as name-value pairs, and then substitute them in the
    /// column values. You can also create a "default row"
    /// </summary>
    [TestMethod]
    public void Trick_Variable_Defaults_1()
    {
      //VARIABLE feature
      //  Here we specify a single variable "lastName".
      //  Variables can be concatenated with other variables and literal strings.
      //DEFAULT feature:
      //  We also use a default row that specifies the default value for the Notes column.
      //  Any individual row can always override the default.

      string @strXml = @"
        <Variables>
          <Variable name=""lastName"" value=""Simpson"" />
        </Variables>
        
        <Table name=""Customer"">
          <Default Notes=""Family"" />
          <Row CustomerName=""Homer $(lastName)"" />
          <Row CustomerName=""Marge $(lastName)"" />
          <Row CustomerName=""Bart $(lastName)"" Notes=""I do not belong here""/>
          <Row CustomerName=""Lisa $(lastName)"" />
          <Row CustomerName=""Maggie $(lastName)"" />
        </Table>
       ";

      InsertManager im = new InsertManager();
      im.RunInserts(strXml);

      Assert.AreEqual(5, DataUtilities.GetTableRowCount("Customer"));

      /* FYI, data will look something like this:
        CustomerName   Notes                LastUpdate
        ------------   -----                ----------
        Bart Simpson   I do not belong here 2000-01-01
        Homer Simpson  Family               2000-01-01
        Lisa Simpson   Family               2000-01-01
        Maggie Simpson Family               2000-01-01
        Marge Simpson  Family               2000-01-01
      */
    }


    [TestMethod]
    public void Trick_Variable_Defaults_2()
    {
      //You can have multiple variable sections
      //You can have multiple default rows.

      string @strXml = @"
              <Variables>
                <Variable name=""lastName"" value=""Simpson"" />
              </Variables>

              <Table name=""Customer"">

                <Default Notes=""Human - adult"" />
                <Row CustomerName=""$(name1) $(lastName)"" />

                <Default Notes=""Human - child"" LastUpdate=""06/21/1997"" />
                <Row CustomerName=""$(name2) $(lastName)""/>

              </Table>

              <Variables>
                <Variable name=""name1"" value=""Homer"" />
                <Variable name=""name2"" value=""Bart"" />
              </Variables>
             ";

      InsertManager im = new InsertManager();
      im.RunInserts(strXml);

      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Customer"));

      /* FYI, data will look something like this:
        CustomerName    Notes           LastUpdate
        ------------    -----           ----------
        Bart Simpson    Human - child   1997-06-21
        Homer Simpson   Human - adult   2000-01-01
      */

    }


    /// <summary>
    /// One of the biggest problems with insert statements is handling the identity values. You'll insert a parent row, and will then 
    /// need to capture the newly-generated primary key (often an identity value) so that you can insert rows into the child tables. 
    /// This gets very tedious, so MDH provides an easy way to handle it: use the "@" lookup operator.
    /// 
    /// The syntax is: [TableName].@N
    /// 
    /// The MDH saves the identity value of every row it inserts. So say you've inserted into the Customer and 
    /// Product tables, and then need both their Primary key (identity) values to insert into the child table 
    /// Order. When you specify CustomerId="Customer.@N", where N is a row (1, 2, 3, etc....), the framework 
    /// is smart enough to check the Customer table, lookup the Nth row's value, and then automatically insert that.
    /// </summary>
    [TestMethod]
    public void Identity_Lookups()
    {
      string strXml = @"
        <Table name=""Customer"">
          <Row CustomerName=""Homer Simpson"" />
          <Row CustomerName=""Marge Simpson"" />
        </Table>

        <Table name=""Product"">
          <Row ProductName=""KrustBurger"" Description=""best burger ever"" />
        </Table>

        <Table name=""Order"">
          <Default LastUpdate=""12/23/1997"" />
          <Row CustomerId=""Customer.@1"" ProductId=""Product.@1"" />
          <Row CustomerId=""Customer.@2"" ProductId=""Product.@1"" />
        </Table>
      ";

      /* There are a few special notes on Identity Lookups:
          'N' is 1 based. So CBankId=@1 will return the first row. 
          Variables are evaluated before identity lookups, so you could have a variable store an identity value. 
          You can self-table reference a table. For example, TableA's row two can reference row one's identity value via "TableA.@1". 
      */

      InsertManager im = new InsertManager();
      im.RunInserts(strXml);

      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Customer"));
      Assert.AreEqual(1, DataUtilities.GetTableRowCount("Product"));
      Assert.AreEqual(2, DataUtilities.GetTableRowCount("Order"));

      //Ensure that database values match
      int intPK = (Int32)DataUtilities.ExecuteScalar("select ProductId from Product");
      int intFK = (Int32)DataUtilities.ExecuteScalar("select top 1 ProductId from [Order]");
      Assert.AreEqual(intPK, intFK);
    }

    #endregion

    #region Store xml in a file

    /// <summary>
    /// Not all data can just be a C# literal string. Sometime you'll want to store your
    /// xml fragment in an physical file instead. You can do that.
    /// NOTE: If the xml fragment is an xml file, then (unlike the C# literal string) it 
    /// needs to be enclosed in a 'Root' element.
    /// </summary>
    [TestMethod]
    public void Get_Data_From_Xml_File()
    {
      //1 - Add the file as an embedded resource. (click the file, view its properties, set BuildAction = "Embedded Resource")
      //2 - use ReflectionUtilities to read that file's contents into a string
      //3 - run InsertManager.RunInserts like normal, using the XmlDocument overload.

      //This requires the namespace, where folders are treated like a step in the namespace. Therefore
      //  this data is stored in the "SampleFile.xml" file of the "XmlScripts" folder.
      string strXml = ReflectionUtilities.GetEmbeddedResourceContent("XmlScripts.SampleFile.xml");
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(strXml);

      InsertManager im = new InsertManager();
      im.RunInserts(xDoc);

      Assert.IsTrue(DataUtilities.TableHasRows("Customer"));

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
