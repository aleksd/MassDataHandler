using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Collections.Specialized;

namespace MassDataHandler.Core.Table
{
  public class GenericSchemaTable : TableBase
  {

    public GenericSchemaTable(string strPrimaryKey, string strTableName, string strIdentityColumn)
    {
      this.PrimaryKey = strPrimaryKey;
      this.TableName = strTableName;
      this.IdentityColumn = strIdentityColumn;
    }

    #region Implement Abstract Methods

    protected override void SetMinimalInsertValues()
    {
      ////Set required columns (besides PK) in table
    }

    protected override void SetDataColumns()
    {
      ////Set schema for all columns in table
    }

    protected override void SetForeignKeyColumns()
    {
      //TODO: not implementing yet.
      ForeignKey[] fKeys = new ForeignKey[0];
      this.ForeignKeys = fKeys;
    }

    #endregion

  }
}
