using System;
using System.Collections.Generic;
using System.Text;

namespace MassDataHandler.Gui
{
  public class EmployeeDemo
  {
    public EmployeeDemo()
    {

    }

    #region Public Properties

    private int _intEmployeeId;

    public int EmployeeId
    {
      get { return _intEmployeeId; }
      set { _intEmployeeId = value; }
    }

    private string _strFirstName;

    public string FirstName
    {
      get { return _strFirstName; }
      set { _strFirstName = value; }
    }

    private string _strLastName;

    public string LastName
    {
      get { return _strLastName; }
      set { _strLastName = value; }
    }

    private DateTime _dtHireDate;

    public DateTime HireDate
    {
      get { return _dtHireDate; }
      set { _dtHireDate = value; }
    }

    #endregion

  }
}
