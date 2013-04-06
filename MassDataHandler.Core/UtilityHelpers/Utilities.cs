using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data;
 
namespace MassDataHandler. Core
{
  public sealed class Utilities
  {

    #region StringDictionary

    /// <summary>
    ///   Merge sd2 into sd1. Sd2 will override sd1.
    /// </summary>
    /// <param name="sd1"></param>
    /// <param name="sd2"></param>
    /// <returns></returns>
    public static StringDictionary MergeStringDictionaries(StringDictionary sd1, StringDictionary sd2)
    {
      StringDictionary sdNew = new StringDictionary();

      if (sd1 == null)
        sd1 = new StringDictionary();
      if (sd2 == null)
        sd2 = new StringDictionary();

      foreach (DictionaryEntry de in sd1)
      {
        string strKey = de.Key.ToString();
        sdNew[strKey] = sd1[strKey];
      }

      foreach (DictionaryEntry de in sd2)
      {
        string strKey = de.Key.ToString();
        sdNew[strKey] = sd2[strKey];
      }

      return sdNew;
    }

    public static string CollapseStringDictionaryToString(StringDictionary sd)
    {
      if (sd == null)
        return null;

      StringBuilder sb = new StringBuilder();
      foreach (DictionaryEntry de in sd)
      {
        string strName = de.Key.ToString();
        string strVal = de.Value.ToString();
        sb.Append(strName + "=" + strVal + "; ");
      }
      return sb.ToString();
    }

    public static StringDictionary PopulateSDFromXmlAttributes(XmlNode n)
    {
      //  <Default CBankId="@1" col4="xyz" />
      StringDictionary sd = new StringDictionary();

      //cycle through all node's attributes, and assign it to Xml
      foreach (XmlAttribute xa in n.Attributes)
      {
        string strName = xa.Name;
        string strValue = xa.Value;
        sd.Add(strName, strValue);
      }

      return sd;
    }

    #endregion

    #region DataTable

    /// <summary>
    ///   Makes columns match actual types.
    /// </summary>
    /// <param name="dr"></param>
    /// <param name="sd"></param>
    /// <returns></returns>
    public static DataRow CreateTypedDataRowFromSD(DataRow dr, StringDictionary sd)
    {
      foreach (DictionaryEntry de in sd)
      {
        string strKey = Convert.ToString(de.Key);
        string strValue = Convert.ToString(de.Value);

        //minimal value key may not be in the DataRow if user requested only some of the columns.
        if (dr.Table.Columns[strKey] != null)
        {
          Type t = Type.GetType("System." + dr.Table.Columns[strKey].DataType.Name);
          dr[strKey] = TypeUtilities.CastStringToType(t, strValue);
        }
      }

      return dr;
    }

    /// <summary>
    ///   Assumes all columns are strings.
    /// </summary>
    /// <param name="dr"></param>
    /// <param name="sd"></param>
    /// <returns></returns>
    public static DataRow CreateDataRowFromSD(DataRow dr, StringDictionary sd)
    {
      foreach (DictionaryEntry de in sd)
      {
        string strKey = Convert.ToString(de.Key);
        string strValue = Convert.ToString(de.Value);
        dr[strKey] = strValue;
      }

      return dr;
    }

    public static DataTable CreateTableSchemaFromXmlNode(XmlNode xNodeTable)
    {
      string strTable = Utilities.GetXmlAttributeValue(xNodeTable, "name");
      DataTable dt = new DataTable(strTable);

      if (xNodeTable.ChildNodes.Count == 0)
        return dt;  //no rows

      //add columns
      foreach (XmlAttribute xa in xNodeTable.ChildNodes[0].Attributes)
      {
        string strColName = xa.Name;
        DataColumn dc = new DataColumn(strColName, typeof(String));
        dt.Columns.Add(dc);
      }

      return dt;
    }

    #endregion

    #region Xml

    public static string GetXmlAttributeValue(XmlNode n, string strAttribute)
    {
      if (n.Attributes[strAttribute] == null)
        return null;
      else
        return n.Attributes[strAttribute].Value;
    }

    #endregion

    public static string Trim(string strVal)
    {
      if (strVal == null)
        return strVal;
      else
        return strVal.Trim();
    }
  }
}
