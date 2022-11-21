namespace CAP_Tracker.Library;
public record CapTracker(int CAPID, string CurrAchv, string NameLast, string NameFirst, DateOnly JoinDate, DateOnly? NextApprovalDate, List<CapAchievemntRecord> CadetAchivements)
{
	public int NextApprovalDays { get; set; }
	public double? AvgDaysToPromote { get; set; }
}
