using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Core
{
  public class MDHException : Exception
  {
    public MDHException(string strMessage) : base(strMessage) { }

    public MDHException(string strMessage, Exception exInner)
      : base(strMessage, exInner) { }
  }

  public class BadDataException : MDHException
  {
    public BadDataException(string strMessge) : base(strMessge) { }

    public BadDataException(string strMessge, Exception exInner)
      : base(strMessge, exInner) { }

  }
  public class BadSchemaException : MDHException
  {
    public BadSchemaException(string strMessge) : base(strMessge) { }

    public BadSchemaException(string strMessge, Exception exInner)
      : base(strMessge, exInner) { }
  }

  public class DataAccessException : MDHException
  {
    public DataAccessException(string strMessge) : base(strMessge) { }

    public DataAccessException(string strMessge, Exception exInner)
      : base(strMessge, exInner) { }
  }

  public class ExpressionException : MDHException
  {
    public ExpressionException(string strMessge) : base(strMessge) { }

    public ExpressionException(string strMessge, Exception exInner)
      : base(strMessge, exInner) { }
  }

  public class ImportException : MDHException
  {
    public ImportException(string strMessge) : base(strMessge) { }

    public ImportException(string strMessge, Exception exInner)
      : base(strMessge, exInner) { }
  }
}

