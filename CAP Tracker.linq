<Query Kind="Program">
  <Reference Relative="publish\CAP Tracker.Library.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\publish\CAP Tracker.Library.dll</Reference>
  <Reference Relative="publish\CAP Tracker.Services.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\publish\CAP Tracker.Services.dll</Reference>
  <Namespace>CAP_Tracker.Library</Namespace>
  <Namespace>CAP_Tracker.Services</Namespace>
</Query>

const string FileName = @"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-11_22_2022.xlsx";
string BinFileName => $"{Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName))}.bin";
List<CapTracker> Tracker;
List<CAPTrackerData> Data;
SortedDictionary<string, Achievement> Achievements;
List<int> inactiveCadets = new List<int> { 666678, 646802, 655222, 578597, 578508, 629476, 671817, 592093, 630678, 658795, 675389, 600925, 649211, 675385, 653495, 653494, 624037, 683219, 674024, 675384 };

void Main()
{
	var clearCache = false;
	if (clearCache)
	{
		DataService.ClearCachedFile(BinFileName);
		return;
	}
	
	if (File.Exists(BinFileName))
	{
		Console.WriteLine("Loading from cached file");
		Data = DataService.DeserializeFile(BinFileName);
	}
	else
	{
		if (!File.Exists(FileName))
		{
			Console.WriteLine($"File '{FileName}' does not exist.");
			return;
		}
		Console.WriteLine("Loading from original file");
		Data = DataService.LoadFile(FileName);
		DataService.SerializeFile(Data, BinFileName);
	}
	Achievements = DataService.GetAchievements();

	var ts = new TrackerService(Data, Achievements, inactiveCadets);
	ts.GetTracker();
	
	ts.Tracker.Where(t => t.NameLast == "Delgado").OrderBy(t => t.AvgDaysToPromote).Dump();
}
