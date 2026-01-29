using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataNewcomerSetting
{
    public int DataNewcomerSettingsId { get; set; }

    public string? EmployeeRowId { get; set; }

    public string? PositionName { get; set; }

    public int? PeriodDurationInMonths { get; set; }

    public DateTime? PeriodStartDate { get; set; }

    public decimal? Guarantee { get; set; }

    public int? Activated { get; set; }
}
