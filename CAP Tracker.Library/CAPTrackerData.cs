using System.Text.Json.Serialization;

namespace CAP_Tracker.Library;
public class CAPTrackerData
{
	public CAPTrackerData() {}
	public CAPTrackerData(int? capId, string nameLast, string nameFirst, DateOnly joinDate, string achvName, DateOnly? aprDate, DateOnly? nextApprovalDate)
	{
		CAPID = capId;
		NameLast = nameLast;
		NameFirst = nameFirst;
		JoinDate = joinDate;
		AchvName = achvName;
		AprDate = aprDate;
		NextApprovalDate = nextApprovalDate;
	}
    public int? CAPID { get; set; }
    public string? NameLast { get; set; }
    public string? NameFirst { get; set; }
    public string? Email { get; set; }
    public string? AchvName { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? AprDate { get; set; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly JoinDate { get; set; }
    public string? Region { get; set; }
    public string? Wing { get; set; }
    public string? Unit { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? PhyFitTest { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? LeadLabDateP { get; set; }
    public int? LeadLabScore { get; set; }
    public AEResults? AEResults { get; set; }
    public DateOnly? CharacterDevelopment { get; set; }
    public bool? ActivePart { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? ActiveParticipationDate { get; set; }
    public bool? CadetOath { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? CadetOathDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? LeadershipExpectationsDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? UniformDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? SpecialActivityDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? NextApprovalDate { get; set; }
    [JsonConverter(typeof(ConditionalDateOnlyJsonConverter))]
    public ConditionallyRequired<DateOnly>? DrillDate { get; set; }
    public int? DrillScore { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? WelcomeCourseDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? EssayDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? SpeechDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? AEInteractiveDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? LeadershipInteractiveDate { get; set; }
    public string? AEInteractiveModule { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? StaffServiceDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? OralPresentationDate { get; set; }
    [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
    public DateOnly? TechnicalWritingAssignmentDate { get; set; }
    public string? TechnicalWritingAssignment { get; set; }
}
