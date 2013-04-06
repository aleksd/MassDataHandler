using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class Cell
  {
    public Cell(TableColumn tc, int intRow)
    {
      _tc = tc;
      _intRow = intRow;
    }

    #region Public Properties

    private TableColumn _tc;
    public TableColumn TableColumnInfo
    {
      get { return _tc; }
      set { _tc = value; }
    }

    private int _intRow;

    public int RowIndex
    {
      get { return _intRow; }
      set { _intRow = value; }
    }
	
    #endregion

  }
}

