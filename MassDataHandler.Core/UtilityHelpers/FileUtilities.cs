using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
 
namespace MassDataHandler.Core
{
  public class FileUtilities
  {
    public static string ReadFileContents(string strFile)
    {
      if (strFile == null || strFile.Length == 0)
        return null;
      if (!File.Exists(strFile))
        return null;

      string s = "";
      System.IO.StreamReader sr = null;
      try
      {
        sr = new StreamReader(strFile);
        s = sr.ReadToEnd();
      }
      catch
      {
        throw;
      }
      finally
      {
        if (sr != null)
          sr.Close();
      } //end of finally

      return s;
    } //end of method

    public static void WriteFileContents(string strContent, string strFile)
    {
      if (strFile == null || strFile.Length == 0)
        return;

      System.IO.StreamWriter sw = null;
      try
      {
        //create directory if it doesn't exist:
        //CreateDirectory(Path.GetDirectoryName(strFile));
        sw = new StreamWriter(strFile);
        sw.Write(strContent);
      }
      catch
      {
        throw;
      }
      finally
      {
        if (sw != null)
          sw.Close();
      } //end of finally
    } //end of method

  }
}
