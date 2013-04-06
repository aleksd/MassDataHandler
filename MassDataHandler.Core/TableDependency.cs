using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class TableDependency
  {

    public TableDependency(string strTableName)
    {
      _strTableName = strTableName;
      _listForeignKeys = new List<ForeignKey>();
    }

    #region Public Properties

    private string _strTableName;
    public string TableName
    {
      get { return _strTableName; }
      set { _strTableName = value; }
    }

    private List<ForeignKey> _listForeignKeys;
    public List<ForeignKey> ForeignKeyList
    {
      get { return _listForeignKeys; }
      set { _listForeignKeys = value; }
    }
	
    #endregion

    public void AddDependentTable(ForeignKey fk)
    {
      _listForeignKeys.Add(fk);
    }

    public override string ToString()
    {
      return "{ChildTableName=" + this.TableName + ", ForeignKey Count=" + this.ForeignKeyList.Count.ToString() + "}";
    }
  }
}
