using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataOrderPosition
{
    public int DataOrderPositionsId { get; set; }

    public string? OrderRowId { get; set; }

    public string? PositionRowId { get; set; }

    public decimal? AllocationPercentage { get; set; }

    public string? PositionName { get; set; }

    public string? PayplanType { get; set; }

    public string? ParentPositionRowId { get; set; }

    public string? EmployeeRowId { get; set; }

    public DateTime DateCreated { get; set; }
}
