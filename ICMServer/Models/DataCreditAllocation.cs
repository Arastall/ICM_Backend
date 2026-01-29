using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICMServer.Models;

public partial class DataCreditAllocation
{
    public int DataCreditAllocationId { get; set; }

    public string? PositionId { get; set; }

    public string? EmployeeId { get; set; }

    public string? ChildPositionId { get; set; }

    public string? AllocationSource { get; set; }

    public string? OrderId { get; set; }

    public string? ItemId { get; set; }

    public int? AdjustmentId { get; set; }

    public int? AllocationTypeId { get; set; }

    public decimal? AllocationList { get; set; }

    public decimal? AllocationValue { get; set; }

    public string? ExcludeFromCalcs { get; set; }

    public string? RollupProcessed { get; set; } = null!;

    public string? PeriodMonth { get; set; } = null!;

    public string? PeriodYear { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    [Column("PAYPLAN")]
    public string? Payplan { get; set; }
}
