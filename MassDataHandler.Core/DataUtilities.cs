using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Specialized;

namespace MassDataHandler.Core
{
  public class DataUtilities
  {

    #region Execute

    /// <summary>
    ///   Returns identity of insert, -1 if there is no identity.
    /// </summary>
    /// <remarks>For performance reasons, this method itself doesn't do the check to see if the table has an identity column. Rather, the caller must first check to see if this table has an identity column. Else, if you both specify IdentiyInserts=On and the table lacks an Indentity column, this method throws an exception. Because A single table could have many rows of data, it is faster performance to do the identityCheck for the one table rather than for each row.</remarks>
    /// <param name="strSql"></param>
    /// <returns></returns>
    internal static int ExecuteInsertReturnIdentity(string strSql, string strDbConString, bool blnIdentityInsertOn, string strTableName)
    {
      if (blnIdentityInsertOn)
        strSql = "SET IDENTITY_INSERT " + strTableName + " ON\r\n" + strSql + "\r\nSET IDENTITY_INSERT " + strTableName + " OFF\r\n";

      //strSql += ";\r\nselect isnull(IDENT_CURRENT( '" + strTableName + "' ), -1)";
      /* Note, three kinds of identity functions:
       *  @@identity --> doesn't handle triggers (i.e. gets trigger's value instead of original table
       *  IDENT_CURRENT( 'Customer' ) --> doesn't handle IdentityInsertOn (i.e. gets last automatically-inserted value, not what was actually hard-coded if insert=On
       *  SCOPE_IDENTITY() --> good for our purposes.
       */

      strSql += ";\r\nselect isnull(SCOPE_IDENTITY(), -1)";
      
      object o = ExecuteScalarGivenConString(strSql, strDbConString);

      return Convert.ToInt32(o);
    }

    #region DeleteTable

    public static int DeleteTable(string strTableName)
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return DeleteTable(strTableName, strDbCon);
    }

    /// <summary>
    ///   Given a string array of tables, delete each one.
    /// Return the number of rows deleted.
    /// </summary>
    /// <param name="strCSVTableName"></param>
    public static int DeleteTable(string[] astrTables)
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return DeleteTable(astrTables, strDbCon);
    }

    public static int DeleteTable(string[] astrTables, string strDbCon)
    {
      int i = 0;
      foreach (string strTable in astrTables)
      {
        i = i + DeleteTable(strTable, strDbCon);
      }
      return i;
    }

    public static int DeleteTable(string strTableName, string strDbCon)
    {
      string strSql = "delete from [" + strTableName + "]";
      int i = ExecuteNonQueryGivenConString(strSql, strDbCon);
      return i;
    }

    public static int DeleteAllTables(string strDbCon)
    {
      string[] astrAllTables = Schema.GetAllTables(strDbCon);
      return DeleteTable(astrAllTables, strDbCon);
    }

    public static int DeleteAllTables()
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return DeleteAllTables(strDbCon);
    }

    #endregion

    public static DataTable ExecuteDataTable(string strSql)
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return ExecuteDataTableGivenConString(strSql, strDbCon);
    }

    public static DataRow ExecuteDataRow(string strSql)
    {
      DataTable dt = ExecuteDataTable(strSql);
      return dt.Rows[0];
    }

    public static object ExecuteScalar(string strSql)
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return ExecuteScalarGivenConString(strSql, strDbCon);
    }

    public static int ExecuteNonQuery(string strSql)
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return ExecuteNonQueryGivenConString(strSql, strDbCon);
    }

    #endregion

    #region Transactions

    public static SqlConnection GetConnectionObject()
    {
      string strDbCon = CreateConnectionStringFromAppConfig();
      return new SqlConnection(strDbCon);
    }

    public static SqlCommand GetCommandObject(string strCmdText, SqlConnection con, SqlTransaction tran)
    {
      SqlCommand cmd = new SqlCommand(strCmdText, con, tran);
      cmd.CommandType = CommandType.Text;

      return cmd;
    }

    /// <summary>
    ///   Runs Sql in a transaction. First this runs the update, then the select. It sends the results
    ///   of the select to a DataTable, and than rolls back the transaction such that no data is changed.
    /// </summary>
    /// <remarks>
    ///   Both Sql statements can be either static Sql, or SP calls.
    ///   To test an Update StoredProc, make strSqlUpdates be the proc and strSqlGetResults be the query to check for results.
    ///   To test a Select StoredProc, optionally use strSqlUpdates to pre-modify the data, then make strSqlGetResults be the exec for the select proc.
    /// </remarks>
    /// <param name="strSqlUpdates">The update (ExecuteNonQuery) to run.</param>
    /// <param name="strSqlGetResults">The select to run, this populates a single DataTable that gets returned.</param>
    /// <returns></returns>
    public static DataTable RunSqlInTransaction(string strSqlUpdates, string strSqlGetResults)
    {
      SqlConnection con = GetConnectionObject();
      DataTable dtOutput = new DataTable();

      using (con)
      {
        // Start a new transaction
        con.Open();
        using (SqlTransaction trans = con.BeginTransaction())
        {
          SqlCommand cmd1 = GetCommandObject(strSqlUpdates, con, trans);
          SqlCommand cmd2 = GetCommandObject(strSqlGetResults, con, trans);

          try
          {
            if (strSqlUpdates != null && strSqlUpdates.Length > 0)
            {
              //Only run the pre-updates if there is something to run.
              cmd1.ExecuteNonQuery();
            }

            if (strSqlGetResults != null && strSqlGetResults.Length > 0)
            {
              //Only run if there's something to try selecting
              SqlDataReader sdr = cmd2.ExecuteReader();
              dtOutput.Load(sdr);
            }

            //Transaction succeeded, still rollback.
            trans.Rollback(); 
          }
          catch (Exception ex)
          {
            // transaction failed
            trans.Rollback();
            throw;
          }

        } //end of sub-using

      } //end of using

      return dtOutput;

    }

    #endregion

    #region Conversion & Analysis Methods

    public static string[] ConvertDataTableToStringArray(DataTable dt)
    {
      string[] astr = new string[dt.Rows.Count];
      for (int i = 0; i < dt.Rows.Count; i++)
      {
        astr[i] = Convert.ToString(dt.Rows[i][0]);
      }

      return astr;
    }

    /// <summary>
    ///   Default: don't ignore case, don't trim.
    ///   Store any errors in string collection. Null implies no errors; everything is valid.
    /// </summary>
    public static StringCollection CompareDataTableAsString(DataTable dt1, DataTable dt2)
    {
      bool blnIgnoreCase = true;
      bool blnTrim = false;

      StringCollection sc = new StringCollection();
      if (dt1 == null && dt2 == null)
      {
        return null;
      }
      else if (dt1 == null && dt2 != null)
      {
        sc.Add("Not Equal: dt1 is null and dt2 is not.");
        return sc;
      }
      else if (dt1 != null && dt2 == null)
      {
        sc.Add("Not Equal: dt2 is null and dt1 is not.");
        return sc;
      }
      else
      {
        //both non-null
        if (dt1.Columns.Count != dt2.Columns.Count)
        {
          sc.Add("Not Equal, Different column counts: dt1 has " + dt1.Columns.Count.ToString() + " column(s), and dt2 has " + dt1.Columns.Count.ToString() + " column(s).");
          return sc;
        }
        else if (dt1.Rows.Count != dt2.Rows.Count)
        {
          sc.Add("Not Equal, Different row counts: dt1 has " + dt1.Rows.Count.ToString() + " row(s), and dt2 has " + dt1.Rows.Count.ToString() + " row(s).");
          return sc;
        }

        //cycle through each cell
        for (int j = 0; j < dt1.Columns.Count; j++)
        {
          string strColName1 = dt1.Columns[j].ColumnName;
          string strColName2 = dt2.Columns[j].ColumnName;
          if (!StringUtilities.AreStringsEqual(strColName1, strColName2, blnIgnoreCase, blnTrim))
          {
            sc.Add("Not Equal, ColumnNames at " + j.ToString() + " are different. dt1='" + strColName1 + "', dt2='" + strColName2 + "'.");
            return sc;
          }

          for (int i = 0; i < dt1.Rows.Count; i++)
          {
            string strVal1 = Convert.ToString(dt1.Rows[i][j]);
            string strVal2 = Convert.ToString(dt2.Rows[i][j]);

            if (!StringUtilities.AreStringsEqual(strVal1, strVal2, blnIgnoreCase, blnTrim))
            {
              sc.Add("Not Equal, Cells at Row=" + i.ToString() + ",Col=" + j.ToString() + " have different values. dt1 has value='" + strVal1 + "', dt2 has value='" + strVal2 + "'.");
              return sc;
            }
          }

        }
      }

      if (sc.Count == 0)
        return null;
      else
        return sc;
    }

    /// <summary>
    ///   Expects a Input object, will create a SP call where each property of object 
    /// corresponds to a sql parameter for the StoredProc. It then makes a DB hit to get any potential output params, and appends
    /// those too.
    /// </summary>
    /// <remarks>
    /// Hits database to get output params
    /// </remarks>
    /// <param name="strStoredProc"></param>
    /// <param name="o">An input object where each property is an input param for a SP.</param>
    /// <returns></returns>
    public static string CreateStoredProcedureCall(string strStoredProc, object o)
    {
      //add any output params
      string[] astrOutputParams = Schema.GetOutputParamsForStoredProcedure(strStoredProc);
      return CreateStoredProcedureCall(strStoredProc, o, astrOutputParams);
    }

    public static string CreateStoredProcedureCall(string strStoredProc, object o, string[] astrOutputParams)
    {
      StringBuilder sb = new StringBuilder();
      StringBuilder sbDeclare = new StringBuilder();

      sb.Append("exec " + strStoredProc + " ");
      Type t = o.GetType();

      if (t == null)
        throw new ArgumentException("Could not get type of input object.");
      else
      {
        PropertyInfo[] api = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo pi in api)
        {
          string strParamName = pi.Name;
          object objVal = pi.GetValue(o, null);

          //can't send in function directly as param, need to evaluate it first. Therefore, if 
          //DateTime, declare and evaluate first.
          //  declare @dstatusDate_1 as datetime
          //  select @dstatusDate_1 = Cast('12/1/2001 12:00:00 AM' as DateTime);
          if (TypeUtilities.GetGeneralType(objVal) == TypeUtilities.GeneralType.DateTime)
          {
            string strTempParamName = "@" + strParamName + "_1";
            sbDeclare.Append("declare " + strTempParamName + " as datetime\r\n");
            sbDeclare.Append("select " + strTempParamName + " = " + SqlTextHelper.WrapTypeDelimiter(objVal) + "\r\n");
            sb.Append("@" + strParamName + "=" + strTempParamName + ", ");
          }
          else
          {
            sb.Append("@" + strParamName + "=" + SqlTextHelper.WrapTypeDelimiter(objVal) + ", ");
          }

        }
      }

      //remove final ", ";
      string strResult = sb.ToString();
      strResult = strResult.Substring(0, strResult.Length - 2);
      sb = new StringBuilder();
      sb.Append(strResult);

      //add any output params
      if (astrOutputParams != null && astrOutputParams.Length > 0)
      {
        sb.Append("\r\n-- Outputs:\r\n");
        //add outputs
        //  ,@iId = @iId_1 out
        foreach (string strOutput in astrOutputParams)
        {
          sbDeclare.Append("declare " + strOutput + "_1 int\r\n");
          sb.Append(" ," + strOutput + " = " + strOutput + "_1 out");
        }
        strResult = sb.ToString();
      }

      strResult = sbDeclare.ToString() + "\r\n" + sb.ToString();
      return strResult;
    }

    #endregion

    #region Advanced Get

    public static string LookupDataTableCell(DataTable dt, string strSearchColumnName, string strSearchValue, string strLookupColumn)
    {
      DataRow dr = LookupDataRow(dt, strSearchColumnName, strSearchValue);
      if (dr == null)
        return null;
      else
        return Convert.ToString(dr[strLookupColumn]);

    }

    public static DataRow LookupDataRow(DataTable dt, string strSearchColumnName, string strSearchValue)
    {
      if (dt == null || dt.Columns.Count == 0 || !dt.Columns.Contains(strSearchColumnName))
        return null;

      foreach (DataRow dr in dt.Rows)
      {
        if (Convert.ToString(dr[strSearchColumnName]).ToLower() == strSearchValue.ToLower())
          return dr;
      }

      return null;
    }

    public static bool TableHasRows(string strTable)
    {
      return (GetTableRowCount(strTable) > 0);
    }

    public static int GetTableRowCount(string strTable)
    {
      string strSelect = "select count(*) from [" + strTable + "]";
      return Convert.ToInt32(ExecuteScalar(strSelect));
    }

    public static int GetSelectRowCount(string strSelect)
    {
      DataTable dt = ExecuteDataTable(strSelect);
      return dt.Rows.Count;
    }

    public static string GetTableFieldValue(string strTable, string strField, int intId)
    {
      return Convert.ToString(
        ExecuteScalar(
        "select " + strField + " from " + strTable + @" where " + strTable + "Id = " + intId.ToString()
        ));
    }

    #endregion

    #region Schema

    public sealed class Schema
    {
      private Schema()
      {
      }

      public static bool DoesTableExist(string strTableName, string strDbCon)
      {
        string strSql = @"
          select count(*) from sys.tables
          where [name] = '" + strTableName + @"'
        ";
        int intCount = Convert.ToInt32( DataUtilities.ExecuteScalarGivenConString(strSql, strDbCon));
        return intCount > 0; //count should be either 0 or 1, (can't be duplicate table names)

      }

      public static string[] GetAllTables(string strDbCon)
      {
        string strSql = @"
          select [name] from sys.tables
          order by [name]
        ";

        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);
        string[] astrTables = ConvertDataTableToStringArray(dt);
        return astrTables;
      }

      public static string[] GetDependentTablesForStoredProc(string strSpName, string strDbCon)
      {
        string strSql = @"
        select distinct T.[name]
        from sys.procedures P, sys.sql_dependencies D, Sys.tables T
        where D.object_id = P.object_id
        and D.referenced_major_id = T.object_id
        and P.[name] = '" + strSpName + @"'
        ";

        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);
        string[] astrTables = ConvertDataTableToStringArray(dt);
        return astrTables;
      }

      public static bool DoesTableContainColumn(string strTable, string strColumn, string strDbCon)
      {
        string strSql = @"select 1 from syscolumns where name = '" + strColumn + "' and id in (select id from sysobjects where name = '" + strTable + "')";
        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);
        if (dt.Rows.Count == 0)
          return false;
        else
          return true;
      }

      public static string[] GetAllColumnsForTable(string strTable, string strDbCon)
      {
        string strSql = "select [name] from syscolumns where id in (select id from sysobjects where name = '" + strTable + "')";
        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);
        string[] astrColumns = ConvertDataTableToStringArray(dt);
        return astrColumns;
      }

      /// <summary>
      ///   returns output params of the form '@myName'
      /// </summary>
      /// <param name="strSpName"></param>
      /// <returns></returns>
      public static string[] GetOutputParamsForStoredProcedure(string strSpName)
      {
        string strDbCon = CreateConnectionStringFromAppConfig();
        return GetOutputParamsForStoredProcedure(strSpName, strDbCon);
      }
      public static string[] GetOutputParamsForStoredProcedure(string strSpName, string strDbCon)
      {
        string strSql = @"
        select P.[name] 
        from sys.all_Parameters P, sys.procedures S
        where P.object_id =  S.object_id
        and S.name='" + strSpName + @"'
        and is_output = 1";

        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);
        string[] astrColumns = ConvertDataTableToStringArray(dt);
        return astrColumns;
      }

      public static string GetPrimaryKey(string strTableName, string strDbCon)
      {
        string strSql = @"
        select c.name as COLUMN_NAME
        from sys.key_constraints as k
        join sys.tables as t
            on t.object_id = k.parent_object_id
        join sys.index_columns as ic
            on ic.object_id = t.object_id
           and ic.index_id = k.unique_index_id
        join sys.columns as c
            on c.object_id = t.object_id
           and c.column_id = ic.column_id
        where t.name = '" + strTableName + @"'
        ";

        string s = Convert.ToString(DataUtilities.ExecuteScalarGivenConString(strSql, strDbCon));
        return s;
      }

      /// <summary>
      ///   Returns true if the given table has an identity column, else false.
      /// </summary>
      /// <param name="strTableName"></param>
      /// <param name="strDbCon"></param>
      /// <returns></returns>
      public static bool HasIdentityColumn(string strTableName, string strDbCon)
      {
        string s = GetIdentityColumn(strTableName, strDbCon);
        return !string.IsNullOrEmpty(s);
      }

      /// <summary>
      ///   Returns an empty string if the table has no Identity column or if the table does not exist.
      /// </summary>
      /// <param name="strTableName"></param>
      /// <param name="strDbCon"></param>
      /// <returns></returns>
      public static string GetIdentityColumn(string strTableName, string strDbCon)
      {
        string strSql = @"
          select C1.Name from Sys.tables T1, Sys.columns C1
          where C1.object_Id = T1.object_id
          and C1.is_identity = 1
          and T1.Name = '" + strTableName + @"'
        ";

        string s = Convert.ToString(DataUtilities.ExecuteScalarGivenConString(strSql, strDbCon));
        return s;
      }

      public static DataTable GetColumnSchemaInfo(string strTableName, string strDbCon)
      {
        string strSql = @"
          select
            C1.Name as ""ColumnName"", 
            TP.name as ""SystemTypeName"", 
            C1.is_Nullable, 
            C1.max_length, 
            C1.is_Identity 
          from Sys.tables T1, Sys.columns C1, Sys.types TP
          where
            C1.object_Id = T1.object_id
            and C1.user_type_id = TP.user_type_id
            and TP.name != 'timestamp'
          and T1.Name = '" + strTableName + @"'
          order by C1.name
        ";

        DataTable dt = DataUtilities.ExecuteDataTableGivenConString(strSql, strDbCon);

        //modify type here
        if (dt == null || dt.Rows.Count == 0)
          return dt;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
          string strType = Convert.ToString(dt.Rows[i]["SystemTypeName"]);
          dt.Rows[i]["SystemTypeName"] = TypeUtilities.ConvertSqlDataType(strType);
        }

        return dt;

      }

      public static string[] GetTableNonDependencies(string strDbCon)
      {
        string strSql = @"
          select T1.[name] from Sys.tables T1
          where T1.[Name] not in 
          (
              select 
              T1.[Name] as ""ChildTable""
              from 
                sys.foreign_key_columns FC, 
                Sys.tables T1, Sys.tables T2,
                Sys.columns C1, Sys.columns C2
              where FC.parent_object_id = T1.object_id
              and FC.referenced_object_id = T2.object_id
              and C1.object_Id = T1.object_id and C1.column_id = FC.Parent_column_id
              and C2.object_Id = T2.object_id and C2.column_id = FC.Referenced_column_id
              --and C2.is_identity = 1
          )
          order by T1.[name]
         ";

        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);
        string[] astr = DataUtilities.ConvertDataTableToStringArray(dt);

        return astr;

      }


      #region Table Dependencies

      public static List<TableDependency> GetTableDependencies(string strDbCon)
      {
        string strSql = @"
                select 
                C2.is_identity as ""IsIdentity"",
                T1.[Name] as ""ChildTable"",
                C1.[Name] as ""ChildColumn"",  
                T2.[Name] as ""ParentTable"",
                C2.[Name] as ""ParentColumn""
                from 
                  sys.foreign_key_columns FC, 
                  Sys.tables T1, Sys.tables T2,
                  Sys.columns C1, Sys.columns C2
                where FC.parent_object_id = T1.object_id
                and FC.referenced_object_id = T2.object_id
                and C1.object_Id = T1.object_id and C1.column_id = FC.Parent_column_id
                and C2.object_Id = T2.object_id and C2.column_id = FC.Referenced_column_id
                --and C2.is_identity = 1
                order by T1.[Name]
                ";

        DataTable dt = ExecuteDataTableGivenConString(strSql, strDbCon);

        //Convert DT to list
        List<TableDependency> tblDeps = new List<TableDependency>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
          //for each group of tables (where a table can have multiple rows), create object
          string strTableName = Convert.ToString(dt.Rows[i]["ChildTable"]);
          TableDependency tblDep = new TableDependency(strTableName);

          int j = i;
          string strTableNameTemp = Convert.ToString(dt.Rows[j]["ChildTable"]);

          while (strTableName == strTableNameTemp)
          {
            string strFkColumn = Convert.ToString(dt.Rows[j]["ChildColumn"]);
            string strParentTable = Convert.ToString(dt.Rows[j]["ParentTable"]);
            string strParentColumn = Convert.ToString(dt.Rows[j]["ParentColumn"]);
            bool blnIsIdentity = Convert.ToBoolean(dt.Rows[j]["IsIdentity"]);

            TableColumn tc = new TableColumn(strParentTable, strParentColumn);
            ForeignKey fk = new ForeignKey(strFkColumn, tc);
            fk.IsIdentity = blnIsIdentity;

            tblDep.AddDependentTable(fk);
            j++;

            if (j >= dt.Rows.Count)
              strTableNameTemp = null;    //continue;
            else
              strTableNameTemp = Convert.ToString(dt.Rows[j]["ChildTable"]);
          }

          i = j - 1;

          tblDeps.Add(tblDep);
        }

        return tblDeps;
      }

      public static List<TableDependency> SortTableDependencies(List<TableDependency> tblDeps)
      {
        /* Create two sets: 
         *  R = Remaining (starts with all, ends with none)
         *  S = Sorted (starts with none, ends with all)
         * Find next Parent (PK) not used in R's children
         */

        List<TableDependency> tblRemaining = tblDeps;
        List<TableDependency> tblSorted = new List<TableDependency>();

        while (tblRemaining.Count > 0)
        {
          //find next parent
          int intNextParentTable = FindNextNotUsed(tblRemaining);
          TableDependency tbl = tblRemaining[intNextParentTable];

          //remove from Remaining, add to Sort
          tblRemaining.RemoveAt(intNextParentTable);

          tblSorted.Add(tbl);
        }

        return tblSorted;
      }

      private static int FindNextNotUsed(List<TableDependency> tblRemaining)
      {
        for (int i = 0; i < tblRemaining.Count; i++)
        {
          bool blnIsUsed = false;

          foreach (ForeignKey fk in tblRemaining[i].ForeignKeyList)
          {
            string strParent = fk.PrimaryKeyColumn.TableName;
            for (int j = 0; j < tblRemaining.Count; j++)
            {
              string strChild = tblRemaining[j].TableName;

              if (StringUtilities.AreStringsEqual(strParent, strChild))
              {
                //ParentTable is used by Child, therefore it's not valid. continue to next parentTable.
                blnIsUsed = true;
                break;
              }
              //ParentTable never used.
            }

            if (blnIsUsed)
              break;  //If ParentTable is referenced by even 1 FK, then it is still used.

          } //end of for - FK

          if (!blnIsUsed)
            return i; //A ParentTable was never used, found it!

        }

        return -1;  //All parent tables were used, this is an error.
      }

      #endregion

    }

    #endregion

    #region Get DB connection from App.Config

    public static string CreateConnectionStringFromAppConfig()
    {
      return GetAppKey("DBConnectionString");
    }

    public static string GetAppKey(string strKey)
    {
      return System.Configuration.ConfigurationManager.AppSettings[strKey].ToString();
    }

    #endregion

    #region Specify Con String

    public static DataSet ExecuteDataSetGivenConString(string strText, string strDbCon)
    {
      DataSet ds = new DataSet();
      SqlConnection con = null;
      SqlCommand cmd = null;

      try
      {
        con = new SqlConnection(strDbCon);
        cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strText;

        SqlDataAdapter oleDa = new SqlDataAdapter(cmd);

        if (cmd.Connection.State == ConnectionState.Closed)
          cmd.Connection.Open();
        oleDa.Fill(ds);

      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        if (con != null)
          con.Close();
      }

      return ds;

    }


    public static DataTable ExecuteDataTableGivenConString(string strText, string strDbCon)
    {
      DataTable dt = new DataTable();
      SqlConnection con = null;
      SqlCommand cmd = null;

      try
      {
        con = new SqlConnection(strDbCon);
        cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strText;

        SqlDataAdapter oleDa = new SqlDataAdapter(cmd);

        if (cmd.Connection.State == ConnectionState.Closed)
          cmd.Connection.Open();
        oleDa.Fill(dt);

      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        if (con != null)
          con.Close();
      }

      return dt;

    }

    public static int ExecuteNonQueryGivenConString(string strText, string strDbCon)
    {
      int intResult = -1;
      SqlConnection con = null;
      SqlCommand cmd = null;

      try
      {
        con = new SqlConnection(strDbCon);
        cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strText;

        if (cmd.Connection.State == ConnectionState.Closed)
          cmd.Connection.Open();

        intResult = cmd.ExecuteNonQuery();

      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        if (con != null)
          con.Close();
      }

      return intResult;
    }

    public static object ExecuteScalarGivenConString(string strText, string strDbCon)
    {
      object objResult = null;
      SqlConnection con = null;
      SqlCommand cmd = null;

      try
      {
        con = new SqlConnection(strDbCon);
        cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strText;

        if (cmd.Connection.State == ConnectionState.Closed)
          cmd.Connection.Open();

        objResult = cmd.ExecuteScalar();

      }
      catch (Exception ex)
      {
        throw;
      }
      finally
      {
        if (con != null)
          con.Close();
      }

      return objResult;
    }

    /// <summary>
    ///   Null = success (no error to report)
    ///   non-Null = error message
    /// </summary>
    /// <param name="strCon"></param>
    /// <returns></returns>
    public static string IsConnectionValid(string strCon)
    {
      //Test DB connection
      SqlConnection con = null;
      try
      {
        con = new SqlConnection(strCon);
        con.Open();
        return null;
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
      finally
      {
        if (con != null)
          con.Close();
      }
    }

    #endregion

  }
}
