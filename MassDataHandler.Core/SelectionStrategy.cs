using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class SelectionStrategy
  {
    public SelectionStrategy()
    {
      this.IncludeIdentityColumn = true;
    }

    #region Public Properties

    private string _strDbCon;
    public string DatabaseConnectionString
    {
      get { return _strDbCon; }
      set { _strDbCon = value; }
    }

    private string[] _astrCompanies;
    public string[] Companies
    {
      get { return _astrCompanies; }
      set { _astrCompanies = value; }
    }

    private SchemaObject _doObject;
    public SchemaObject SourceSchemaObject
    {
      get { return _doObject; }
      set { _doObject = value; }
    }

    private int _intMaxRowsPerTable;
    public int MaxRowsPerTable
    {
      get { return _intMaxRowsPerTable; }
      set { _intMaxRowsPerTable = value; }
    }

    private string _strCustomFilter;
    public string CustomTableFilter
    {
      get { return _strCustomFilter; }
      set { _strCustomFilter = value; }
    }

    private bool _blnEnsureAtLeastOneRow;
    public bool EnsureAtLeastOneRow
    {
      get { return _blnEnsureAtLeastOneRow; }
      set { _blnEnsureAtLeastOneRow = value; }
    }

    private bool _blnIncludeIdentityColumn;
    /// <summary>
    ///   If true, includes the identity column in the generated Xml, else it excludes it.
    /// </summary>
    public bool IncludeIdentityColumn
    {
      get { return _blnIncludeIdentityColumn; }
      set { _blnIncludeIdentityColumn = value; }
    }
	
    #endregion

    public bool IsCustomTableSelect
    {
      get
      {
        if (this.CustomTableFilter == null || this.CustomTableFilter.Length == 0)
          return false;
        else
          return true;
      }
    }
    
    /// <summary>
    ///   
    /// </summary>
    /// <param name="strTableCurrent">The table to get rows for. Note that this is not neccessarily
    /// the value in SourceSchemaObject. strTableCurrent could be a dependent table from the SP listed in SourceSchemaObject.</param>
    /// <param name="blnTableHasCompanyColumn"></param>
    /// <returns></returns>
    public string CreateSqlSelect(string strTableCurrent, bool blnTableHasCompanyColumn)
    {
      //if custom sql is specified, use that.
      if (this.IsCustomTableSelect)
      {
        return this.CustomTableFilter; 
      }
      else
      {
        StringBuilder sb = new StringBuilder();
        //select top 5 * from MyTable
        sb.Append("select ");
        if (this.MaxRowsPerTable > 0)
          sb.Append("top " + this.MaxRowsPerTable.ToString() + " ");
        sb.Append("* from " + strTableCurrent + " ");

        if (blnTableHasCompanyColumn)
        {
          if (this.Companies != null && this.Companies.Length > 0)
          {
            string strCoList = ArrayUtilities.JoinStringArray(this.Companies, "'", "'", ", ");
            sb.Append("where co in (" + strCoList + ")");
          }
        }

        return sb.ToString().Trim();
      }

    }

    public string GetSerializedCustomStrategy()
    {
      if (!this.IsCustomTableSelect)
        return "";

      string s = string.Format(
@"This xml snippet was created with the CustomSql section of the 'Use Existing Data' tab. Here are the values used to get this data:

--TableName(s):
{0}

--Custom Sql:
{1}
",
this.SourceSchemaObject.ObjectName,
this.CustomTableFilter);

      //return main 
      return s;
    }
  }

}

