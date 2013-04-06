using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace MassDataHandler.Core
{
  public class ResultSql
  {
    public ResultSql()
    {
      _scSql = new StringCollection();
      _intTotalRowsInserted = 0;
    }

    #region Public Properties

    private int _intTotalRowsInserted;
    public int TotalRowsInserted
    {
      get { return _intTotalRowsInserted; }
    }

    private StringCollection _scSql;
    protected StringCollection SqlInserts
    {
      get { return _scSql; }
      set { _scSql = value; }
    }

    private TableOutputs _tOutputs;
    public TableOutputs IdentityValues
    {
      get { return _tOutputs; }
      set { _tOutputs = value; }
    }

    private TimeSpan _tsTotalTime;
    public TimeSpan TotalTime
    {
      get { return _tsTotalTime; }
      set { _tsTotalTime = value; }
    }
	

    #endregion

    #region Helper Lookup Methods

    public int LookupIdentityValue(string strTable, int intIndex)
    {
      return Convert.ToInt32(this.IdentityValues.LookupIdentityValue(strTable, intIndex) );
    }

    #endregion

    public void AddSqlComment(string strSqlComment)
    {
      this.SqlInserts.Add("\r\n-- " + strSqlComment);
    }

    public void AddSqlInsert(string strSql)
    {
      this.SqlInserts.Add(strSql);
      _intTotalRowsInserted++;
    }

    public string GetSqlScript()
    {
      StringBuilder sb = new StringBuilder();

      //Add Table selects
      sb.Append("--Created on " + DateTime.Now.ToString() + "\r\n\r\n");
      sb.Append("--List all tables:\r\n");
      foreach (TableOutput t in _tOutputs.Tables)
      {
        sb.Append("select * from " + t.TableName + "\r\n");
      }

      sb.Append("\r\n\r\n");

      //Shosw SQL inserts
      foreach (string s in SqlInserts)
      {
        sb.Append(s + "\r\n");
      }
      return sb.ToString();
    }

  }
}
