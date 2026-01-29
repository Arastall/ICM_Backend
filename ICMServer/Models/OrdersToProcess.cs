using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class OrdersToProcess
{
    public int Id { get; set; }

    public string? OrderId { get; set; }

    public DateTime? StatusDt { get; set; }
}
