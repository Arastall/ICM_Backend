using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigPayplanEnhancement
{
    public int EnhancementId { get; set; }

    public string? PayplanType { get; set; }

    public string? EnhancementDesc { get; set; }

    public string? EnhancementCriteriaSql { get; set; }

    public decimal? EnhancementRate { get; set; }
}
