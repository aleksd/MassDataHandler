using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
 
namespace MassDataHandler.Core
{
  /// <summary>
  ///   The methods in this class are based on the blog entry:
  ///   http://timstall.dotnetdevelopersjournal.com/embedded_resources.htm
  /// </summary>
  public class ReflectionUtilities
  {
    public static string GetEmbeddedResourceContent(Type typeInAssembly, string strResourceName)
    {
      Assembly asm = Assembly.GetAssembly(typeInAssembly);
      return GetEmbeddedResourceContent(asm, strResourceName);
    } //end of method

    /// <summary>
    ///		Gets the embedded resource from the calling assembly.
    /// </summary>
    /// <param name="strResourceName">The namespace-qualified resource name.</param>
    /// <returns>The value of the resource as a string.</returns>
    public static string GetEmbeddedResourceContent(string strResourceName)
    {
      Assembly asm = Assembly.GetCallingAssembly();
      return GetEmbeddedResourceContent(asm, strResourceName);

    } //end of method

    public static string GetEmbeddedResourceContent(Assembly asm, string strResourceName)
    {
      string strContent = "";
      Stream strm = null;
      StreamReader reader = null;
      try
      {
        //get resource:
        string strName = asm.GetName().Name + "." + strResourceName;
        strm = asm.GetManifestResourceStream(strName);
        if (strm == null)
          strContent = null;
        else
        {
          //read contents of embedded file:
          reader = new StreamReader(strm);

          if (reader == null)
            strContent = null;
          else
            strContent = reader.ReadToEnd();
        } //end of if
      }
      catch
      {
        throw;
      }
      finally
      {
        if (strm != null)
          strm.Close();
        if (reader != null)
          reader.Close();
      } //end of finally

      return strContent;

    } //end of method


  }
}
