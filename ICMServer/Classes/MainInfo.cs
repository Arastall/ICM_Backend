namespace ICMServer.Classes
{
    public class MainInfo
    {
        public int LastRunId { get; set; }
        public string Status { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string User { get; set; } // ✅ Ajoutez cette propriété
        public string Salesperiod { get; set; }
    }
}
