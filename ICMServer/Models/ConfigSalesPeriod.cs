using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigSalesPeriod
{
    public string? PeriodYear { get; set; }

    public string? PeriodMonth { get; set; }

    public DateTime? PeriodStart { get; set; }

    public DateTime? PeriodEnd { get; set; }
}
