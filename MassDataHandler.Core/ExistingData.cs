using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.Specialized;

namespace MassDataHandler.Core
{
  public class ExistingData
  {
    #region Constructors

    public ExistingData()
    {

    }

    #endregion

    #region CreateXml

    public string CreateXml(SelectionStrategy objSS)
    {
      if (objSS.IsCustomTableSelect)
      {
        //specified customSelect --> returns 1 table.
        string[] strTable = new string[1] { objSS.SourceSchemaObject.ObjectName };
        return CreateXml(strTable, objSS);
      }
      else if (objSS.SourceSchemaObject.SchemaType == SchemaObject.SchemaObjectType.StoredProcedure)
      {
        //StoredProcedure --> many tables
        return CreateXml(objSS.SourceSchemaObject.ObjectName, objSS);
      }
      else
      {
        //Table (accepts a CSV list)
        string[] strTable = ArrayUtilities.SplitCSVToArray(objSS.SourceSchemaObject.ObjectName);
        return CreateXml(strTable, objSS);
      }
    }

    public string CreateXml(string strSpName, SelectionStrategy objSS)
    {
      string[] astrTables = DataUtilities.Schema.GetDependentTablesForStoredProc(strSpName, objSS.DatabaseConnectionString);
      return CreateXml(astrTables, objSS);
    }

    public string CreateXml(string[] astrTables, SelectionStrategy objSS)
    {
      StringBuilder sb = new StringBuilder();

      string strSerializedCustomStrategy = "";
      if (objSS.IsCustomTableSelect)
      {
        strSerializedCustomStrategy = "\r\n\t<Meta><![CDATA[\r\n" + objSS.GetSerializedCustomStrategy() + "]]></Meta>\r\n";
      }

      sb.Append("<Root>");
      sb.Append(strSerializedCustomStrategy);

      foreach (string strTable in astrTables)
      {
        sb.Append(CreateTableXml(strTable, objSS));
      }

      sb.Append("</Root>");

      return sb.ToString();
    }

    public string CreateTableXml(string strTable, string strCustomSql)
    {
      SelectionStrategy objSS = new SelectionStrategy();
      objSS.CustomTableFilter = strCustomSql;
      objSS.DatabaseConnectionString = DataUtilities.CreateConnectionStringFromAppConfig();
      return CreateTableXml(strTable, objSS);
    }

    public string CreateTableXml(string strTable, SelectionStrategy objSS)
    {
      /*
        <Table name="Service">
          <Row serviceName="MyService1" Account="1111" />
          <Row serviceName="MyService2" Account="2222" />
        </Table>
       */

      //Hit DB for actual data
      bool blnTableHasCompanyColumn = DataUtilities.Schema.DoesTableContainColumn(strTable, "co", objSS.DatabaseConnectionString);
      string strSql = objSS.CreateSqlSelect(strTable, blnTableHasCompanyColumn);

      DataSet ds = DataUtilities.ExecuteDataSetGivenConString(strSql, objSS.DatabaseConnectionString);
      string[] astrTableNames = StringUtilities.SplitCSVToArray(strTable);

      //Ensure that there is a tableName for each select
      if (ds.Tables.Count != astrTableNames.Length)
      {
        throw new Exception(string.Format("There must be one tableName specified for each SQL select statement. There were {0} SQL statements, and {1} tableNames.", ds.Tables.Count.ToString(), astrTableNames.Length.ToString()));
      }

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < ds.Tables.Count; i++)
      {
        DataTable dt = ds.Tables[i];
        dt.TableName = astrTableNames[i];
        sb.Append(CreateTableXml(dt, objSS));
        sb.Append("\r\n\r\n");
      }
      //foreach (DataTable dt in ds.Tables)
      //{
      //  dt.TableName = strTable;
      //  sb.Append(CreateTableXml(dt, objSS));
      //  sb.Append("\r\n\r\n");
      //}
      //DataTable dt = DataUtilities.ExecuteDataTableGivenConString(strSql, objSS.DatabaseConnectionString);
      //dt.TableName = strTable;

      //return CreateTableXml(dt, objSS);
      return sb.ToString();
    }

    public static string CreateTableXml(DataTable dt)
    {
      SelectionStrategy objSS = new SelectionStrategy();
      objSS.DatabaseConnectionString = DataUtilities.CreateConnectionStringFromAppConfig();
      return CreateTableXml(dt, objSS);
    }

    public static string CreateTableXml(DataTable dt, SelectionStrategy objSS)
    {
      Table.TableBase tb = Table.TableBase.CreateTableObjectFromDatabase(dt.TableName, objSS.DatabaseConnectionString);
      if (objSS.EnsureAtLeastOneRow && dt.Rows.Count == 0)
      {
        //Need to add a row
        DataRow dr = dt.NewRow();

        //Create minimal inserts
        //Table.TableBase tb = Table.TableBase.CreateTableObject(dt.TableName);
        StringDictionary sdMin = tb.MinimalInsertValues;
        dr = Utilities.CreateTypedDataRowFromSD(dr, sdMin);

        dt.Rows.Add(dr);
      }

      StringBuilder sb = new StringBuilder();
      sb.Append("<Table name=\"" + dt.TableName + "\">");

      sb.Append(GetRows(dt, objSS, tb));

      sb.Append("</Table>");

      return sb.ToString();
    }

    #endregion

    #region Helper methods

    private static string GetRows(DataTable dt, SelectionStrategy objSS, Table.TableBase tb)
    {
      //  <Row serviceName="MyService1" DefaultPayFromAccount="1111" />
      //cycle through each cell value, generate
      StringBuilder sb = new StringBuilder();
      foreach (DataRow dr in dt.Rows)
      {
        sb.Append("<Row ");
        foreach (DataColumn c in dt.Columns)
        {
          string strColumnName = c.ColumnName;

          if (objSS.IncludeIdentityColumn == false && StringUtilities.AreStringsEqual(strColumnName, tb.IdentityColumn, true, true))
          {
            //this is the identity column, don't include it.
          }
          else
          {
            //Sql encode entity
            string strColumnValue = WrapValue(strColumnName, dr);

            sb.Append(strColumnName + "=\"" + strColumnValue + "\" ");
          }

        }
        sb.Append(" />");
      }

      return sb.ToString();
    }

    private static string WrapValue(string strColumnName, DataRow dr)
    {
      //display NULLS as null
      if (dr[strColumnName] is DBNull)
        return "NULL";

      string strColumnValue = Convert.ToString(dr[strColumnName]);

      //encode:
      strColumnValue = EncodeSqlEntity(strColumnValue);
      strColumnValue = XmlUtilities.EncodeEntity(strColumnValue);

      return strColumnValue;
    }

    private static string EncodeSqlEntity(string strVal)
    {
      //encode out all '
      //strVal = strVal.Replace("'", "''");
      //Version 1.2 Change - Select existing data doesn't encode it, have insert do that instead.
      return strVal;
    }

    #endregion

    #region CreateDatabaseSingleRowFile

    //public static void CreateDatabaseSingleRowFile(string strDbCon, string strOutputFile)
    //{
    //  Console.WriteLine("MassDataHelper.ExistingData:");
    //  Console.WriteLine(" Xml output file: " + strOutputFile);
    //  Console.WriteLine(" DB ConString: " + strDbCon);

    //  /* Get non-Dep tables
    //   * Get Dep tables
    //   */

    //  StringBuilder sb = new StringBuilder();
    //  sb.Append("<Root>\r\n");

    //  //Non-Dependent tables

    //  string[] astrNonDeps = DataUtilities.Schema.GetTableNonDependencies(strDbCon);
    //  sb.Append("\t<!-- - - - - - - - - - - - NON-DEPENDENT TABLES - - - - - - - - - - - -->\r\n");
    //  foreach (string s in astrNonDeps)
    //  {
    //    sb.Append("\t<Table name=\"" + s + "\">\r\n");
    //    sb.Append("\t\t<Row />\r\n");
    //    sb.Append("\t</Table>\r\n\r\n");
    //  }
  
    //  //Dependent tables (consume an identity)
    //  List<TableDependency> tblDeps = DataUtilities.Schema.GetTableDependencies(strDbCon);
    //  tblDeps = DataUtilities.Schema.SortTableDependencies(tblDeps);
    //  sb.Append("\t<!-- - - - - - - - - - - - DEPENDENT TABLES - - - - - - - - - - - -->\r\n");
    //  foreach (TableDependency tblDep in tblDeps)
    //  {
    //    sb.Append("\t<Table name=\"" + tblDep.TableName + "\">\r\n");
    //    sb.Append("\t\t<Row ");
    //    //add all attributes for identities
    //    foreach (ForeignKey fk in tblDep.ForeignKeyList)
    //    {
    //      //  ChildColumn="ParentTable.@1"
    //      if (fk.IsIdentity)
    //      {
    //        sb.Append(fk.ForeignKeyName + "=\"" + fk.PrimaryKeyColumn.TableName + ".@1\" ");
    //      }
    //    }

    //    sb.Append(" />\r\n");
    //    sb.Append("\t</Table>\r\n\r\n");
    //  }

    //  sb.Append("</Root>\r\n");

    //  string strContent = sb.ToString();
    //  FileUtilities.WriteFileContents(strContent, strOutputFile);

    //  Console.WriteLine("SUCCESS: Created output file.");
    //}

    #endregion

  }

}
