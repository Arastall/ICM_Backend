using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigAllocationType
{
    public int AllocationTypeId { get; set; }

    public string? AllocationType { get; set; }

    public string? AllocationDescription { get; set; }

    public string? AllocationCriteriaSql { get; set; }
}
