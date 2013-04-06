using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;

namespace MassDataHandler.Core
{
  public class DataHandlerFacade
  {
    #region Constructors

    public DataHandlerFacade(Assembly aResourceAssembly, string strEmbeddedResourceRoot, string strScript)
      : this(aResourceAssembly, strEmbeddedResourceRoot)
    {
      _strCurrentScript = strScript;
    }

    public DataHandlerFacade(Assembly aResourceAssembly, string strEmbeddedResourceRoot)
    {
      _aResourceAssembly = aResourceAssembly;
      _strEmbeddedResourceRoot = strEmbeddedResourceRoot;

      InsertStrategy objStrategy = new InsertStrategy();
      objStrategy.ImportStrategyData = new ImportStrategyResource(this.ResourceAssembly, this.EmbeddedResourceRoot);
      _im = new InsertManager(objStrategy);

    }

    #endregion

    #region Public Properties

    private InsertManager _im = null;

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

    private string _strCurrentScript;
    public string CurrentScript
    {
      get { return _strCurrentScript; }
      set { _strCurrentScript = value; }
    }
	

    #endregion

    #region Helper methods

    private string GetFullResourceName(string strScript)
    {
      return this.EmbeddedResourceRoot + "." + strScript;
    }

    private XmlDocument GetFragment(string strScript)
    {
      if (strScript == null)
        throw new ArgumentNullException("strScript");

      string strInputData = ReflectionUtilities.GetEmbeddedResourceContent(this.ResourceAssembly, GetFullResourceName(strScript));
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(strInputData);

      return xDoc;
    }

    #endregion

    #region RunXmlScript

    /// <summary>
    ///   Runs the current script; specifies the given script as the "current script" for all other operatons (such as variable lookups).
    /// </summary>
    /// <param name="strScript"></param>
    /// <returns></returns>
    public ResultSql RunXmlScript(string strScript)
    {
      this.CurrentScript = strScript;

      XmlDocument xDoc = GetFragment(this.CurrentScript);

      ResultSql objResults = _im.RunInserts(xDoc);
      return objResults;
    }

    #endregion

    #region GetVariableValue

    public string GetVariableValue(string strVarName)
    {
      XmlDocument xDoc = GetFragment(this.CurrentScript);
      return GetVariableValue(xDoc, strVarName);
    }

    public string GetVariableValue(string strScript, string strVarName)
    {
      XmlDocument xDoc = GetFragment(strScript);
      return GetVariableValue(xDoc, strVarName);
    }

    private string GetVariableValue(XmlDocument xDoc, string strVarName)
    {
      //apply Imports
      xDoc = _im.ApplyImports(xDoc);

      string strValue = xDoc.SelectSingleNode("/Root/Variables/Variable[@name='" + strVarName + "']").Attributes["value"].Value;
      return strValue;
    }

    #endregion

  }
}
