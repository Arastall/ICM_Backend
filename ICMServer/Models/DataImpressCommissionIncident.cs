using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataImpressCommissionIncident
{
    public int DataImpressCommissionIncidentId { get; set; }

    public string? SfOrderId { get; set; }

    public string? SfEmployeeId { get; set; }

    public string? IncidentDescription { get; set; }

    public string? PeriodMonth { get; set; }

    public string? PeriodYear { get; set; }
}
