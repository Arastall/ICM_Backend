namespace ICMServer.Classes
{
    public class AchievementHistory
    {
        public string OrderId { get; set; }
        public DateTime AchievementDate { get; set; }
        public decimal TotalOrder { get; set; }
        public decimal TotalAchievement { get; set; }
        public decimal PercentageAchievement { get; set; }
        public decimal RateToApply { get; set; }
    }
}

