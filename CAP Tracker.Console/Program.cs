// See https://aka.ms/new-console-template for more information
using CAP_Tracker.Services;
using CAP_Tracker.Library;

const string FileName = @"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-11_19_2022.xlsx";
string BinFileName = $"{Path.Combine(Path.GetDirectoryName(FileName)!, Path.GetFileNameWithoutExtension(FileName)!)}.bin";
// List<CapTracker> Tracker;
// List<CAPTrackerData> Data;
SortedDictionary<string, Achievement> Achievements;
List<int> inactiveCadets = new List<int> { 666678, 646802, 655222, 578597, 578508, 629476, 671817, 592093, 630678, 658795, 675389, 600925, 649211, 675385, 653495, 653494, 624037, 683219, 674024, 675384 };

// void GetTracker()
// {
//     Tracker = (from t in Data.Where(d => !inactiveCadets.Contains(d.CAPID))
//                group t by new { t.CAPID, t.NameLast, t.NameFirst, t.JoinDate } into g
//                let achievements = (from a in g select new CapAchievemntRecord(a.AchvName, a.AprDate) { PT = a.PhyFitTest.HasValue, LD = a.LeadLabDateP.HasValue || a.LeadershipInteractiveDate.HasValue, AE = a.AEDateP.HasValue || a.AEInteractiveDate.HasValue, Drill = a.DrillDate.HasValue, CD = a.CharacterDevelopment.HasValue }).ToList()
//                let currAchv = Achievements[achievements.Where(a => a.AprDate.HasValue).Last().AchvName].Rank
//                select new CapTracker(g.Key.CAPID, currAchv, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, achievements.All(a => a.AprDate.HasValue) ? null : g.Max(c => c.NextApprovalDate.Value), achievements)).ToList();


//     foreach (var cadet in Tracker)
//     {
//         for (int i = 0; i < cadet.CadetAchivements.Count; i++)
//         {
//             var currentAchv = cadet.CadetAchivements[i];

//             currentAchv.StartDate = currentAchv.AprDate.HasValue ? currentAchv.AprDate.Value : null;
//             currentAchv.EndDate = cadet.CadetAchivements.Count() > i + 1 && cadet.CadetAchivements[i + 1].AprDate.HasValue ? cadet.CadetAchivements[i + 1].AprDate.Value : null;

//             if (currentAchv.StartDate.HasValue && currentAchv.EndDate.HasValue)
//             {
//                 currentAchv.DaysToPromote = ((currentAchv.EndDate.HasValue ? currentAchv.EndDate.Value : DateOnly.FromDateTime(DateTime.Today)).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
//             }
//             else if (currentAchv.StartDate.HasValue && !currentAchv.EndDate.HasValue)
//             {
//                 currentAchv.DaysToPromote = (DateOnly.FromDateTime(DateTime.Today).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
//             }
//             else if (!currentAchv.AprDate.HasValue)
//             {
//                 //currentAchv.DaysToPromote = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
//                 cadet.NextApprovalDays = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
//             }
//             else
//             {
//                 currentAchv.DaysToPromote = 0;
//             }
//         }
//         cadet.AvgDaysToPromote = cadet.CadetAchivements.Any(ca => ca.EndDate.HasValue) ? cadet.CadetAchivements.Where(ca => ca.AprDate.HasValue && ca.EndDate.HasValue).Average(ca => ca.DaysToPromote) : null;
//     }
// }
Achievements = DataService.GetAchievements();
//Achievements.Dump();

if (File.Exists(BinFileName))
{
    DataService.DeserializeFile(BinFileName);
}
else
{
    if (!File.Exists(FileName))
    {
        Console.WriteLine($"File '{FileName}' does not exist.");
        return;
    }
    DataService.LoadFile(FileName);
}