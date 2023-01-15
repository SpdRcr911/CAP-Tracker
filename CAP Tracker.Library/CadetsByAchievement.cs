namespace CAP_Tracker.Library;

public record CadetsByAchievement(int CAPID, string LastName, string FirstName, DateOnly JoinDate, string Email, int? AvgDaysToPromote, string CadetAchvID, DateOnly? NextApprovalDate, int? NextApprovalDays, string NextAchievement, bool? CD, bool? PT, bool? Drill, bool? AE, bool? LD, int PromotionScore)
{
    public static IEnumerable<CadetsByAchievement> GroupByAchievement(ITrackerService service)
    {

        return (from t in service.Tracker
                             join a in Achievement.All on t.LastAchv.CadetAchvID equals a.Value.CadetAchvID
                             let WorkingOn = t.CadetAchivements.Last()
                             let PromotionScore = Convert.ToInt16(t.NextApprovalDays >= 0) << 5 |
                                                  Convert.ToInt16(WorkingOn.CD ?? true) << 4 |
                                                  Convert.ToInt16(WorkingOn.PT ?? true) << 3 |
                                                  Convert.ToInt16(WorkingOn.Drill ?? true) << 2 |
                                                  Convert.ToInt16(WorkingOn.AE ?? true) << 1 |
                                                  Convert.ToInt16(WorkingOn.LD ?? true) << 0
                             //where t.NextApprovalDays >= 100
                             orderby a.Key, PromotionScore descending, t.NextApprovalDays descending
                             select new CadetsByAchievement
                             (
                                 t.CAPID,
                                 t.NameLast,
                                 t.NameFirst,
                                 t.JoinDate,
                                 t.Email,
                                 (int?)t.AvgDaysToPromote,
                                 t.LastAchv.CadetAchvID,
                                 t.NextApprovalDate,
                                 t.NextApprovalDays,
                                 WorkingOn.AchvName == "New Cadet" ? "Achievement 1" : WorkingOn.AchvName,
                                 WorkingOn.CD,
                                 WorkingOn.PT,
                                 WorkingOn.Drill,
                                 WorkingOn.AE,
                                 WorkingOn.LD,
                                 PromotionScore
                             ));
    }
}
