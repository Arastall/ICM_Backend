using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataBusinessUnit
{
    public int DataBusinessUnitId { get; set; }

    public string? OrderId { get; set; }

    public string? StatusCd { get; set; }

    public DateTime? StatusDt { get; set; }

    public string? BuName { get; set; }
}
