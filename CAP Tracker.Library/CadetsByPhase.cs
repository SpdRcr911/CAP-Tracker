namespace CAP_Tracker.Library;

public record CadetsByPhase(string Phase, int Count, IEnumerable<AchievementInfo> Achievements)
{
    public static IEnumerable<CadetsByPhase> GroupByPhase(ITrackerService service)
    {
        return (from a in Achievement.All
                    group a by a.Value.Phase into phases
                    let achievements =
                        (from p in phases
                         join t in service.Tracker on p.Value.CadetAchvID equals t.LastAchv.CadetAchvID into g
                         select new AchievementInfo
                         (
                             p.Value.CadetAchvID,
                             g.Count(),
                             p.Value.Grade,
                             p.Value.Insignia,
                             g.OrderByDescending(c => c.NextApprovalDays).Select(c => Cadet.GetCadets(service.Data, c.CAPID).Single()).ToList()
                         ))
                    let count = achievements.Sum(a => a.Cadets.Count())
                    select new CadetsByPhase(phases.Key, count, achievements));

    }
}
public record AchievementInfo(string CadetAchID, int Count, string Grade, Uri? Insignia, IEnumerable<Cadet> Cadets);
