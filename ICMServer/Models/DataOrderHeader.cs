using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataOrderHeader
{
    public int DataOrderHeadersId { get; set; }

    public string? OrderRowId { get; set; }

    public string? OrderNumber { get; set; }

    public string? SapOrderReference { get; set; }

    public string? OrderType { get; set; }

    public string? PromotionCode { get; set; }

    public string? CustomerAccountNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerType { get; set; }

    public string? PrPostnId { get; set; }

    public decimal? OrderListVal { get; set; }

    public decimal? OrderSaleVal { get; set; }

    public decimal? OrderDiscountVal { get; set; }

    public decimal? OrderDiscountPercent { get; set; }

    public decimal? ServiceListVal { get; set; }

    public decimal? ServiceSaleVal { get; set; }

    public decimal? ServiceDiscountVal { get; set; }

    public decimal? ServiceDiscountPercent { get; set; }

    public string? ServiceType { get; set; }

    public DateTime DateCreated { get; set; }

    public string? PeriodMonth { get; set; }

    public string? PeriodYear { get; set; }

    public string? MaintenanceTerm { get; set; }
}
