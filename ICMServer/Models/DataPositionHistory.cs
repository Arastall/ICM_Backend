using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataPositionHistory
{
    public int DataPositionHistoryId { get; set; }

    public string? EmployeeRowId { get; set; }

    public string? PositionRowId { get; set; }

    public string? PositionName { get; set; }

    public DateTime? StartDt { get; set; }

    public DateTime? EndDt { get; set; }

    public string? ParentRowId { get; set; }

    public string? XRepType { get; set; }

    public DateTime DateCreated { get; set; }
}
