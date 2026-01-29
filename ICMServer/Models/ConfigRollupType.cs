using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigRollupType
{
    public int RollupTypeId { get; set; }

    public string? PayplanType { get; set; }

    public string? RollupType { get; set; }

    public string? RollupCriteria { get; set; }

    public string PeriodYear { get; set; } = null!;
}
