using CAP_Tracker.Library;

public record Cadet(int CAPID, string FirstName, string LastName, string Email, string LastAchievement,
    DateOnly? LastApprovalDate, DateOnly? NextPromotionDate, CAPTrackerData NextPromotionData,
    IEnumerable<CAPTrackerData> AllData)
{

    public static IEnumerable<Cadet> GetCadets(IEnumerable<CAPTrackerData> data)
    => from d in data
       group d by new { CAPID = d.CAPID!.Value, d.NameFirst, d.NameLast, d.Email } into gd
       let isSpaatz = !gd.Any(g => g.AprDate is null)
       let latestAchievement = (true ? gd.Where(g => g.AprDate == gd.Max(c => c.AprDate)) : gd.Where(g => g.AprDate is null)).First()
       let nextAprDate = gd.First().NextApprovalDate
       let nextAchievement = gd.SingleOrDefault(g => g.AprDate is null)
       let cadet = gd.Key
       select new Cadet(
           cadet.CAPID,
           cadet.NameFirst,
           cadet.NameLast,
           cadet.Email,
           latestAchievement.AchvName!,
           latestAchievement.AprDate,
           nextAprDate,
           nextAchievement,
           gd.ToList()
       );
}
