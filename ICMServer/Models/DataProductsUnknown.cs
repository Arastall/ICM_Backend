using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataProductsUnknown
{
    public int DataProductsUnknownId { get; set; }

    public string? ProductDesc { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductLevel1 { get; set; }

    public string? ProductLevel2 { get; set; }

    public string? ProductLevel3 { get; set; }
}
