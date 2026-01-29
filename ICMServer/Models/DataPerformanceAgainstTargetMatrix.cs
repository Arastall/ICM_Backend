using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataPerformanceAgainstTargetMatrix
{
    public int RowId { get; set; }

    public string? Payplan { get; set; }

    public string? EmployeeId { get; set; }

    public int? PeriodYear { get; set; }

    public int? PeriodMonthStart { get; set; }

    public int? PeriodMonthEnd { get; set; }

    public int? AllocationId { get; set; }

    public int? TargetPercentageStart { get; set; }

    public int? TargetPercentageEnd { get; set; }

    public decimal? CommissionValue { get; set; }

    public int? IsManager { get; set; }
}
