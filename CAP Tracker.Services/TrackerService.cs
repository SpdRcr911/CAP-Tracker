using CAP_Tracker.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAP_Tracker.Services
{
    public class TrackerService
    {
        List<CAPTrackerData> Data;
        SortedDictionary<string, Achievement> Achievements;
        List<int> InactiveCadets;
        public List<CapTracker> Tracker { get; private set; }

        public TrackerService()
        {
            Data = new List<CAPTrackerData>();
            Achievements = new SortedDictionary<string, Achievement>();
            InactiveCadets = new List<int>();
            Tracker = new List<CapTracker>();
        }
        public TrackerService(List<CAPTrackerData> data, SortedDictionary<string, Achievement> achivements, List<int> inactiveCadets)
        {
            Data = data;
            Achievements = achivements;
            InactiveCadets = inactiveCadets;
            Tracker = new List<CapTracker>();
        }
        public void GetTracker()
        {


            Tracker = (from t in Data.Where(d => !InactiveCadets.Contains(d.CAPID!.Value))
                       group t by new { t.CAPID, t.NameLast, t.NameFirst, t.JoinDate } into g
                       let achievements = (from a in g select new CapAchievemntRecord(a.AchvName!, a.AprDate) { PT = a.PhyFitTest.HasValue, LD = a.LeadLabDateP.HasValue || a.LeadershipInteractiveDate.HasValue, AE = a.AEDateP.HasValue || a.AEInteractiveDate.HasValue, Drill = a.DrillDate.HasValue, CD = a.CharacterDevelopment.HasValue }).ToList()
                       let currAchv = Achievements[achievements.Where(a => a.AprDate.HasValue).Last().AchvName].Rank
                       let nextApprovalDate = (achievements.All(a => a.AprDate.HasValue) ? new DateOnly?() : g.Max(c => c.NextApprovalDate!.Value))
                       select new CapTracker(g.Key.CAPID!.Value, currAchv, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, nextApprovalDate, achievements)).ToList();


            foreach (var cadet in Tracker)
            {
                for (int i = 0; i < cadet.CadetAchivements.Count; i++)
                {
                    var currentAchv = cadet.CadetAchivements[i];

                    currentAchv.StartDate = currentAchv.AprDate.HasValue ? currentAchv.AprDate.Value : null;
                    currentAchv.EndDate = cadet.CadetAchivements.Count > i + 1 && cadet.CadetAchivements[i + 1].AprDate.HasValue ? cadet.CadetAchivements[i + 1].AprDate!.Value : null;

                    if (currentAchv.StartDate.HasValue && currentAchv.EndDate.HasValue)
                    {
                        currentAchv.DaysToPromote = ((currentAchv.EndDate.HasValue ? currentAchv.EndDate.Value : DateOnly.FromDateTime(DateTime.Today)).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
                    }
                    else if (currentAchv.StartDate.HasValue && !currentAchv.EndDate.HasValue)
                    {
                        currentAchv.DaysToPromote = (DateOnly.FromDateTime(DateTime.Today).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
                    }
                    else if (!currentAchv.AprDate.HasValue)
                    {
                        currentAchv.DaysToPromote = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
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
    }
}
