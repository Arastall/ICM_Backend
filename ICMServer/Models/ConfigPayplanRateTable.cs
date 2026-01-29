using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigPayplanRateTable
{
    public int RateTableId { get; set; }

    public string? PayPlanType { get; set; }

    public string? RateTableDesc { get; set; }
}
