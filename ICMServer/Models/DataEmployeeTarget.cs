using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataEmployeeTarget
{
    public int DataEmployeeTargetsId { get; set; }

    public string? EmployeeId { get; set; }

    public int? AllocationTypeId { get; set; }

    public decimal? TargetValue { get; set; }

    public string PeriodMonth { get; set; } = null!;

    public string PeriodYear { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string? Payplan { get; set; }
}
