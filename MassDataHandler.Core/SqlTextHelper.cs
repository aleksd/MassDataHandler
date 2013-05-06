using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Collections;
using System.Collections.Specialized;
using MassDataHandler.Core.Table;

namespace MassDataHandler.Core
{
  public class SqlTextHelper
  {

    /// <summary>
    ///   Given a collection of column-values, and the table (which contains the column schema), create the appropriate 
    ///   SQL insert statement.
    /// </summary>
    /// <param name="sdValues"></param>
    /// <param name="columnSchema"></param>
    /// <returns></returns>
    public static string CreateSqlInsert(StringDictionary sdValues, TableBase table)
    {
      if (table.Columns.Length == 0)
        return "";

      //build SQL of the form:
      //Insert into [MyTable] (	[co1] )																																																																							
      //Values(	'value1');																																																																							

      StringBuilder sbCol = new StringBuilder("Insert into [" + table.TableName + "] (");
      StringBuilder sbVal = new StringBuilder("Values(");

      
      //cycle through values and create:
      if (sdValues.Count == 0)
      {
        //rare case that there were 0 columns (all non-Pk/non-Identity cols are null). Therefore use first null column
        string strColumn = null;
        string strValue = null;
        for (int i = 0; i < table.Columns.Length; i++)
        {
          if (table.Columns[i].AllowDBNull == true)
          {
            strColumn = table.Columns[i].ColumnName;
            if (StringUtilities.AreStringsEqual(strColumn, table.PrimaryKey) || StringUtilities.AreStringsEqual(strColumn, table.IdentityColumn))
              continue;

            string strType = table.Columns[i].DataType.Name;
            strValue = CreateMinimalData(strType);

            sbCol.Append("\t[" + strColumn + "], ");
            sbVal.Append("\t" + WrapTypeDelimiter(strValue, strColumn, table) + ", ");
            break;
          }
        }
      }
      else
      {
        foreach (DictionaryEntry de in sdValues)
        {
          string strColumn = Convert.ToString(de.Key);
          string strValue = Convert.ToString(de.Value);

          //depending on dataType, format value differently:
          sbCol.Append("\t[" + strColumn + "], ");
          sbVal.Append("\t" + WrapTypeDelimiter(strValue, strColumn, table) + ", ");
        }
      }

      //remove trailing comma
      sbCol.Remove(sbCol.Length - 2, 2);
      sbVal.Remove(sbVal.Length - 2, 2);

      sbCol.Append(" )");
      sbVal.Append(" )");

      //return final SQL statement:
      return sbCol.ToString() + "\r\n" + sbVal.ToString() + ";";
    }

    //private static string RemoveTrailingComma(string str)
    //{
    //  if (str == null || str.Length < 2)
    //    return str;

    //  str = str.Substring(str, str.Length - 2);
    //  return str;
    //}

    public static string WrapTypeDelimiter(object objVal)
    {
      if (objVal == null)
        return "NULL";

      string strVal = objVal.ToString();
      TypeUtilities.GeneralType gType = TypeUtilities.GetGeneralType(objVal);
      return WrapTypeDelimiter(strVal, gType);
    }

    public static string WrapTypeDelimiter(string strVal, TypeUtilities.GeneralType gType)
    {
      if (strVal == null)
        return "NULL";

      switch (gType)
      {
        case TypeUtilities.GeneralType.Text:
          return "'" + strVal + "'";
        case TypeUtilities.GeneralType.DateTime:
          return "Cast('" + strVal + "' as DateTime)";
        default:
          if (strVal.Length == 0)
            return "NULL";
          else if (gType == TypeUtilities.GeneralType.Boolean)
            return ConvertBooleanToSql(strVal);
          else
            return strVal;
      }
    }

    private static string WrapTypeDelimiter(string strVal, string strColumnName, TableBase table)
    {
      DataColumn dt = GetColumnGivenName(strColumnName, table);

      /* Types mapping:
       * NULL --> NULL
       * Varchar, char --> 'x'
       * Date  --> Cast('x' as DateTime)
       * Numeric --> x
       * Bit --> x
       */

      //check for bad data exceptions
      //  can't insert empty string into an Int field (would generate bad SQl, two adjacent commas).
      
      //dt.DataType.is


      if (strVal.ToUpper() == "NULL")
        return "NULL";
      if (strVal.ToUpper() == @"\NULL") //escape out null.
        strVal = strVal.Substring(1);  

      string strType = dt.DataType.Name.ToUpper();
      
      //handle rare cases of non-simple fields
      switch (strType)
      {
        case "GUID":
        case "BYTE[]":
          return "'" + strVal + "'";
      }

      TypeUtilities.GeneralType gType = TypeUtilities.GetGeneralType(strType);
      switch (gType)
      {
        case TypeUtilities.GeneralType.Text:
          return "'" + strVal + "'";
        case TypeUtilities.GeneralType.DateTime:
          return "Cast('" + strVal + "' as DateTime)";
        default:
          //no wrapping text, therefore ensure that there is at least something such that we don't have two adjacent commas.
          if (strVal.Length == 0)
          {
            //no value, and it requires value. --> assume this is null.
            return "NULL";
            //throw new BadDataException(table.TableName + " has column '" + strColumnName + "' of type '" + strType + "', which is a " + gType.ToString() + ". The value for this column is empty, yet columns of this type require at least some value, even if it's 'NULL'.");
          }
          else if (gType == TypeUtilities.GeneralType.Boolean)
          {
            return ConvertBooleanToSql(strVal);
          }

          return strVal;
      }

    }

    public static string ConvertBooleanToSql(string strVal)
    {
      //Converts booleans to SQl 1 or 0
      if (strVal == null)
        throw new ArgumentNullException("Value cannot be null.");

      strVal = strVal.ToUpper();
      if (strVal == "TRUE" || strVal == "T" || strVal == "1")
        return "1";
      else if (strVal == "FALSE" || strVal == "F" || strVal == "0")
        return "0";
      else
        throw new BadDataException("Cannot convert '" + strVal + "' to a boolean.");

    }

    /// <summary>
    ///   Gets the column given the name. Throw exception if not found.
    /// </summary>
    /// <param name="strColumnName"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataColumn GetColumnGivenName(string strColumnName, TableBase table)
    {
      if (strColumnName == null || strColumnName.Length == 0)
        throw new BadSchemaException("ColumnName did't exist.");
      if (table == null || table.Columns == null || table.Columns.Length == 0)
        throw new BadSchemaException("TableName did't exist.");

      foreach (DataColumn dc in table.Columns)
      {
        if (strColumnName.ToLower() == dc.ColumnName.ToLower())
        {
          return dc;
        }
      }
      throw new BadSchemaException("Column '" + strColumnName + "' in table '" + table.TableName + "' not found.");
    }

    public static string CreateSelectString(string[] astrColumns, string strTable)
    {
      return "select " + ArrayUtilities.JoinStringArray(astrColumns, "", "", ", ") + " from " + strTable;
    }

    public static string CreateMinimalData(string strNativeType)
    {
      strNativeType = strNativeType.ToUpper();
      if (strNativeType == "GUID")
        return "'6F9619FF-8B86-D011-B42D-00C04FC964FF'";

      TypeUtilities.GeneralType gType = TypeUtilities.GetGeneralType(strNativeType);
      switch (gType)
      {
        case TypeUtilities.GeneralType.Text:
          return "a";
        case TypeUtilities.GeneralType.Boolean:
          return "0";
        case TypeUtilities.GeneralType.DateTime:
          return "1/1/2000";
        case TypeUtilities.GeneralType.Number:
          return "1";
        default:
          return "0";
      }
    }

    public static string EscapeSqlString(string strSql)
    {
      if (string.IsNullOrEmpty(strSql))
        return strSql;
      else
        return strSql.Replace("'", "''");
    }
  }
}

