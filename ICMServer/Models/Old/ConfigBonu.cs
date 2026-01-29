//using System;
//using System.Collections.Generic;

//namespace ICMServer.Models;

//public partial class ConfigBonu
//{
//    public int ConfigBonusId { get; set; }

//    public string PayPlanType { get; set; } = null!;

//    public string EmployeeId { get; set; } = null!;

//    /// <summary>
//    /// BONUS_TYPE - either QUARTER, ANNUAL or RETRO
//    /// </summary>
//    public string BonusType { get; set; } = null!;

//    /// <summary>
//    /// This will be the ID as on CONFIG_ALLOCATION_TYPE
//    /// </summary>
//    public int AllocationTypeId { get; set; }

//    public string? Description { get; set; }

//    /// <summary>
//    /// Actual bonus value to be paid
//    /// </summary>
//    public decimal Bonus { get; set; }

//    public decimal Percentage { get; set; }

//    public decimal RangeFrom { get; set; }

//    public decimal RangeTo { get; set; }

//    public string FinYear { get; set; } = null!;

//    /// <summary>
//    /// Indicates if the bonus line is a child value used to determine whether or not PARENT bonus can be released
//    /// </summary>
//    public string IsChild { get; set; } = null!;

//    /// <summary>
//    /// if the IS_CHILD field is set to true then this must point to the parent bonus held in this table (iterative link)
//    /// </summary>
//    public int ParentBonusId { get; set; }
//}
