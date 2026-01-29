using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class DataOrderStatusHistory
{
    public string? RowId { get; set; }

    public string StatusCd { get; set; } = null!;

    public DateTime? StatusDt { get; set; }

    public DateTime DateCreated { get; set; }
}
