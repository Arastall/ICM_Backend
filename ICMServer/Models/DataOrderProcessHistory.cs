using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataOrderProcessHistory
{
    public int DataOrderProcessHistoryId { get; set; }

    public string? RowId { get; set; }

    public string? BuName { get; set; }

    public bool? RevenueProcessed { get; set; }

    public bool? CommissionProcessed { get; set; }

    public DateTime DateCreated { get; set; }
}
