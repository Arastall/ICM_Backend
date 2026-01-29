using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigPayplanType
{
    public string? PayPlanType { get; set; }

    public int? DefaultRateTable { get; set; }

    public string? PayPlanDesc { get; set; }
}
