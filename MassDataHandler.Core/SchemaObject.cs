using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class SchemaObject
  {
    public SchemaObject()
    {

    }

    public SchemaObject(string strObjectName, SchemaObjectType doType)
    {
      _strObjectName = strObjectName;
      _doType = doType;
    }

    #region Public Properties

    private string _strObjectName;
    public string ObjectName
    {
      get { return _strObjectName; }
      set { _strObjectName = value; }
    }

    private SchemaObjectType _doType;
    public SchemaObjectType SchemaType
    {
      get { return _doType; }
      set { _doType = value; }
    }
	

    #endregion

    public enum SchemaObjectType
    {
      StoredProcedure,
      Table
    }

    public string GetSelects(string strDbCon)
    {
      string[] astrTables = null;
      string strHeader = null;
      if (this.SchemaType == SchemaObjectType.Table)
      {
        //split out possible CSV list
        astrTables = ArrayUtilities.SplitCSVToArray(this.ObjectName);
        strHeader = "Table(s)";
      }
      else
      {
        astrTables = DataUtilities.Schema.GetDependentTablesForStoredProc(this.ObjectName, strDbCon);
        strHeader = "Table(s) used in StoredProcedure '" + this.ObjectName + "'";
      }

      return GetSelects(strHeader, astrTables);
    }

    private string GetSelects(string strHeader, string[] astrTables)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("-- " + astrTables.Length.ToString() + " " + strHeader + ":\r\n");
      sb.Append("-- SELECTS:\r\n");
      foreach (string s in astrTables)
      {
        sb.Append("select * from " + s + "\r\n");
      }
      sb.Append("\r\n");
      sb.Append("-- DELETES:\r\n");
      foreach (string s in astrTables)
      {
        sb.Append("delete from " + s + "\r\n");
      }

      return sb.ToString();
    }
  }
}

