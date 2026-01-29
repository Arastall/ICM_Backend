using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigSystemParameter
{
    public string? ParameterName { get; set; }

    public string? ParameterValue { get; set; }

    public string? ParameterDescription { get; set; }
}
