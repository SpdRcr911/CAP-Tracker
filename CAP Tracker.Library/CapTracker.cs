namespace CAP_Tracker.Library;
public record CapTracker(int CAPID, Achievement LastAchv, string NameLast, string NameFirst, string Email, DateOnly JoinDate, DateOnly? NextApprovalDate, List<CapAchievemntRecord> CadetAchivements)
{
	public int NextApprovalDays { get; set; }
	public double? AvgDaysToPromote { get; set; }
}
