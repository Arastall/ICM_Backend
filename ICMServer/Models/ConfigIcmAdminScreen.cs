using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigIcmAdminScreen
{
    public int IcmScreenId { get; set; }

    public string? ScreenName { get; set; }

    public string? ScreenTitle { get; set; }

    public int? GroupId { get; set; }
}
