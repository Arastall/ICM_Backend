using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class SystemLog
{
    public int SystemLogId { get; set; }

    public string? LogSource { get; set; }

    public string? LogDesc { get; set; }

    public string? ReferenceType { get; set; }

    public string? ReferenceValue { get; set; }

    public string? LogCategory { get; set; }

    public DateTime? LogDatetime { get; set; }

    public int? LogCountId { get; set; }
}
