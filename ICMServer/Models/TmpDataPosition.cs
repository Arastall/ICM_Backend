using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class TmpDataPosition
{
    public int TmpDataPositionsId { get; set; }

    public string? RowId { get; set; }

    public string? PositionName { get; set; }

    public DateTime? EndDt { get; set; }

    public DateTime? StartDt { get; set; }

    public string? PrEmpId { get; set; }

    public string? XRepType { get; set; }

    public string? ParentRowId { get; set; }
}
