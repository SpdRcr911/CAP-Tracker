<Query Kind="Program">
  <Reference Relative="CAP Tracker.Services\publish\CAP Tracker.Library.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\CAP Tracker.Services\publish\CAP Tracker.Library.dll</Reference>
  <Reference Relative="CAP Tracker.Services\publish\CAP Tracker.Services.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\CAP Tracker.Services\publish\CAP Tracker.Services.dll</Reference>
  <Namespace>CAP_Tracker.Library</Namespace>
  <Namespace>CAP_Tracker.Services</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

readonly List<int> inactiveCadets = new List<int> { 624037, 600925, 592093, 631353, 578508 };
//static List<int> inactiveCadets = null;
DumpContainer ResultContainer;
FilePicker FilePicker;

void Main()
{
	var inputContainer = new WrapPanel();
	ResultContainer = new DumpContainer();
	FilePicker = new FilePicker() { Text = Util.LoadString("Last file path") ?? "" };

	var dataOnly = new Button("Data Only");
	dataOnly.Click += DataOnlyClick;

	var groupByPhrase = new Button("Group by Phase");
	groupByPhrase.Click += GroupByPhaseClick;

	var groupByAchievement = new Button("Group by Achievement");
	groupByAchievement.Click += GroupByAchievementClick;

	var achievementGraph = new Button("Graph Achievements");
	achievementGraph.Click += AchievementGraphClick;

	var phaseGraph = new Button("Graph Phases");
	phaseGraph.Click += PhaseGraphClick;

	inputContainer.Children.Add(FilePicker);
	inputContainer.Children.Add(dataOnly);
	inputContainer.Children.Add(groupByPhrase);
	inputContainer.Children.Add(groupByAchievement);
	inputContainer.Children.Add(achievementGraph);
	inputContainer.Children.Add(phaseGraph);
	
	inputContainer.Dump();
	ResultContainer.Dump();

}

void DataOnlyClick(object sender, EventArgs e)
{
	if (!string.IsNullOrEmpty(FilePicker.Text))
	{
		DataOnly(FilePicker.Text, ResultContainer);
		Util.SaveString("Last file path", FilePicker.Text);
	}
}
void DataOnly(string trackerFileName, DumpContainer container)
{
	List<CAPTrackerData> Data;
	Data = DataService.LoadFile(trackerFileName);
	container.Content = Data;//.Where(d => d.AchvName == "Achievement 1");
}

void PhaseGraphClick(object sender, EventArgs e)
{
	if (!string.IsNullOrEmpty(FilePicker.Text))
	{
		PhaseGraph(FilePicker.Text, ResultContainer);
		Util.SaveString("Last file path", FilePicker.Text);
	}
}
void PhaseGraph(string trackerFileName, DumpContainer container)
{
	List<CAPTrackerData> Data;
	Data = DataService.LoadFile(trackerFileName);

	var service = new TrackerService(Data, inactiveCadets);

	var phaseGrp = (from phases in TrackerService.Achievements.Values.Select(v => v.Phase).Distinct()
					select new { Achievement = phases, Count = service.Tracker.Count(t => t.LastAchv.Phase == phases) });
	var winChart2 = phaseGrp.Chart(c => c.Achievement, c => c.Count).ToWindowsChart();
	winChart2.Titles.Add($"Squadron 144 - {DateTime.Now.ToShortDateString()}");
	winChart2.Titles.Add($"Cadets by Phase");
	foreach (var element in winChart2.Series)
	{
		element.IsValueShownAsLabel = true;
	}
	container.Content = winChart2.ToBitmap(800,600);
}

void AchievementGraphClick(object sender, EventArgs e)
{
	if (!string.IsNullOrEmpty(FilePicker.Text))
	{
		AchievementGraph(FilePicker.Text, ResultContainer);
		Util.SaveString("Last file path", FilePicker.Text);
	}
}
void AchievementGraph(string trackerFileName, DumpContainer container)
{
	List<CAPTrackerData> Data;
	Data = DataService.LoadFile(trackerFileName);

	var service = new TrackerService(Data, inactiveCadets);
	var achGrp = (from phases in TrackerService.Achievements
				  select new { Achievement = phases.Value.CadetAchvID, Count = service.Tracker.Count(t => t.LastAchv.CadetAchvID == phases.Value.CadetAchvID) });
	var winChart = achGrp.Chart(c => c.Achievement, c => c.Count).ToWindowsChart();
	winChart.Titles.Add($"Squadron 144 - {DateTime.Now.ToShortDateString()}");
	winChart.Titles.Add($"Cadets by Achievement");
	foreach (var element in winChart.Series)
	{
		element.IsValueShownAsLabel = true;
	}
	container.Content = winChart.ToBitmap(1200,600);
}

void GroupByAchievementClick(object sender, EventArgs e)
{
	if (!string.IsNullOrEmpty(FilePicker.Text))
	{
		GroupByAchievement(FilePicker.Text, ResultContainer);
		Util.SaveString("Last file path", FilePicker.Text);
	}
}
void GroupByAchievement(string trackerFileName, DumpContainer container)
{
	List<CAPTrackerData> Data;
	Data = DataService.LoadFile(trackerFileName);

	var service = new TrackerService(Data, inactiveCadets);
	container.Content = (from t in service.Tracker
						 join a in TrackerService.Achievements on t.LastAchv.CadetAchvID equals a.Value.CadetAchvID
						 let WorkingOn = t.CadetAchivements.Last()
						 let PromotionScore = Convert.ToInt16(t.NextApprovalDays >= 0) << 5 |
						                      Convert.ToInt16(WorkingOn.CD ?? true) << 4 |
											  Convert.ToInt16(WorkingOn.PT ?? true) << 3 |
											  Convert.ToInt16(WorkingOn.Drill ?? true) << 2 |
											  Convert.ToInt16(WorkingOn.AE ?? true) << 1 |
											  Convert.ToInt16(WorkingOn.LD ?? true) << 0
						 //where t.NextApprovalDays >= 100
						 orderby a.Key, PromotionScore descending, t.NextApprovalDays descending
						 select new
						 {
							 t.CAPID,
							 t.NameLast,
							 t.NameFirst,
							 t.JoinDate,
							 t.Email,
							 AvgDaysToPromote = (int?)t.AvgDaysToPromote,
							 t.LastAchv.CadetAchvID,
							 t.NextApprovalDate,
							 t.NextApprovalDays,
							 NextAchievement = WorkingOn.AchvName == "New Cadet" ? "Achievement 1" : WorkingOn.AchvName,
							 CD = WorkingOn.CD,
							 PT = WorkingOn.PT,
							 Drill = WorkingOn.Drill,
							 AE = WorkingOn.AE,
							 LD = WorkingOn.LD,
							 PromotionScore
						 });
}

void GroupByPhaseClick(object sender, EventArgs args)
{
	if (!string.IsNullOrEmpty(FilePicker.Text))
	{
		GroupByPhase(FilePicker.Text, ResultContainer);
		Util.SaveString("Last file path", FilePicker.Text);
	}
}
void GroupByPhase(string trackerFileName, DumpContainer container)
{
	List<CAPTrackerData> Data;
	Data = DataService.LoadFile(trackerFileName);

	var service = new TrackerService(Data, inactiveCadets);
	var dout = (from a in TrackerService.Achievements
				group a by a.Value.Phase into phases
				let achievements =
					(from p in phases
					 join t in service.Tracker on p.Value.CadetAchvID equals t.LastAchv.CadetAchvID into g
					 select new
					 {
						 p.Value.CadetAchvID,
						 Count = g.Count(),
						 p.Value.Grade,
						 Insignia = Util.Image(p.Value.Insignia, scaleMode: Util.ScaleMode.ResizeTo(width: default, height: 50)),
						 cadets = g.OrderByDescending(c => c.NextApprovalDays).Select(c => $"{c.NameLast}, {c.NameFirst} ({c.NextApprovalDays})").ToList()
					 })
				let count = achievements.Sum(a => a.cadets.Count())
				select new { Phase = phases.Key, Count = count, Achievements = achievements });

	container.Content = dout;
}

