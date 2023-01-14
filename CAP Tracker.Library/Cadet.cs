using CAP_Tracker.Library;

public record Cadet(int CAPID, string FirstName, string LastName, string Email, string LastAchievement,
    DateOnly? LastApprovalDate, DateOnly? NextPromotionDate, CAPTrackerData NextPromotionData,
    IEnumerable<CAPTrackerData> AllData);
