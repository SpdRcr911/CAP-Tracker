namespace CAP_Tracker.Library;

public record CapAchievemntRecord(string AchvName, DateOnly? AprDate)
{
	public int DaysToPromote { get; set; }
	public DateOnly? StartDate { get; set; }
	public DateOnly? EndDate { get; set; }
	public bool? PT { get; set; }
	public bool? LD { get; set; }
	public bool? AE { get; set; }
	public bool? Drill { get; set; }
	public bool? CD { get; set; }
}