using System;
using System.Collections.Generic;

namespace ICMServer.Models;

public partial class ConfigIcmAdminUser
{
    public int IcmUserId { get; set; }

    public string? Username { get; set; }

    public string? UserFname { get; set; }

    public string? UserSname { get; set; }

    public int? UserGroupid { get; set; }
}
