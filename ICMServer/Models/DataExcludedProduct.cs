using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataExcludedProduct
{
    public int DataExcludedProductsId { get; set; }

    public string? ItemId { get; set; }

    public string? ProductDesc { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductLevel1 { get; set; }

    public string? ProductLevel2 { get; set; }

    public string? ProductLevel3 { get; set; }

    public string? ProductType { get; set; }

    public int? Excluded { get; set; }

    public int? LevelBased { get; set; }
}
