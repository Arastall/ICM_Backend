using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class TmpDataEmployee
{
    public int TmpDataEmployeesId { get; set; }

    public string? RowId { get; set; }

    public string? FstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddr { get; set; }

    public string? JobTitle { get; set; }

    public string? EmployeeNumber { get; set; }

    public string? PrHeldPostnId { get; set; }

    public string? Login { get; set; }
}
