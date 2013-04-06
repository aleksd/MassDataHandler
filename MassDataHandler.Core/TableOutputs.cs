using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace MassDataHandler.Core
{
  public class TableOutputs
  {
    public TableOutputs()
    {
      _listTables = new List<TableOutput>();
    }

    private List<TableOutput> _listTables;
    public List<TableOutput> Tables
    {
      get { return _listTables; }
      set { _listTables = value; }
    }

    #region Helper Methods

    /// <summary>
    ///   Each table can only have 1 identity value per row.
    /// </summary>
    /// <param name="strTable"></param>
    /// <param name="intIndex"></param>
    /// <returns></returns>
    public string LookupIdentityValue(string strAliasName, int intIndex)
    {
      TableOutput tOutput = GetTableGivenAliasName(strAliasName);
      if (tOutput == null)
        throw new BadSchemaException("Tried to lookup Identity for table '" + strAliasName + "', but that table doesn't exist.");

      //Get Row Index (Note, user expected 1-based, not 0-based)
      if (tOutput.Rows.Count < intIndex)
        throw new BadSchemaException("Tried to lookup Identity for row " + intIndex.ToString() + ", but table '" + tOutput.ToString() + "' only has '" + tOutput.Rows.Count + " row(s). There is no row at index " + intIndex.ToString() + ". (Note that the index is 1-based, not 0-based.)");
      StringDictionary sdRow = tOutput.Rows[intIndex - 1];

      //Get column --> assume only 1 value, gets first.
      string strVal = null;
      if (sdRow == null || sdRow.Count == 0)
        throw new BadSchemaException("Table '" + tOutput.ToString() + "' does not have an Identity column.");

      foreach (DictionaryEntry de in sdRow)
      {
        if (de.Value == null)
          throw new BadSchemaException("Table '" + tOutput.ToString() + "' does not have an Identity value.");
        strVal = de.Value.ToString();
      }

      return strVal;
    }


    public TableOutput GetTableGivenAliasName(string strAliasName)
    {
      foreach (TableOutput t in this.Tables)
      {
        if (StringUtilities.AreStringsEqual(t.AliasName, strAliasName))
          return t;
      }
      return null;
    }

    #endregion

  }


  /// <summary>
  ///   Contains a List of Rows, where a Row is a StringDictionary of PK columns.
  ///   Note that a Row can have more than 1 PK column (hence it is represented as
  ///   a SD, as opposed to just a DictionaryEntry).
  /// </summary>
  public class TableOutput
  {
    public TableOutput(string strTableName, string strAliasName)
    {
      _strTableName = strTableName;
      
      //If aliasName is null, then make aliasName be the tableName.
      if (strAliasName == null || strAliasName.Length == 0)
        _strAliasName = strTableName;
      else
        _strAliasName = strAliasName;

      _sdListRows = new List<StringDictionary>();
    }

    private string _strTableName;
    public string TableName
    {
      get { return _strTableName; }
      set { _strTableName = value; }
    }

    private string _strAliasName;
    public string AliasName
    {
      get { return _strAliasName; }
      set { _strAliasName = value; }
    }
	

    private List<StringDictionary> _sdListRows;
    public List<StringDictionary> Rows
    {
      get { return _sdListRows; }
      set { _sdListRows = value; }
    }

    public override string ToString()
    {
      return TableName + " (alias='" + this.AliasName + "')";
    }
  }
}
