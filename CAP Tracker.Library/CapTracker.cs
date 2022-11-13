public record CapTracker(int CAPID, string CurrAchv, string NameLast, string NameFirst, DateOnly JoinDate, DateOnly? NextApprovalDate, List<CapAchievemntRecord> CadetAchivements)
{
	public int NextApprovalDays { get; set; }
	public double? AvgDaysToPromote { get; set; }
}
const string FileName = @"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-11_11_2022.xlsx";
string BinFileName => $"{Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName))}.bin";
List<CapTracker> Tracker;
List<CAPTrackerData> Data;
SortedDictionary<string, Achievement> Achievements;
List<int> inactiveCadets = new List<int> { 666678, 646802, 655222, 578597, 578508, 629476, 671817, 592093, 630678, 658795, 675389, 600925, 649211, 675385, 653495, 653494, 624037, 683219, 674024, 675384 };

void GetTracker()
{


	Tracker = (from t in Data.Where(d => !inactiveCadets.Contains(d.CAPID))
			   group t by new { t.CAPID, t.NameLast, t.NameFirst, t.JoinDate } into g
			   let achievements = (from a in g select new CapAchievemntRecord(a.AchvName, a.AprDate) { PT = a.PhyFitTest.HasValue, LD = a.LeadLabDateP.HasValue || a.LeadershipInteractiveDate.HasValue, AE = a.AEDateP.HasValue || a.AEInteractiveDate.HasValue, Drill = a.DrillDate.HasValue, CD = a.CharacterDevelopment.HasValue }).ToList()
			   let currAchv = Achievements[achievements.Where(a => a.AprDate.HasValue).Last().AchvName].Rank
			   select new CapTracker(g.Key.CAPID, currAchv, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, achievements.All(a => a.AprDate.HasValue) ? null : g.Max(c => c.NextApprovalDate.Value), achievements)).ToList();


	foreach (var cadet in Tracker)
	{
		for (int i = 0; i < cadet.CadetAchivements.Count; i++)
		{
			var currentAchv = cadet.CadetAchivements[i];
			
			currentAchv.StartDate = currentAchv.AprDate.HasValue ? currentAchv.AprDate.Value : null;
			currentAchv.EndDate = cadet.CadetAchivements.Count() > i + 1 && cadet.CadetAchivements[i + 1].AprDate.HasValue ? cadet.CadetAchivements[i + 1].AprDate.Value : null;
			
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
				//currentAchv.DaysToPromote = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
				cadet.NextApprovalDays = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
			}
			else 
			{
				currentAchv.DaysToPromote = 0;
			}
		}
		cadet.AvgDaysToPromote = cadet.CadetAchivements.Any(ca => ca.EndDate.HasValue) ? cadet.CadetAchivements.Where(ca => ca.AprDate.HasValue && ca.EndDate.HasValue).Average(ca => ca.DaysToPromote) : null;
	}
}
void GetAchievements()
{
	Achievements = new SortedDictionary<string, Achievement>();
	Achievements.Add("Achievement 0", new Achievement("Achievement 0", "0", "C/AB", false, false));
	Achievements.Add("Achievement 1", new Achievement("Achievement 1", "1", "C/Amn", false, true));
	Achievements.Add("Achievement 2", new Achievement("Achievement 2", "2", "C/A1C", false, true));
	Achievements.Add("Achievement 3", new Achievement("Achievement 3", "3", "C/SrA", false, true));
	Achievements.Add("Wright Brothers", new Achievement("Wright Brothers", "Wright", "C/SSgt", false, false));
	Achievements.Add("Achievement 4", new Achievement("Achievement 4", "4", "C/TSgt", false, true));
	Achievements.Add("Achievement 5", new Achievement("Achievement 5", "5", "C/MSgt", false, true));
	Achievements.Add("Achievement 6", new Achievement("Achievement 6", "6", "C/SMSgt", false, true));
	Achievements.Add("Achievement 7", new Achievement("Achievement 7", "7", "C/CMSgt (1)", false, true));
	Achievements.Add("Achievement 8", new Achievement("Achievement 8", "8", "C/CMSgt (2)", true, true));
	Achievements.Add("Billy Mitchell", new Achievement("Billy Mitchell", "Mitchell", "C/2d Lt (1)", false, false));
	Achievements.Add("Achievement 9", new Achievement("Achievement 9", "9", "C/2d Lt (2)", true, true));
	Achievements.Add("Achievement 10", new Achievement("Achievement 10", "10", "C/1st Lt (1)", true, true));
	Achievements.Add("Achievement 11", new Achievement("Achievement 11", "11", "C/1st Lt (2)", true, true));
	Achievements.Add("Amelia Earhart", new Achievement("Amelia Earhart", "Earhart", "C/Capt (1)", false, false));
	Achievements.Add("Achievement 12", new Achievement("Achievement 12", "12", "C/Capt (2)", true, true));
	Achievements.Add("Achievement 13", new Achievement("Achievement 13", "13", "C/Capt (3)", true, true));
	Achievements.Add("Achievement 14", new Achievement("Achievement 14", "14", "C/Maj (1)", true, true));
	Achievements.Add("Achievement 15", new Achievement("Achievement 15", "15", "C/Maj (2)", true, true));
	Achievements.Add("Achievement 16", new Achievement("Achievement 16", "16", "C/Maj (3)", true, true));
	Achievements.Add("Gen Ira C Eaker", new Achievement("Gen Ira C Eaker", "Eaker", "C/Lt Col", false, false));
	Achievements.Add("Gen Carl A Spaatz", new Achievement("Gen Carl A Spaatz", "Spaatz", "C/Col", false, false));
}