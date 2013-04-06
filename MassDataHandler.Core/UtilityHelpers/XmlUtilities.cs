using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
 
namespace MassDataHandler.Core
{
  public class XmlUtilities
  {
    public static string EncodeEntity(string strVal)
    {
      if (strVal == null)
        return strVal;

      //   &lt; , &gt; , &amp; , &quot; , &apos;
      strVal = strVal.Replace("&", "&amp;");
      strVal = strVal.Replace("\"", "&quot;");
      strVal = strVal.Replace("<", "&lt;");
      strVal = strVal.Replace(">", "&gt;");
      strVal = strVal.Replace("'", "&apos;");
      return strVal;
    }

    public static string DecodeEntity(string strVal)
    {
      if (strVal == null)
        return strVal;

      //   &lt; , &gt; , &amp; , &quot; , &apos;
      strVal = strVal.Replace("&apos;", "'");
      strVal = strVal.Replace("&gt;", ">");
      strVal = strVal.Replace("&lt;", "<");
      strVal = strVal.Replace("&quot;", "\"");
      strVal = strVal.Replace("&amp;", "&");
      return strVal;
    }


    public static void ReplaceNodes(XmlNode nOld, XmlNode nNew)
    {
      if (nOld == null || nNew == null)
        return;

      //Handle if node from different xDoc
      XmlNode nNew2 = nOld.OwnerDocument.ImportNode(nNew, true);

      nOld.ParentNode.ReplaceChild(nNew2, nOld);
    }


    public static void ReplaceNodes(XmlNode nOld, XmlNodeList nNewList)
    {
      if (nOld == null || nNewList == null)
        return;

      XmlNode nRef = nOld;

      foreach (XmlNode nNew in nNewList)
      {
        XmlNode nNew2 = nOld.OwnerDocument.ImportNode(nNew, true);

        nOld.ParentNode.InsertAfter(nNew2, nRef);
        nRef = nNew2;
      }

      //remove nOld;
      nOld.ParentNode.RemoveChild(nOld);
    }

    public static string FormatXml(string strXml)
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(strXml);

      XmlTextWriter xw = null;
      try
      {
        StringBuilder sb = new StringBuilder();
        System.IO.StringWriter sw = new System.IO.StringWriter(sb);
        xw = new XmlTextWriter(sw);
        xw.Formatting = Formatting.Indented;

        xDoc.WriteTo(xw);
        return sb.ToString();
      }
      finally
      {
        if (xw != null)
          xw.Close();
      }

    }

  }
}
