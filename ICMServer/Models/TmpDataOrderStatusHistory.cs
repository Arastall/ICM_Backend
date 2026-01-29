using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class TmpDataOrderStatusHistory
{
    public int TmpDataOrderStatusHistoryId { get; set; }

    public string? RowId { get; set; }

    public string? BuName { get; set; }

    public DateTime DateCreated { get; set; }
}
