using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class TableColumn
  {
    public TableColumn(string strTableName, string strColumnName)
    {
      _strTableName = strTableName;
      _strColumnName = strColumnName;
    }

    #region Public Properties

    private string _strTableName;
    public string TableName
    {
      get { return _strTableName; }
      set { _strTableName = value; }
    }

    private string _strColumnName;
    public string ColumnName
    {
      get { return _strColumnName; }
      set { _strColumnName = value; }
    }

    #endregion

    public override string ToString()
    {
      return "{Table=" + this.TableName + ", Column=" + this.ColumnName + "}";
    }
  }
}
