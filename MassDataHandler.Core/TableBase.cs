using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Collections.Specialized;

namespace MassDataHandler.Core.Table
{
  public abstract class TableBase
  {
    protected TableBase()
    {
      SetDataColumns();
      SetMinimalInsertValues();
      SetForeignKeyColumns();
    }

    protected abstract void SetDataColumns();

    protected abstract void SetForeignKeyColumns();

    /// <summary>
    ///   Inserts the minimum data for all the non-null, required values.
    ///     varchar/char - 'a'
    ///     numeric - 1
    ///     bit - 0
    ///     dateTime - Jan 1, 2000 00:00:00
    /// </summary>
    protected abstract void SetMinimalInsertValues();


    #region Public Properties

    private string _strTableName;
    public string TableName
    {
      get { return _strTableName; }
      protected set { _strTableName = value; }
    }
	

    private string _strPrimaryKey = "";
    public string PrimaryKey 
    {
      get
      {
        return _strPrimaryKey;
      }
      protected set
      {
        _strPrimaryKey = value;
      }

    }

    private string _strIdentityColumn = "";
    public string IdentityColumn
    {
      get
      {
        return _strIdentityColumn;
      }
      protected set
      {
        _strIdentityColumn = value;
      }

    }

    private DataColumn[] _columns;
    public DataColumn[] Columns
    {
      get 
      { 
        return _columns; 
      }
      protected set 
      {
        _columns = value;
      }
    }
	
    private ForeignKey[] _fKeys;
    public ForeignKey[] ForeignKeys
    {
      get
      {
        return _fKeys;
      }
      protected set
      {
        _fKeys = value;
      }
    }

    #endregion

    #region Private / Protected Members

    private StringDictionary _sdMinimalInsertValues;
    public StringDictionary MinimalInsertValues
    {
      get { return _sdMinimalInsertValues; }
      set { _sdMinimalInsertValues = value; }
    }
	
    protected static DataColumn CreateDataColumn(string strName, Type t, bool blnNullable, int intMaxLength)
    {
      DataColumn c = new DataColumn(strName, t);
      c.AllowDBNull = blnNullable;
      
      //MaxLength only applys to DataColumns of type=string
      if (t == typeof(string))
        c.MaxLength = intMaxLength;
  
      return c;
    }

    /// <summary>
    ///   Creates from Schema
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    protected static DataColumn CreateDataColumn(DataRow dr)
    {
      string strName = Convert.ToString(dr["ColumnName"]);

      string strType = Convert.ToString(dr["SystemTypeName"]);
      Type t = Type.GetType("System." + strType);
      bool blnNullable = Convert.ToBoolean(dr["is_Nullable"]);
      int intMaxLength = 0;
      if (t is String)
        intMaxLength = Convert.ToInt32(dr["max_length"]); //Only let strings have length

      return CreateDataColumn(strName, t, blnNullable, intMaxLength);
    }


    #endregion

    #region Public Methods

    //public int InsertMinimalRow()
    //{
    //  string strSql = SqlTextHelper.CreateSqlInsert(this.MinimalInsertValues, this);
    //  int intNewId =  DataUtilities.ExecuteInsertReturnIdentity(strSql);
    //  return intNewId;
    //}

    public TableColumn LookupPrimaryKeyGivenForeignKey(string strForeignKey)
    {
      foreach (ForeignKey fk in this.ForeignKeys)
      {
        if (StringUtilities.AreStringsEqual(strForeignKey, fk.ForeignKeyName) )
        {
          return fk.PrimaryKeyColumn;
        }
      }
      return null;
    }

    #endregion

    #region Creation Methods

    public static TableBase CreateTableObject(string strTableName)
    {
      //gets root Assembly Name like: MassDataHandler.Core
      try
      {
        string strRoot = System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", "");
        string strName = strRoot + ".Table." + strTableName;

        Type t = Type.GetType(strName, true, true);
        object obj1 = Activator.CreateInstance(t);
        TableBase tblObj = (TableBase)obj1;

        return tblObj;
      }
      catch (Exception ex)
      {
        throw new BadSchemaException("Could not create table '" + strTableName + "' from xml script.", ex);
      }
    }

    public static TableBase CreateTableObjectFromDatabase(string strTableName, string strDbCon)
    {
      //Get from DB
      string strPrimaryKey = DataUtilities.Schema.GetPrimaryKey(strTableName, strDbCon);
      string strIdentityColumn = DataUtilities.Schema.GetIdentityColumn(strTableName, strDbCon);

      TableBase tblGeneric = new GenericSchemaTable(strPrimaryKey, strTableName, strIdentityColumn);

      DataTable dt = DataUtilities.Schema.GetColumnSchemaInfo(strTableName, strDbCon);
      
      //SetDataColumns
      DataColumn[] dc = new DataColumn[dt.Rows.Count];
      for (int i = 0; i < dt.Rows.Count; i++)
      {
        dc[i] = CreateDataColumn(dt.Rows[i]);
      }
      tblGeneric.Columns = dc;

      //SetMinimalInsertValues
      //  Cycle through and get all non-null columns that aren't the identity column
      StringDictionary sd = new StringDictionary();
      for (int i = 0; i < dt.Rows.Count; i++)
      {
        DataRow dr = dt.Rows[i];
        bool blnNullable = Convert.ToBoolean(dr["is_Nullable"]);
        bool blnIsIdentity = Convert.ToBoolean(dr["is_Identity"]);
        if (!blnNullable && !blnIsIdentity)
        {
          //required (and not identity) --> create it
          string strName = Convert.ToString(dr["ColumnName"]);
          string strType = Convert.ToString(dr["SystemTypeName"]);
          string strDefaultData = SqlTextHelper.CreateMinimalData(strType) ;
          sd.Add(strName, strDefaultData);
        }
      }
      tblGeneric.MinimalInsertValues = sd;

      return tblGeneric;
    }

    #endregion
  }
}
