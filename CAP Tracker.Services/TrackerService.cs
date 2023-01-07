using CAP_Tracker.Library;

namespace CAP_Tracker.Services;

public class TrackerService
{
    readonly List<CAPTrackerData> Data;
    readonly List<int>? InactiveCadets;
    public List<CapTracker> Tracker { get; private set; }

    public TrackerService()
    {
        Data = new List<CAPTrackerData>();
        InactiveCadets = new List<int>();
        Tracker = new List<CapTracker>();
    }
    public TrackerService(List<CAPTrackerData> data, List<int>? inactiveCadets = default)
    {
        Data = data;
        InactiveCadets = inactiveCadets;
        Tracker = new List<CapTracker>();
        GetTracker();
    }
    private void GetTracker()
    {
        Tracker = (from t in Data.Where(d => InactiveCadets == null || !InactiveCadets.Contains(d.CAPID!.Value))
                   group t by new { t.CAPID, t.NameLast, t.NameFirst, t.Email, t.JoinDate } into g
                   let achievements = (from a in g select new CapAchievemntRecord(a.AchvName!, a.AprDate) { PT = a.PhyFitTest.HasValue, LD = a.LeadLabDateP.HasValue || a.LeadershipInteractiveDate.HasValue, AE = a.AEDateP.HasValue || a.AEInteractiveDate.HasValue, Drill = a.DrillDate.HasValue, CD = a.CharacterDevelopment.HasValue }).ToList()
                   let currentAchv = Achievements.Values.First(a => a.CadetAchvID == achievements.Where(a => a.AprDate.HasValue).Last().AchvName)
                   let nextApprovalDate = (achievements.All(a => a.AprDate.HasValue) ? new DateOnly?() : g.Max(c => c.NextApprovalDate!.Value))
                   select new CapTracker(g.Key.CAPID!.Value, currentAchv, g.Key.NameLast, g.Key.NameFirst, g.Key.Email, g.Key.JoinDate, nextApprovalDate, achievements)).ToList();


        foreach (var cadet in Tracker)
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
    }

    public static readonly SortedDictionary<int, Achievement> Achievements = new()
    {
        { 1, new Achievement("Achievement 1", "1", "C/Amn", "PHASE I - The Learning Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/curry_insig_F3AB50D13AA8E.jpg")) },
        { 2, new Achievement("Achievement 2", "2", "C/A1C", "PHASE I - The Learning Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/arnold_insig_65858DA05810F.jpg")) },
        { 3, new Achievement("Achievement 3", "3", "C/SrA", "PHASE I - The Learning Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/feik_insig_2CBC98B37573A.jpg")) },
        { 4, new Achievement("Wright Brothers", "Wright", "C/SSgt", "PHASE I - The Learning Phase", false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/wright_insig_48E8C7C1AA1B1.jpg")) },
        { 5, new Achievement("Achievement 4", "4", "C/TSgt", "PHASE II - The Leadership Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/EVR_insig_A14EB349FFF79.jpg")) },
        { 6, new Achievement("Achievement 5", "5", "C/MSgt", "PHASE II - The Leadership Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/lindburgh_insig_FEADDF248147A.jpg")) },
        { 7, new Achievement("Achievement 6", "6", "C/SMSgt", "PHASE II - The Leadership Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/doolittle_insig_3703604194B56.jpg")) },
        { 8, new Achievement("Achievement 7", "7", "C/CMSgt (1)", "PHASE II - The Leadership Phase", false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/goddard_armstrong_insig_3F44AA2E9617C.jpg")) },
        { 9, new Achievement("Achievement 8", "8", "C/CMSgt (2)", "PHASE II - The Leadership Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/goddard_armstrong_insig_3F44AA2E9617C.jpg")) },
        { 10, new Achievement("Billy Mitchell", "Mitchell", "C/2d Lt (1)", "PHASE II - The Leadership Phase", false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_1b_6C4760028DE68.gif")) },
        { 11, new Achievement("Achievement 9", "9", "C/2d Lt (2)", "PHASE III - The Command Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_1b_6C4760028DE68.gif")) },
        { 12, new Achievement("Achievement 10", "10", "C/1st Lt (1)", "PHASE III - The Command Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_2b_6558A9577C5EA.jpg")) },
        { 13, new Achievement("Achievement 11", "11", "C/1st Lt (2)", "PHASE III - The Command Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_2b_6558A9577C5EA.jpg")) },
        { 14, new Achievement("Amelia Earhart", "Earhart", "C/Capt (1)", "PHASE III - The Command Phase", false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_3b_6A8431B7E1FD1.gif")) },
        { 15, new Achievement("Achievement 12", "12", "C/Capt (2)", "PHASE IV - The Executive Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_3b_6A8431B7E1FD1.gif")) },
        { 16, new Achievement("Achievement 13", "13", "C/Capt (3)", "PHASE IV - The Executive Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_3b_6A8431B7E1FD1.gif")) },
        { 17, new Achievement("Achievement 14", "14", "C/Maj (1)", "PHASE IV - The Executive Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_4b_35E3F2B152B0B.jpg")) },
        { 18, new Achievement("Achievement 15", "15", "C/Maj (2)", "PHASE IV - The Executive Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_4b_35E3F2B152B0B.jpg")) },
        { 19, new Achievement("Achievement 16", "16", "C/Maj (3)", "PHASE IV - The Executive Phase", true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_4b_35E3F2B152B0B.jpg")) },
        { 20, new Achievement("Gen Ira C Eaker", "Eaker", "C/Lt Col", "PHASE IV - The Executive Phase", false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_5b_E75C6247F7174.jpg")) },
        { 21, new Achievement("Gen Carl A Spaatz", "Spaatz", "C/Col", "Pinnacle", false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_6b_F03B21162A70A.jpg")) }
    };
}
