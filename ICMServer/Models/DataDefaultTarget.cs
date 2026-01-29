using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataDefaultTarget
{
    public int DataDefaultTargetsId { get; set; }

    public string? PayPlanType { get; set; }

    public string? EmployeeId { get; set; }

    public int? AllocationTypeId { get; set; }

    public string? AllocationDescription { get; set; }

    public decimal? TargetValue { get; set; }

    public string FinYear { get; set; } = null!;
}
