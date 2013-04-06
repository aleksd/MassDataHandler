using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using MassDataHandler.Core.Table;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using System.IO;

namespace MassDataHandler.Core
{
  public class InsertManager
  {
    #region Constructors

    public InsertManager()
    {
      this.Strategy = new InsertStrategy();
    }

    public InsertManager(InsertStrategy objStrategy)
    {
      this.Strategy = objStrategy;
      //if (this.Strategy != null)
      //  this.ImportStrategyData = this.Strategy.ImportStrategyData;
    }

    #endregion

    #region Instance Properties

    private InsertStrategy _strategy;
    public InsertStrategy Strategy
    {
      get { return _strategy; }
      set { _strategy = value; }
    }
	 
    #endregion

    #region Create Datatable Methods

    public DataTable CreateDataTable(string strXml)
    {
      XmlDocument xDoc = AddRootNodes(strXml);
      return CreateDataTable(xDoc, null);
    }

    public DataTable CreateDataTable(string strXml, TableOutputs tOutputs)
    {
      XmlDocument xDoc = AddRootNodes(strXml);
      return CreateDataTable(xDoc, tOutputs);

    }
    public DataTable CreateDataTable(XmlDocument xDoc)
    {
      return CreateDataTable(xDoc, null);
    }

    /// <summary>
    ///   Given the MassDataHandler's xDoc and Identity lookups, create a DataTable.
    ///   DataTable will have all columns of type=string.
    /// </summary>
    /// <remarks>
    ///   Assumes that all rows have the same number of columns.
    /// </remarks>
    /// <param name="xDoc">Looks in the MassDataHandler, only the first Table section is used.</param>
    /// <param name="tOutputs">The Identity Lookups</param>
    /// <returns></returns>
    public DataTable CreateDataTable(XmlDocument xDoc, TableOutputs tOutputs)
    {
      xDoc = ApplyImports(xDoc);

      //parse out vars
      StringDictionary sdVars = GetVariables(xDoc);

      XmlNode xNodeTable = xDoc.SelectSingleNode("/Root/Table");
      DataTable dt = Utilities.CreateTableSchemaFromXmlNode(xNodeTable);

      foreach (XmlNode xRow in xNodeTable.ChildNodes)
      {
        StringDictionary sdRow = Utilities.PopulateSDFromXmlAttributes(xRow);
        sdRow = DoSubstition(sdRow, tOutputs, sdVars);

        DataRow dr = dt.NewRow();
        dr = Utilities.CreateDataRowFromSD(dr, sdRow);
        dt.Rows.Add(dr);
      }

      return dt;
    }


    #endregion

    #region Call From File (Static methods callable from a command line)

    /// <summary>
    ///   Call with a xmlFile path and DB conString
    /// </summary>
    public static ResultSql RunInsertsFromFile(string strXmlFile, string strDbConString)
    {
      Console.WriteLine("MassDataHelper.RunInserts:");
      Console.WriteLine(" Xml file: " + strXmlFile);
      Console.WriteLine(" DB ConString: " + strDbConString);

      XmlDocument xDoc = new XmlDocument();
      xDoc.Load(strXmlFile);

      string strImportDir = Path.GetDirectoryName(strXmlFile);
      Console.WriteLine("Import directory: " + strImportDir);

      InsertStrategy objStrategy = new InsertStrategy();
      objStrategy.ImportStrategyData = new ImportStrategyFile(strImportDir);

      InsertManager im = new InsertManager(objStrategy);

      Console.WriteLine("About to run inserts:");
      return im.RunInserts(xDoc, strDbConString);
    }

    //public static void CreateSingleRowInserts(string strOutputFile, string strDbConString)
    //{
    //  Console.WriteLine("MassDataHelper.RunInserts:");
    //  Console.WriteLine(" Xml output file: " + strOutputFile);
    //  Console.WriteLine(" DB ConString: " + strDbConString);
    //}

    #endregion

    #region RunInserts Methods

    /// <summary>
    /// Takes an Xml fragment. This fragement should not have the "Root" node, the method
    /// will add it.
    /// </summary>
    public ResultSql RunInserts(string strXml)
    {
      XmlDocument xDoc = AddRootNodes(strXml);
      return RunInserts(xDoc);
    }

    /// <summary>
    ///   No DbConString provided, gets conString from app.config
    /// </summary>
    public ResultSql RunInserts(XmlDocument xDoc)
    {
      string strDbConString = DataUtilities.CreateConnectionStringFromAppConfig();
      return RunInserts(xDoc, strDbConString);
    }

    public ResultSql RunInserts(XmlDocument xDoc, string strDbConString)
    {
      //get variables as a SD

      /*for each table:
       * Get Insert-Columns
       *  Start with Table's minimum inserts
       *  Apply any Default Row
       *  Overwrite with custom values
       * 
       * Apply Substitutions
       *  Variables     $(...)
       *  Foreign Keys  @1
       * 
       * Do Insert
       * Record Output (such as PK)
       */

      DateTime dtStart = DateTime.Now;

      xDoc = ApplyImports(xDoc);

      EnsureTablesExist(xDoc, strDbConString);
      if (this.Strategy.DeleteAllTableData)
        DeleteTables(xDoc, strDbConString);       //DATABASE HIT

      //int intInsertCount = 0;
      ResultSql objResultSql = new ResultSql();
      
      StringDictionary sdVars = GetVariables(xDoc);
      TableOutputs tOutputs = new TableOutputs();
      objResultSql.IdentityValues = tOutputs;

      //foreach table:
      XmlNodeList xTables = xDoc.SelectNodes("/Root/Table");
      foreach (XmlNode xNodeTable in xTables)
      {
        string strTable = Utilities.Trim(Utilities.GetXmlAttributeValue(xNodeTable, "name") );
        string strAliasName = Utilities.Trim(Utilities.GetXmlAttributeValue(xNodeTable, "alias") );

        TableBase tblBase = null;
        if (this.Strategy.UseDatabaseForTableSchema)
          tblBase = TableBase.CreateTableObjectFromDatabase(strTable, strDbConString);
        else
          tblBase = TableBase.CreateTableObject(strTable);

        TableOutput tOutput = new TableOutput(strTable, strAliasName);
        tOutputs.Tables.Add(tOutput);
        objResultSql.AddSqlComment("TABLE: " + strTable);

        StringDictionary sdMinimal = tblBase.MinimalInsertValues;
        StringDictionary sdDefaultTemplate = new StringDictionary();

        bool blnAllowIdentityInserts = GetIdentityInsertValue(this.Strategy, xNodeTable, strDbConString);

        //Have table instance, now get each row (which is just children)
        foreach (XmlNode xRow in xNodeTable.ChildNodes)
        {
          try
          {
            //Get Insert-Columns
            RowType rType = GetRowType(xRow);
            switch (rType)
            {
              case RowType.Default:
                //Update the default SD
                sdDefaultTemplate = Utilities.PopulateSDFromXmlAttributes(xRow);
                break;
              case RowType.Custom:
                //Get custom SD
                StringDictionary sdCustom = Utilities.PopulateSDFromXmlAttributes(xRow);

                /*Finish all the hard work here (we only insert if there's a custom row)
                 * Combine 3 SD into one
                 * Do substitution for final SD (var & FK)
                 * Create SQL insert from that
                 * Do Insert
                 * Record Output
                 */
                StringDictionary sdFinal = MergeStringDictionaries(sdMinimal, sdDefaultTemplate, sdCustom);
                sdFinal = DoSubstition(sdFinal, tOutputs, sdVars);

                String strSqlInsert = SqlTextHelper.CreateSqlInsert(sdFinal, tblBase);
                int intNewPK = -1;
                try
                {
                  intNewPK = DataUtilities.ExecuteInsertReturnIdentity(strSqlInsert, strDbConString, blnAllowIdentityInserts, tblBase.TableName);     //DATABASE HIT HERE
                  Console.WriteLine("Inserted line: " + strSqlInsert);
                  objResultSql.AddSqlInsert(strSqlInsert);
                }
                catch (Exception ex)
                {
                  throw new MDHException(ex.Message + "\r\nError inserting into database.\r\nTable: " + tblBase.TableName
                    + "\r\nRow: " + xRow.OuterXml
                    + "\r\nSql Insert: " + strSqlInsert
                    + "\r\nFinal String Dictionary: " + Utilities.CollapseStringDictionaryToString(sdFinal)
                    + "\r\n", ex);
                }
                //NOTE: for now only handling 1-column PK. --> Update: really this is just saving the Identity column (single value), not PK (multi-value)
                //Need PK column
                StringDictionary sdRowPK = new StringDictionary();
                if (tblBase.IdentityColumn != null && tblBase.IdentityColumn.Length > 0)  //only add if Identity exists.
                  sdRowPK.Add(tblBase.IdentityColumn, intNewPK.ToString());
                tOutput.Rows.Add(sdRowPK);

                break;
            }
          }
          catch (Exception ex)
          {
            throw new MDHException("General error:"
              + "\r\nTable: " + strTable
              + "\r\nRow: " + xRow.OuterXml
              , ex);
          }
        } //end of row foreach

      } //end of table foreach

      DateTime dtEnd = DateTime.Now;
      TimeSpan tsTotalTime = dtEnd.Subtract(dtStart);
      objResultSql.TotalTime = tsTotalTime;

      return objResultSql;
    }

    private static bool GetIdentityInsertValue(InsertStrategy Strategy, XmlNode xNodeTable, string strDbCon)
    {
      if (Strategy == null)
        throw new ArgumentNullException("Strategy cannot be null");
      if (xNodeTable == null)
        throw new ArgumentNullException("Xml row cannot be null");

      //See if this table has an Identity. If it doesn't have an identity, then return false
      //  Trying to set IdentityInsert=On for a table without identities will cause an error.
      string strTableName = GetXmlAttributeValue(xNodeTable, "name");
      if (!DataUtilities.Schema.HasIdentityColumn(strTableName, strDbCon))
        return false; 

      bool blnAllowIdentityInserts = Strategy.AllowIdentityInserts;

      //Get value from XmlNode
      string strAllowIdentityInserts = GetXmlAttributeValue(xNodeTable, "allowIdentityInserts");
      if (!string.IsNullOrEmpty(strAllowIdentityInserts))
        blnAllowIdentityInserts = TypeUtilities.ConverToBoolean(strAllowIdentityInserts);

      return blnAllowIdentityInserts;
    }

    private static string GetXmlAttributeValue(XmlNode xNode, string strAttrName)
    {
      if (xNode == null || string.IsNullOrEmpty(strAttrName) || xNode.Attributes[strAttrName] == null)
        return null;
      else
        return xNode.Attributes[strAttrName].Value;
    }

    #endregion

    #region Handle Expressions

    private static StringDictionary DoSubstition(StringDictionary sdColumnValues, TableOutputs tOutputs, StringDictionary sdVars)
    {
      //NOTE: can't modify collection while enumerating through it, so create a new collection.
      /*For each item in sdColumnValues, replace it
       * Variable:      $(...)
       * FK Inference   @n
       */
      StringDictionary sdColumnValuesNew = new StringDictionary();

      foreach (DictionaryEntry de in sdColumnValues)
      {
        string strCurrentColumn = de.Key.ToString();
        string strValue = de.Value.ToString();
        //STEP 1: Handle $(..) embedded expressions (could be variables or Identity lookups)
        strValue = DoExpressionSubstitution(strValue, sdVars, strCurrentColumn, tOutputs);

        //STEP 2: Idenities. Simple cases (like '@1') may not be in an expression syntax.
        if (IsIdentityLookup(strValue))
        {
          strValue = DoIdentityLookup(strValue, strCurrentColumn, tOutputs);
        }

        strValue = SqlTextHelper.EscapeSqlString(strValue);
        sdColumnValuesNew.Add(strCurrentColumn, strValue);
      }

      return sdColumnValuesNew;
    }

    private static string DoIdentityLookup(string strValue, string strCurrentColumn, TableOutputs tOutputs)
    {
      Cell cParent = GetFKLookupCell(strValue);

      string strParentTable = null;
      if (cParent.TableColumnInfo == null)
        strParentTable = strCurrentColumn.Substring(0, strCurrentColumn.Length - 2); //remove the "ID";
      else
        strParentTable = cParent.TableColumnInfo.TableName;

      strValue = tOutputs.LookupIdentityValue(strParentTable, cParent.RowIndex);
      return strValue;
    }

    private static string DoExpressionSubstitution(string strOriginal, StringDictionary sdVars, string strCurrentColumn, TableOutputs tOutputs)
    {
      /*expect a string with optional variables of the form $(..)
       * First find next $(..)
       * Get its var name
       * Lookup value (throw error if not found)
       * Do string replace
       * Repeat until no more vars
       */

      const string strMatchPattern = @"(?<=\$\().+?(?=\))";

      Match m = Regex.Match(strOriginal, strMatchPattern);
      while (m.Success)
      {
        string strExpression = m.Value;
        //If Expression is of the form \w (or no '@', then assume it's a variable, else assume it's an Identity lookup.
        string strNewVal = null;
        if (strExpression.IndexOf("@") == -1)
        {
          //Variable, like "myCompany"
          if (!sdVars.ContainsKey(strExpression))
            throw new ExpressionException("The variable '" + strExpression + "' was not defined.");
          strNewVal = sdVars[strExpression];
        }
        else
        {
          //Identity lookup, like "TableX.@12" or "@3"
          if (IsIdentityLookup(strExpression))
          {
            strNewVal = DoIdentityLookup(strExpression, strCurrentColumn, tOutputs);
          }
          else
          {
            throw new ExpressionException("The expression '" + strExpression + "' is not a valid Identity Lookup expression.");
          }
        }

        strOriginal = strOriginal.Replace("$(" + strExpression + ")", strNewVal);
        m = Regex.Match(strOriginal, strMatchPattern);
      };

      return strOriginal;

    }

    private static bool IsIdentityLookup(string strVal)
    {
      /* Must end with @\d+, like:
       *  Table1.@123   --> specifies table prefix
       *  @34           --> no table prefix, table must be infered from column Name
       */ 
      return System.Text.RegularExpressions.Regex.Match(strVal, @"@\d+$").Success;
    }

    private static Cell GetFKLookupCell(string strVal)
    {
      //NOTE: don't worry about columnName as a table can have only 1 identity column

      int intPeriodIndex = strVal.IndexOf(".");
      Cell c = null;
      if (intPeriodIndex == -1)
      {
        //no '.', just "@n" --> get n
        string s = strVal.Substring(1);
        int intRowIndex = Convert.ToInt32(s);
        c = new Cell(null, intRowIndex);
      }
      else
      {
        // there is a preceeding table, like "Table1.@123".
        string strTable = strVal.Substring(0, intPeriodIndex);
        int intRowIndex = Convert.ToInt32( strVal.Substring(intPeriodIndex + 2) );  // add for both '.' and '@' 
        c = new Cell(new TableColumn(strTable, null), intRowIndex);
      }

      return c;
    }

    #endregion

    #region Specifc Helper Methods

    public XmlDocument ApplyImports(XmlDocument xDoc)
    {
      //Ensure that if any Imports exist in the parent doc, then there is an import strategy specified.
      XmlNode nOld = xDoc.SelectSingleNode("/Root/Import");
      if (nOld != null && (this.Strategy == null || this.Strategy.ImportStrategyData == null) )
        throw new ImportException("An Import statement was used in the Xml file (" + nOld.OuterXml + "), but no ImportStratgey was specified in the InsertManager constructor.");

      return _ApplyImports(xDoc);
    }

    /// <summary>
    /// </summary>
    /// <remarks>
    ///  Note, this automatically handles nested imports because we keep checked for Import nodes.
    /// Therefore if you import(1) a file that has an import(2), that file first gets merged with the main xDoc,
    /// then the method researches for any import nodes, finds import(2) because it's now part of the main doc,
    /// and handles it.
    /// </remarks>
    private XmlDocument _ApplyImports(XmlDocument xDoc)
    {
      //Create a new instance of the xDoc
      XmlDocument xDoc2 = new XmlDocument();
      xDoc2.LoadXml(xDoc.OuterXml);

      //For each <Import>, get its embedded resource content and replace the node.
      XmlNode nOld = xDoc2.SelectSingleNode("/Root/Import");

      if (nOld == null)
        return xDoc2;  //No imports, so just return doc

      while (nOld != null)
      {
        string strResource = Utilities.GetXmlAttributeValue(nOld, "resource");
        if (strResource == null)
        {
          throw new ImportException("There was no 'resource' file specified in: " + nOld.OuterXml);
        }
        string strContent = this.Strategy.ImportStrategyData.GetResourceContent(strResource);

        XmlDocument xDocImport = new XmlDocument();
        xDocImport.LoadXml(strContent);
        XmlNode nNew = xDocImport.SelectSingleNode("Root");
        XmlUtilities.ReplaceNodes(nOld, nNew.ChildNodes);

        //check for additional Imports:
        nOld = xDoc2.SelectSingleNode("/Root/Import");
      }

      return xDoc2;

    }

    //Throw exception
    private static void EnsureTablesExist(XmlDocument xDoc, string strDbCon)
    {
      string[] astrTables = ParseOutTables(xDoc);
      StringCollection sc = new StringCollection();
      foreach (string strTable in astrTables)
      {
        if (!DataUtilities.Schema.DoesTableExist(strTable, strDbCon))
          sc.Add(strTable);
      }

      if (sc.Count > 0)
      {
        String[] s1 = StringUtilities.ConvertStringCollectionToArray(sc);
        string s2 = StringUtilities.JoinStringArray(s1, "'", "'", ", ");
        //return s2;
        throw new Exception("The following tables do not exist in the specified database: " + s2 + "\r\n");
      }
    }

    private static string[] ParseOutTables(XmlDocument xDoc)
    {
      StringCollection sc = new StringCollection();
      XmlNodeList xTables = xDoc.SelectNodes("/Root/Table");
      foreach (XmlNode xNodeTable in xTables)
      {
        string strTable = xNodeTable.Attributes["name"].Value.ToLower();
        if (!sc.Contains(strTable))
          sc.Add(strTable);
      }

      return StringUtilities.ConvertStringCollectionToArray(sc);
    }

    private static void DeleteTables(XmlDocument xDoc, string strDbCon)
    {
      //Get table list, delete 1 by 1.
      string[] astrTables = ParseOutTables(xDoc);
      foreach (string strTable in astrTables)
      {
        try
        {
          DataUtilities.DeleteTable(strTable, strDbCon);
        }
        catch (Exception ex)
        {
          throw new DataAccessException("Could not delete table '" + strTable + "'.", ex);
        }
      }
    }

    public static XmlDocument AddRootNodes(string strXml)
    {
      if (string.IsNullOrEmpty(strXml))
        return null;

      //Only add root nodes if not already present
      if(!strXml.Trim().StartsWith("<Root>"))
        strXml = "<Root>" + strXml + "</Root>";
      
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(strXml);
      return xDoc;
    }

    #endregion

    #region Generic Helper Methods

    public static StringDictionary MergeStringDictionaries(StringDictionary sdMinimal, StringDictionary sdDefaultTemplate, StringDictionary sdCustom)
    {
      StringDictionary sdTemp = Utilities.MergeStringDictionaries(sdMinimal, sdDefaultTemplate);
      sdTemp = Utilities.MergeStringDictionaries(sdTemp, sdCustom);
      return sdTemp;
    }

    public static StringDictionary GetVariables(XmlDocument xDoc)
    {
      XmlNodeList xList = xDoc.SelectNodes("/Root/Variables/Variable");
      StringDictionary sd = new StringDictionary();

      if (xList.Count == 0)
        return sd;

      //  <Variable name="co" value="0669" />
      //  <Variable name="username" value="Mr.Smith" />
      foreach (XmlNode n in xList)
      {
        string strName = n.Attributes["name"].Value;
        string strValue = n.Attributes["value"].Value;
        sd.Add(strName, strValue);
      }

      return sd;
    }

    private static RowType GetRowType(XmlNode n)
    {
      string strType = n.Name.ToLower();
      switch (strType)
      {
        case "default":
          return RowType.Default;
        case "row":
          return RowType.Custom;
        case "#comment":
          return RowType.Comment;
        default:
          throw new ArgumentException("Unexpected xml node type '" + strType + "' in XmlNode: " + n.OuterXml);
      }
    }

    public enum RowType
    {
      Default,  //<Default>
      Custom,   //<Row>
      Comment     //Like Comments
    }

    #endregion
  }

  #region  Strategies

  public class InsertStrategy
  {
    public InsertStrategy()
    {
      _blnUseDatabaseForTableSchema = true;
      _blnAllowIdentityInserts = false;
      _blnDeleteAllTableData = true;
    }

    private bool _blnUseDatabaseForTableSchema;
    /// <summary>
    ///   True = Use the live database to dynamically generate the TableBase schema objects. (Good if static TableBase objects don't exist)
    ///   False = Use static, codeGenerated classes to store the schema info. (Good for performance and to ensure assumptions about the schema).
    /// </summary>
    public bool UseDatabaseForTableSchema
    {
      get { return _blnUseDatabaseForTableSchema; }
      set { _blnUseDatabaseForTableSchema = value; }
    }

    private bool _blnAllowIdentityInserts;
    /// <summary>
    ///   True to wrap each insert with "SET IDENTITY_INSERT XService ON". Default is false.
    /// </summary>
    public bool AllowIdentityInserts
    {
      get { return _blnAllowIdentityInserts; }
      set { _blnAllowIdentityInserts = value; }
    }

    private bool _blnDeleteAllTableData;
    /// <summary>
    ///   Deletes all data in the tables before inserting.
    /// </summary>
    public bool DeleteAllTableData
    {
      get { return _blnDeleteAllTableData; }
      set { _blnDeleteAllTableData = value; }
    }
	
    private ImportStrategyBase _importStrategy;
    public ImportStrategyBase ImportStrategyData
    {
      get { return _importStrategy; }
      set { _importStrategy = value; }
    }

  }

  public abstract class ImportStrategyBase
  {
    public abstract string GetResourceContent(string strResourceName);
  }

  public class ImportStrategyResource : ImportStrategyBase
  {
    public ImportStrategyResource()
    {

    }
    public ImportStrategyResource(Assembly aResourceAssembly, string strEmbeddedResourceRoot)
    {
      _aResourceAssembly = aResourceAssembly;
      _strEmbeddedResourceRoot = strEmbeddedResourceRoot;
    }

    #region Public Properties

    private Assembly _aResourceAssembly;
    /// <summary>
    ///   The assembly that contains the resources
    /// </summary>
    public Assembly ResourceAssembly
    {
      get { return _aResourceAssembly; }
      set { _aResourceAssembly = value; }
    }

    private string _strEmbeddedResourceRoot;
    /// <summary>
    ///   The root embedded resource path
    /// </summary>
    public string EmbeddedResourceRoot
    {
      get { return _strEmbeddedResourceRoot; }
      set { _strEmbeddedResourceRoot = value; }
    }

    #endregion

    public override string GetResourceContent(string strResourceName)
    {
      string strFullResourceName = this.EmbeddedResourceRoot + "." + strResourceName;

      string strContent = ReflectionUtilities.GetEmbeddedResourceContent(this.ResourceAssembly, strFullResourceName);
      if (strContent == null)
        throw new ImportException("The expected embedded resource '" + strResourceName + "' is '" + strFullResourceName + "', which does not exist.");
      return strContent;
    }
  }

  public class ImportStrategyFile : ImportStrategyBase
  {
    public ImportStrategyFile()
    {

    }
    public ImportStrategyFile(string strRootDirectory)
    {
      RootDirectory = strRootDirectory;
    }

    #region Public Properties

    private string _strRootDirectory;
    /// <summary>
    ///   The Root directory that contains the resouce
    /// </summary>
    public string RootDirectory
    {
      get { return _strRootDirectory; }
      set { _strRootDirectory = value; }
    }

    #endregion

    /// <summary>
    ///   Given the resource (file name) with extension, return its contents.
    /// </summary>
    /// <param name="strResourceName"></param>
    /// <returns></returns>
    public override string GetResourceContent(string strResourceName)
    {
      string strFullFilePath = Path.Combine(this.RootDirectory, strResourceName);

      if (!File.Exists(strFullFilePath))
        throw new ImportException("The expected file for the resource '" + strResourceName + "' is '" + strFullFilePath + "', which does not exist.");

      string strContent = FileUtilities.ReadFileContents(strFullFilePath);
      return strContent;
    }
  }

  #endregion
}
