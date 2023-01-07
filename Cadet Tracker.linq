<Query Kind="Program">
  <Reference Relative="CAP Tracker.Services\publish\CAP Tracker.Library.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\CAP Tracker.Services\publish\CAP Tracker.Library.dll</Reference>
  <Reference Relative="CAP Tracker.Services\publish\CAP Tracker.Services.dll">C:\Users\rafae\OneDrive\Code\CAP Tracker\CAP Tracker.Services\publish\CAP Tracker.Services.dll</Reference>
  <Namespace>CAP_Tracker.Library</Namespace>
  <Namespace>CAP_Tracker.Services</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//List<int> inactiveCadets = new List<int> { 578508, 592093, 624037, 683219 };
static List<int> inactiveCadets = null;

void Main()
{
	var inputContainer = new WrapPanel();
	var results = new DumpContainer();
	var filePath = new FilePicker() { Text = Util.LoadString("Last file path") ?? "" };
	var button = new Button("Analyse");

	inputContainer.Children.Add(filePath);
	inputContainer.Children.Add(button);
	
	
	//inputContainer.Content = button;
	inputContainer.Dump();
	results.Dump();

	button.Click += (sender, args) =>
	{
		if (!string.IsNullOrEmpty(filePath.Text))
		{

			AnalizeTracker(filePath.Text, results);
			Util.SaveString("Last file path", filePath.Text);
		}
		else if (!string.IsNullOrEmpty(filePath.Text))
		{
			
		}
	};
}

public static void AnalizeTracker(string trackerFileName, DumpContainer container)
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