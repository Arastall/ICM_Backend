using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigProcessingType
{
    public string? Payplan { get; set; }

    public string? PeriodYear { get; set; }

    public string? ProcessingType { get; set; }
}
