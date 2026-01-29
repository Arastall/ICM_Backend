using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataImpressSubscriptionOrder
{
    public int DataImpressSubscriptionOrderId { get; set; }

    public string? ImOrderNumber { get; set; }

    public string? SfOrderNumber { get; set; }

    public string? ImAccountName { get; set; }

    public string? ImAccountNumber { get; set; }

    public string? ImPlanName { get; set; }

    public string? ImPlanTier { get; set; }

    public string? SfEmployeeId { get; set; }

    public int? SfSubscriptionMonthsAmount { get; set; }

    public decimal? SfAnticipatedUsage { get; set; }

    public decimal? SfSubscriptionPrice { get; set; }

    public string? SfOrderStatus { get; set; }

    public DateTime? SfLastStatusUpdateTime { get; set; }

    public decimal? ImPricePerUsageItem { get; set; }

    public int? ImActualUsage { get; set; }

    public int? Processed { get; set; }
}
