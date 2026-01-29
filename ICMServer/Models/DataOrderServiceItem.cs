using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataOrderServiceItem
{
    public int DataOrderServiceItemsId { get; set; }

    public string? OrderItemId { get; set; }

    public string? OrderRowId { get; set; }

    public int? LineNumber { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductDesc { get; set; }

    public decimal? ListValue { get; set; }

    public decimal? SaleValue { get; set; }

    public decimal? Quantity { get; set; }

    public string? ProductLevel1 { get; set; }

    public string? ProductLevel2 { get; set; }

    public string? ProductLevel3 { get; set; }

    public string? ProductType { get; set; }

    public DateTime DateCreated { get; set; }
}
