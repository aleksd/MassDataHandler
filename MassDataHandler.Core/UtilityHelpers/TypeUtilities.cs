using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
 
namespace MassDataHandler.Core
{

  public sealed class TypeUtilities
  {
    private TypeUtilities()
    {

    }

    #region Primitive Type wrappers

    ///// <summary>
    /////   Takes an .Net class name for a type (like Int32) and returns if it is numeric.
    ///// Note, this will fail for C# names like "int" as opposed to CLR
    ///// </summary>
    ///// <param name="strTypeName"></param>
    ///// <returns></returns>
    //public static bool IsNumber(string strTypeName)
    //{
    //  return (GetGeneralType(strTypeName) == GeneralType.Number);
    //}

    //public static bool IsText(string strTypeName)
    //{
    //  return (GetGeneralType(strTypeName) == GeneralType.Text);
    //}

    //public static bool IsBoolean(string strTypeName)
    //{
    //  return (GetGeneralType(strTypeName) == GeneralType.Boolean);
    //}

    //public static bool IsDateTime(string strTypeName)
    //{
    //  return (GetGeneralType(strTypeName) == GeneralType.DateTime);
    //}

    public static GeneralType GetGeneralType(object obj)
    {
      if (obj == null)
        return GeneralType.Other;
      return GetGeneralType(obj.GetType().Name);
    }

    public static GeneralType GetGeneralType(string strTypeName)
    {
      if (strTypeName == null)
        return GeneralType.Other;

      switch (strTypeName.ToUpper())
      {
        case "BYTE":
        case "SBYTE":
        case "INT16":
        case "INT32":
        case "INT64":
        case "UINT16":
        case "UINT32":
        case "UINT64":
        case "SINGLE":
        case "DOUBLE":
        case "DECIMAL":
        case "INTPTR":
        case "UINTPTR":
          return GeneralType.Number;
        case "STRING":
        case "CHAR":
          return GeneralType.Text;
        case "BOOLEAN":
          return GeneralType.Boolean;
        case "DATETIME":
          return GeneralType.DateTime;
        default:
          return GeneralType.Other; //Includes 'OBJECT'
      }
    }

    public enum GeneralType
    {
      Number,
      Text,
      Boolean,
      DateTime,
      Other
    }

    public static object CastStringToType(Type t, string strVal)
    {
      if (t == null)
        throw new ArgumentNullException("Type cannot be null.");
      if (strVal == null)
        throw new ArgumentNullException("strVal cannot be null.");

      //handle boolean
      object objNew = null;
      if (t == typeof(Boolean))
        return ConverToBoolean(strVal);
      else
        objNew = Convert.ChangeType(strVal, t);

      return objNew;
    }

    public static object CastStringToType(object objTarget, string strVal)
    {
      if (objTarget == null)
        throw new ArgumentNullException("objTarget cannot be null.");
      return CastStringToType(objTarget.GetType(), strVal);
    }

    public static bool ConverToBoolean(string strVal)
    {
      strVal = strVal.ToUpper();

      if (strVal == "TRUE" || strVal == "1")
        return true;
      else if (strVal == "FALSE" || strVal == "0")
        return false;
      else
        throw new ArgumentException("Cannot convert '" + strVal + "' to Boolean.");
    }

    public static string ConvertSqlDataType(string strSqlType)
    {
      if (strSqlType == null || strSqlType.Length == 0)
        return strSqlType;

      switch (strSqlType.ToLower())
      {
        case "bit":
          return "Boolean";

        case "char":
          return "String";

        case "datetime":
          return "DateTime";

        case "decimal":
          return "Decimal";

        case "float":
          return "Decimal";

        case "image":
          return "Byte[]";

        case "int":
          return "Int32";

        case "money":
          return "Decimal";

        case "numeric":
          return "Double";

        case "nchar":
          return "String";

        case "ntext":
          return "String";

        case "nvarchar":
          return "String";

        case "real":
          return "Decimal";

        case "smalldatetime":
          return "DateTime";

        case "smallint":
          return "Int16";

        case "bigint":
          return "Int64";

        case "text":
          return "String";

        case "smallmoney":
          return "Decimal";

        case "timestamp":
          return "DateTime";

        case "tinyint":
          return "Byte";

        case "varchar":
          return "String";

        case "uniqueidentifier":
          return "Guid";

        case "sysname":
        case "xml":
          return "String";

        default:
          throw new ArgumentException("Unknown parameter type '" + strSqlType + "', cannot convert to a .Net Type.");
      }
    }

    #endregion

    //public static object CastStringToType(Type t, string strVal)
    //{
    //  if (t == null)
    //    throw new ArgumentNullException("Type cannot be null.");
    //  if (strVal == null)
    //    throw new ArgumentNullException("strVal cannot be null.");

    //  //handle boolean
    //  object objNew = null;
    //  if (t == typeof(Boolean))
    //    return ConverToBoolean(strVal);
    //  else
    //    objNew = Convert.ChangeType(strVal, t);

    //  return objNew;
    //}

    //public static object CastStringToType(object objTarget, string strVal)
    //{
    //  if (objTarget == null)
    //    throw new ArgumentNullException("objTarget cannot be null.");
    //  return CastStringToType(objTarget.GetType(), strVal);
    //}

    //private static bool CanConvertStringToBoolean(string strVal)
    //{
    //  if (strVal == null)
    //    return false;

    //  strVal = strVal.ToUpper();
    //  return (strVal == "TRUE" || strVal == "1" || strVal == "FALSE" || strVal == "0");
    //}

    //public static bool ConverToBoolean(string strVal)
    //{
    //  strVal = strVal.ToUpper();

    //  if (strVal == "TRUE" || strVal == "1")
    //    return true;
    //  else if (strVal == "FALSE" || strVal == "0")
    //    return false;
    //  else
    //    throw new ArgumentException("Cannot convert '" + strVal + "' to Boolean.");
    //}

    //#region Sql Types

    //public static string ConvertSqlDataType(string strSqlType)
    //{
    //  if (strSqlType == null || strSqlType.Length == 0)
    //    return strSqlType;

    //  switch (strSqlType.ToLower())
    //  {
    //    case "bit":
    //      return "Boolean";

    //    case "char":
    //      return "String";

    //    case "datetime":
    //      return "DateTime";

    //    case "decimal":
    //      return "Decimal";

    //    case "float":
    //      return "Decimal";

    //    case "image":
    //      return "Byte[]";

    //    case "int":
    //      return "Int32";

    //    case "money":
    //      return "Decimal";

    //    case "numeric":
    //      return "Double";

    //    case "nchar":
    //      return "String";

    //    case "ntext":
    //      return "String";

    //    case "nvarchar":
    //      return "String";

    //    case "real":
    //      return "Decimal";

    //    case "smalldatetime":
    //      return "DateTime";

    //    case "smallint":
    //      return "Int16";

    //    case "bigint":
    //      return "Int64";

    //    case "text":
    //      return "String";

    //    case "smallmoney":
    //      return "Decimal";

    //    case "timestamp":
    //      return "DateTime";

    //    case "tinyint":
    //      return "Byte";

    //    case "varchar":
    //      return "String";

    //    case "uniqueidentifier":
    //      return "Guid";

    //    case "sysname":
    //    case "xml":
    //      return "String";

    //    default:
    //      throw new ArgumentException("Unknown parameter type '" + strSqlType + "', cannot convert to a .Net Type.");
    //  }
    //}

    //#endregion

  }
}
