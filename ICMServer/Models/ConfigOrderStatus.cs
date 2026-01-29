using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigOrderStatus
{
    public string? StatusCd { get; set; }

    public string? BuName { get; set; }

    public bool? Revenue { get; set; }

    public bool? Commission { get; set; }
}
