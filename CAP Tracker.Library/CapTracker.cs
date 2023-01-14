namespace CAP_Tracker.Library;
public record CapTracker(int CAPID, Achievement LastAchv, string NameLast, string NameFirst, string Email, DateOnly JoinDate, DateOnly? NextApprovalDate, List<CapAchievemntRecord> CadetAchivements)
{
	public int NextApprovalDays { get; set; }
	public double? AvgDaysToPromote { get; set; }

    public static IEnumerable<CapTracker> GetTracker(IEnumerable<CAPTrackerData> data, IEnumerable<int>? inactiveCadets)
    {
        var tracker = (from t in data.Where(d => inactiveCadets == null || !inactiveCadets.Contains(d.CAPID!.Value))
                       group t by new { t.CAPID, t.NameLast, t.NameFirst, t.Email, t.JoinDate } into g
                       let achievements = (from a in g
                                           let achRow = Achievement.All.Values.First(ar => ar.CadetAchvID == a.AchvName)
                                           select new CapAchievemntRecord(a.AchvName!, a.AprDate)
                                           {
                                               PT = a.PhyFitTest.HasValue,
                                               LD = (a.AchvName == "Gen Ira C Eaker") ? a.SpeechDate.HasValue : (a.LeadLabDateP.HasValue || a.LeadershipInteractiveDate.HasValue),
                                               AE = !achRow.NeedsAE ? null : (a.AEDateP.HasValue || a.AEInteractiveDate.HasValue),
                                               Drill = !achRow.NeedsDrill ? null : a.DrillDate.HasValue,
                                               CD = !achRow.NeedsCD ? null : a.CharacterDevelopment.HasValue
                                           }).ToList()
                       let lastAch = achievements.Any(a => a.AprDate.HasValue) ? achievements.Where(a => a.AprDate.HasValue).Last() : achievements.Last()
                       let currentAchv = Achievement.All.Values.First(a => a.CadetAchvID == lastAch.AchvName)
                       let nextApprovalDate = (achievements.All(a => a.AprDate.HasValue) ? new DateOnly?() : g.Max(c => c.NextApprovalDate!.Value))
                       select new CapTracker(g.Key.CAPID!.Value, currentAchv, g.Key.NameLast, g.Key.NameFirst, g.Key.Email, g.Key.JoinDate, nextApprovalDate, achievements)).ToList();


        foreach (var cadet in tracker)
        {
            for (int i = 0; i < cadet.CadetAchivements.Count; i++)
            {
                var currentAchv = cadet.CadetAchivements[i];

                currentAchv.StartDate = currentAchv.AprDate ?? null;
                currentAchv.EndDate = cadet.CadetAchivements.Count > i + 1 && cadet.CadetAchivements[i + 1].AprDate.HasValue ? cadet.CadetAchivements[i + 1].AprDate!.Value : null;

                if (currentAchv.StartDate.HasValue && currentAchv.EndDate.HasValue)
                {
                    currentAchv.DaysToPromote = ((currentAchv.EndDate ?? DateOnly.FromDateTime(DateTime.Today)).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
                }
                else if (currentAchv.StartDate.HasValue && !currentAchv.EndDate.HasValue)
                {
                    currentAchv.DaysToPromote = (DateOnly.FromDateTime(DateTime.Today).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
                }
                else if (!currentAchv.AprDate.HasValue)
                {
                    currentAchv.DaysToPromote = (DateTime.Today - cadet.NextApprovalDate!.Value.ToDateTime(new TimeOnly())).Days;
                    cadet.NextApprovalDays = (DateTime.Today - cadet.NextApprovalDate!.Value.ToDateTime(new TimeOnly())).Days;
                }
                else
                {
                    currentAchv.DaysToPromote = 0;
                }
            }
            cadet.AvgDaysToPromote = cadet.CadetAchivements.Any(ca => ca.EndDate.HasValue) ? cadet.CadetAchivements.Where(ca => ca.AprDate.HasValue && ca.EndDate.HasValue).Average(ca => ca.DaysToPromote) : null;
        }
        return tracker;
    }
}
