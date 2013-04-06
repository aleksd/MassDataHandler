using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
 
namespace MassDataHandler.Core
{
  public class ArrayUtilities
  {

    public static string[] SplitCSVToArray(string strCSV)
    {
      if (strCSV == null || strCSV.Length == 0)
        return new string[0];

      string[] astrNew = strCSV.Split(',');
      //filter out null/empties.

      StringCollection sc = new StringCollection();
      foreach (string s in astrNew)
      {
        if (s != null)
        {
          string strNew = s.Trim();
          if (strNew.Length > 0)
            sc.Add(strNew);
        }

      }

      return ArrayUtilities.ConvertStringCollectionToArray(sc);

    }

    private static string[] ConvertStringCollectionToArray(System.Collections.Specialized.StringCollection sc)
    {
      if (sc == null)
        return null;

      string[] astr = new string[sc.Count];
      for (int i = 0; i < sc.Count; i++)
      {
        astr[i] = sc[i];
      }
      return astr;
    }

    public static string JoinStringArray(string[] astr, string strBeforeEach, string strAfterEach, string strDivider)
    {
      if (astr == null || astr.Length == 0)
        return "";

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < astr.Length; i++)
      {
        string s = astr[i];
        sb.Append(strBeforeEach + s + strAfterEach);
        if (i < astr.Length - 1)
        {
          sb.Append(strDivider);
        }
      }

      return sb.ToString();
    }

  }
}
