using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class TmpDataOrderRep
{
    public int TmpDataOrderRepsId { get; set; }

    public string? OrderPosId { get; set; }

    public string? RowId { get; set; }

    public string? PositionId { get; set; }

    public decimal? AllocationPerc { get; set; }
}
