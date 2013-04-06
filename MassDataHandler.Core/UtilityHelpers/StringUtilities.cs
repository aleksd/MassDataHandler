using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace MassDataHandler.Core
{
  public class StringUtilities
  {

    public static bool AreStringsEqual(string s1, string s2)
    {
      return AreStringsEqual(s1, s2, true, false);
    }

    public static bool AreStringsEqual(string str1, string str2, bool blnIgnoreCase, bool blnTrim)
    {
      //check for nulls
      if (str1 == null && str2 == null)
        return true;
      else if ((str1 == null && str2 != null) || (str1 != null && str2 == null))
        return false;
      else
      {
        //non-null values, do check
        if (blnTrim)
        {
          str1 = str1.Trim();
          str2 = str2.Trim();
        }
        if (blnIgnoreCase)
        {
          str1 = str1.ToLower();
          str2 = str2.ToLower();
        }
        return (str1 == str2);
      }
    }

    public static string[] ConvertStringCollectionToArray(StringCollection sc)
    {
      if (sc == null)
        return null;

      string[] astr = new string[sc.Count];
      for (int i = 0; i < sc.Count; i++)
        astr[i] = sc[i];
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
          sb.Append(strDivider);
      }

      return sb.ToString();
    }

    public static string[] SplitCSVToArray(string strCSV)
    {
      if (strCSV == null || strCSV.Length == 0)
        return new string[0];

      string[] astrNew = strCSV.Split(',');
  
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

      return ConvertStringCollectionToArray(sc);
    }

  }
}
