using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SetupUI
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());

      return _intReturnCode;
    }

    private static int _intReturnCode = 0;
    /// <summary>
    /// 0 = Success
    /// 1 = Cancel
    /// 2 = Error
    /// </summary>
    public static int ReturnCode
    {
      get { return _intReturnCode; }
      set { _intReturnCode = value; }
    }

    public const int ReturnCode_Success = 0;
    public const int ReturnCode_Cancel = 1;
    public const int ReturnCode_Error = 2;

  }
}