using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MassDataHandler.Core
{
  public class CodeGen
  {
    public CodeGen()
    {

    }

    /// <summary>
    ///   
    /// </summary>
    /// <param name="strAssemblyName">Physical path to assembly</param>
    /// <param name="strFullClassName">Full class name, including namespace</param>
    /// <param name="strObjectName">Name of instantiated object</param>
    /// <returns></returns>
    public static string CreateObjectInstantiation(string strAssemblyName, string strFullClassName, string strObjectName)
    {
      try
      {
        Assembly a = Assembly.LoadFile(strAssemblyName);
        Type t = a.GetType(strFullClassName, true, true);

        if (t == null)
          throw new ArgumentException("Could not get type of input object.");
        else
          return CreateObjectInstantiation(t, strObjectName);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static string CreateObjectInstantiation(Type t, string strObjectName)
    {
      //Create Method like:
      //private TabRecordInput GetInput(ResultSql objResult)
      //{
      //  NAMESPACE.TabRecordInput input = new NAMESPACE.TabRecordInput();
      //  input.ActiveCompanyId = _mdh.GetVariableValue("co2");
      //  input.ActiveEmployeeId = _mdh.GetVariableValue("empId_CoAdmin");
      //  input.ViewedCompanyId = _mdh.GetVariableValue("co2");
      //  input.sMenuSpace = "co";
      //  input.sTabId = "COMPANY";
      //  input.bDebug = false;

      //  return input;
      //}

      PropertyInfo[] api = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
      string strClassName = t.Name;

      StringBuilder sb = new StringBuilder();
      sb.Append("private " + strClassName + " GetInputObject()\r\n");
      sb.Append("{\r\n");
      sb.Append("\t" + strClassName + " " + strObjectName + " = new " + strClassName + "();\r\n");

      foreach (PropertyInfo pi in api)
      {
        string strParamName = pi.Name;
        string strParamType = pi.PropertyType.Name;

        sb.Append("\t" + strObjectName + "." + strParamName + " = " + CreateSampleData(strParamType) + ";\r\n");
      }

      sb.Append("\r\n\treturn input;\r\n");
      sb.Append("}\r\n");
      return sb.ToString();
    }


    private static string CreateSampleData(string strType)
    {
      TypeUtilities.GeneralType gType = TypeUtilities.GetGeneralType(strType);

      switch (gType)
      {
        case TypeUtilities.GeneralType.Boolean:
          return "true";
        case TypeUtilities.GeneralType.DateTime:
          return "new DateTime(2000, 1, 1)";
        case TypeUtilities.GeneralType.Number:
          return "123";
        case TypeUtilities.GeneralType.Text:
          return "\"abc\"";
        default:
          return "OTHER_TYPE: " + strType;
      }

    }
  }
}