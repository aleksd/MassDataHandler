using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class ForeignKey
  {
    public ForeignKey(string strForeignKeyName, TableColumn primaryKeyColumn)
    {
      _strForeignKeyName = strForeignKeyName;
      _tc = primaryKeyColumn;
    }

    #region Public Properties

    private bool _blnIsIdentity;
    public bool IsIdentity
    {
      get { return _blnIsIdentity; }
      set { _blnIsIdentity = value; }
    }

    private string _strForeignKeyName;
    public string ForeignKeyName
    {
      get { return _strForeignKeyName; }
      set { _strForeignKeyName = value; }
    }

    private TableColumn _tc;
    public TableColumn PrimaryKeyColumn
    {
      get { return _tc; }
      set { _tc = value; }
    }
	
    #endregion

    public override string ToString()
    {
      return "{ForeignKey=" + this.ForeignKeyName + ", PrimaryKey=" + this.PrimaryKeyColumn.ToString() + "}";
    }
  }

}
