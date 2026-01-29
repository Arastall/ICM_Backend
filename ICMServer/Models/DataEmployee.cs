using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataEmployee
{
    public int DataEmployeesId { get; set; }

    public string? RowId { get; set; }

    public string? FstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddr { get; set; }

    public string? JobTitle { get; set; }

    public string? EmployeeNumber { get; set; }

    public string? PrHeldPostnId { get; set; }

    public string? Login { get; set; }

    /// <summary>
    /// Indicates whether EMPLOYEE has been deleted from the imported TMP_DATA_EMPLOYEES extract [1 = DELETED]
    /// </summary>
    public int? DeleteFlag { get; set; }

    public DateTime DateCreated { get; set; }
}
