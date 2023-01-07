<Query Kind="Program">
  <Reference Relative="publish\CAP Tracker.Library.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\publish\CAP Tracker.Library.dll</Reference>
  <Reference Relative="publish\CAP Tracker.Services.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\publish\CAP Tracker.Services.dll</Reference>
  <Namespace>CAP_Tracker.Library</Namespace>
  <Namespace>CAP_Tracker.Services</Namespace>
</Query>

const string FileName = @"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-1_4_2023.xlsx";
string BinFileName => $"{Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName))}.bin";
List<CAPTrackerData> Data;
//List<int> inactiveCadets = new List<int> { 578508, 592093, 624037, 683219 };
List<int> inactiveCadets = null;

void Main()
{
	//	var clearCache = false;
	//	if (clearCache)
	//	{
	//		DataService.ClearCachedFile(BinFileName);
	//		return;
	//	}
	//
	//	if (File.Exists(BinFileName))
	//	{
	//		Console.WriteLine("Loading from cached file");
	//		Data = DataService.DeserializeFile(BinFileName);
	//	}
	//	else
	//	{
	//		if (!File.Exists(FileName))
	//		{
	//			Console.WriteLine($"File '{FileName}' does not exist.");
	//			return;
	//		}
	//		Console.WriteLine("Loading from original file");
	//		Data = DataService.LoadFile(FileName);
	//		DataService.SerializeFile(Data, BinFileName);
	//	}
	Data = DataService.LoadFile(FileName);

	Data.Where(t => t.CAPID == 669070).Dump();
	//return;
	//Console.ReadLine();
	var ts = new TrackerService(Data, inactiveCadets);
	//ts.Tracker.Dump();
	//return;
	var dout = (from a in TrackerService.Achievements
	 group a by a.Value.Phase into phases
	 let achievements = 
	 	(from p in phases
		  join t in ts.Tracker on p.Value.CadetAchvID equals t.LastAchv.CadetAchvID into g
		  select new
		  {
			  p.Value.CadetAchvID,
			  Count = g.Count(),
			  p.Value.Grade,
			  Insignia = Util.Image(p.Value.Insignia, scaleMode: Util.ScaleMode.ResizeTo(width: default, height: 50)),
			  cadets = g.OrderByDescending(c => c.NextApprovalDays).Select(c => $"{c.NameLast}, {c.NameFirst} ({c.NextApprovalDays})").ToList() })
	 let count = achievements.Sum(a => a.cadets.Count())
	 select new { Phase = phases.Key, Count = count, Achievements = achievements }).Dump(collapseTo: 3);

	//LINQPad.Util.ToHtmlString(true, dout).Dump();
	//ts.Dump();
	//(from a in TrackerService.Achievements
	// join t in ts.Tracker.Where(t => t.CAPID == 627818) on a.Value.CadetAchvID equals t.LastAchv.CadetAchvID into g
	// select new { a.Key, a.Value.CadetAchvID, a.Value.Phase, count = g.Count(), cadets = g }).Dump();
}
