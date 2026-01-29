using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataManualAdjustment
{
    public int AdjustmentId { get; set; }

    public string? AdjustmentType { get; set; }

    public string? PositionId { get; set; }

    public string? EmployeeId { get; set; }

    public string? OrderId { get; set; }

    public int? AllocationTypeId { get; set; }

    public decimal? AllocationValue { get; set; }

    public decimal? AllocationList { get; set; }

    public string? ExcludeFromCalcs { get; set; }

    public string? AdjustmentReason { get; set; }

    public DateTime AdjustmentDate { get; set; }

    public string AdjustmentBy { get; set; } = null!;

    public string AdjustmentProcessed { get; set; } = null!;

    public string PeriodMonth { get; set; } = null!;

    public string PeriodYear { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string? OrderNumber { get; set; }

    public int ItIssue { get; set; }

    public decimal? AllocationRate { get; set; }
}
