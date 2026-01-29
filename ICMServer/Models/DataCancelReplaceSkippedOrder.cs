using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataCancelReplaceSkippedOrder
{
    public int DataCancelReplaceSkippedOrderId { get; set; }

    public string? OrderRowId { get; set; }

    public string? OrderNumber { get; set; }

    public string? PeriodMonth { get; set; }

    public string? PeriodYear { get; set; }

    public DateTime DateCreated { get; set; }
}
