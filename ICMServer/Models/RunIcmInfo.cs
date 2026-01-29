using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class RunIcmInfo
{
    public int RunIcmInfoId { get; set; }

    public DateTime? BeginDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }
}
