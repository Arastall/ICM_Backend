using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class TmpDataOrderHeader
{
    public int TmpDataOrderHeaderId { get; set; }

    public string? RowId { get; set; }

    public string? OrderNumber { get; set; }

    public string? AccountNumber { get; set; }

    public string? SapReference { get; set; }

    public string? PromotionCode { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerType { get; set; }

    public string? OrderType { get; set; }

    public string? PrPostnId { get; set; }

    public string? MaintenanceTerm { get; set; }
}
