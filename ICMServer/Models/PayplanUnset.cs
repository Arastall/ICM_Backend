using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class PayplanUnset
{
    public int PayplanUnsetId { get; set; }

    public string? Payplan { get; set; }

    public string Orderid { get; set; } = null!;
}
