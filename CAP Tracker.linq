<Query Kind="Program">
  <Reference Relative="publish\CAP Tracker.Library.dll">C:\Code\CAP-Tracker\publish\CAP Tracker.Library.dll</Reference>
  <Reference Relative="publish\CAP Tracker.Services.dll">C:\Code\CAP-Tracker\publish\CAP Tracker.Services.dll</Reference>
  <Reference Relative="publish\Microsoft.Office.Interop.Excel.dll">C:\Code\CAP-Tracker\publish\Microsoft.Office.Interop.Excel.dll</Reference>
  <NuGetReference>DocumentFormat.OpenXml</NuGetReference>
  <Namespace>CAP_Tracker.Library</Namespace>
  <Namespace>CAP_Tracker.Services</Namespace>
  <Namespace>Excel = Microsoft.Office.Interop.Excel</Namespace>
</Query>

const string FileName = @"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-11_19_2022.xlsx";
string BinFileName => $"{Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName))}.bin";
List<CapTracker> Tracker;
List<CAPTrackerData> Data;
SortedDictionary<string, Achievement> Achievements;
List<int> inactiveCadets = new List<int> { 666678, 646802, 655222, 578597, 578508, 629476, 671817, 592093, 630678, 658795, 675389, 600925, 649211, 675385, 653495, 653494, 624037, 683219, 674024, 675384 };

void Main()
{
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
		DataService.LoadFile(FileName).Dump();
	}
	
	//DataService.DeserializeFile();
	//DeserializeFile();
	//Data.Dump();
	//LoadFile();
	//SerializeFile();

	

	//GetTracker();

	//Tracker.OrderBy(t => t.AvgDaysToPromote).Dump();
}
//
//void GetTracker()
//{
//
//
//	Tracker = (from t in Data.Where(d => !inactiveCadets.Contains(d.CAPID))
//			   group t by new { t.CAPID, t.NameLast, t.NameFirst, t.JoinDate } into g
//			   let achievements = (from a in g select new CapAchievemntRecord(a.AchvName, a.AprDate) { PT = a.PhyFitTest.HasValue, LD = a.LeadLabDateP.HasValue || a.LeadershipInteractiveDate.HasValue, AE = a.AEDateP.HasValue || a.AEInteractiveDate.HasValue, Drill = a.DrillDate.HasValue, CD = a.CharacterDevelopment.HasValue }).ToList()
//			   let currAchv = Achievements[achievements.Where(a => a.AprDate.HasValue).Last().AchvName].Rank
//			   select new CapTracker(g.Key.CAPID, currAchv, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, achievements.All(a => a.AprDate.HasValue) ? null : g.Max(c => c.NextApprovalDate.Value), achievements)).ToList();
//
//
//	foreach (var cadet in Tracker)
//	{
//		for (int i = 0; i < cadet.CadetAchivements.Count; i++)
//		{
//			var currentAchv = cadet.CadetAchivements[i];
//			
//			currentAchv.StartDate = currentAchv.AprDate.HasValue ? currentAchv.AprDate.Value : null;
//			currentAchv.EndDate = cadet.CadetAchivements.Count() > i + 1 && cadet.CadetAchivements[i + 1].AprDate.HasValue ? cadet.CadetAchivements[i + 1].AprDate.Value : null;
//			
//			if (currentAchv.StartDate.HasValue && currentAchv.EndDate.HasValue)
//			{
//				currentAchv.DaysToPromote = ((currentAchv.EndDate.HasValue ? currentAchv.EndDate.Value : DateOnly.FromDateTime(DateTime.Today)).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
//			}
//			else if (currentAchv.StartDate.HasValue && !currentAchv.EndDate.HasValue)
//			{
//				currentAchv.DaysToPromote = (DateOnly.FromDateTime(DateTime.Today).ToDateTime(new TimeOnly()) - currentAchv.StartDate.Value.ToDateTime(new TimeOnly())).Days;
//			}
//			else if (!currentAchv.AprDate.HasValue)
//			{
//				//currentAchv.DaysToPromote = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
//				cadet.NextApprovalDays = (DateTime.Today - cadet.NextApprovalDate.Value.ToDateTime(new TimeOnly())).Days;
//			}
//			else 
//			{
//				currentAchv.DaysToPromote = 0;
//			}
//		}
//		cadet.AvgDaysToPromote = cadet.CadetAchivements.Any(ca => ca.EndDate.HasValue) ? cadet.CadetAchivements.Where(ca => ca.AprDate.HasValue && ca.EndDate.HasValue).Average(ca => ca.DaysToPromote) : null;
//	}
//}
//
//record CapTracker(int CAPID, string CurrAchv, string NameLast, string NameFirst, DateOnly JoinDate, DateOnly? NextApprovalDate, List<CapAchievemntRecord> CadetAchivements)
//{
//	public int NextApprovalDays { get; set; }
//	public double? AvgDaysToPromote { get; set; }
//}
//record CapAchievemntRecord(string AchvName, DateOnly? AprDate)
//{
//	public int DaysToPromote { get; set; }
//	public DateOnly? StartDate { get; set; }
//	public DateOnly? EndDate { get; set; }
//	public bool? PT { get; set; }
//	public bool? LD { get; set; }
//	public bool? AE { get; set; }
//	public bool? Drill { get; set; }
//	public bool? CD { get; set; }
//}
//record Achievement(string CadetAchvID, string Name, string Rank, bool NeedsSDA, bool NeedsCD);
//void LoadFile()
//{
//	var data = new List<CAPTrackerData>();
//	var excel = new Excel.Application();
//	var workbook = excel.Workbooks.Open(FileName);
//	Excel.Worksheet worksheet = workbook.Worksheets["Cadet Promotions Full Track"];
//
//	for (int i = 2; i <= worksheet.UsedRange.Rows.Count; i++)
//	{
//		int j = 1;
//		data.Add(new CAPTrackerData
//		{
//			CAPID = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
//			NameLast = worksheet.Cells[i, j++].Value2,
//			NameFirst = worksheet.Cells[i, j++].Value2,
//			Email = worksheet.Cells[i, j++].Value2,
//			AchvName = worksheet.Cells[i, j++].Value2,
//			AprDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			JoinDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly().Value,
//			Region = worksheet.Cells[i, j++].Value2,
//			Wing = worksheet.Cells[i, j++].Value2,
//			Unit = worksheet.Cells[i, j++].Value2,
//			PhyFitTest = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			LeadLabDateP = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			LeadLabScore = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
//			AEDateP = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			AEScore = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
//			AEModuleOrTest = worksheet.Cells[i, j++].Value2,
//			CharacterDevelopment = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			ActivePart = worksheet.Cells[i, j++].Value2,
//			ActiveParticipationDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			CadetOath = worksheet.Cells[i, j++].Value2,
//			CadetOathDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			LeadershipExpectationsDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			UniformDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			SpecialActivityDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			NextApprovalDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			StaffServiceDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			OralPresentationDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			TechnicalWritingAssignmentDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			TechnicalWritingAssignment = worksheet.Cells[i, j++].Value2,
//			DrillDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			DrillScore = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
//			WelcomeCourseDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			EssayDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			SpeechDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			AEInteractiveDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
//			AEInteractiveModule = worksheet.Cells[i, j++].Value2,
//			LeadershipInteractiveDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly()
//		}
//		);
//	}
//	workbook.Close(false, FileName);
//	var zeroData = (from c in data
//					group c by new { c.CAPID, c.NameLast, c.NameFirst, c.JoinDate } into g
//					select new CAPTrackerData(g.Key.CAPID, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, "Achievement 0", g.Key.JoinDate, g.Key.JoinDate)).ToList();
//
//	zeroData.AddRange(data);
//	Data = zeroData;
//}
//class CAPTrackerData
//{
//	public CAPTrackerData() {}
//	public CAPTrackerData(int capId, string nameLast, string nameFirst, DateOnly joinDate, string achvName, DateOnly? aprDate, DateOnly? nextApprovalDate)
//	{
//		CAPID = capId;
//		NameLast = nameLast;
//		NameFirst = nameFirst;
//		JoinDate = joinDate;
//		AchvName = achvName;
//		AprDate = aprDate;
//		NextApprovalDate = nextApprovalDate;
//	}
//	public int CAPID { get; set; }
//	public string NameLast { get; set; }
//	public string NameFirst { get; set; }
//	public string Email { get; set; }
//	public string AchvName { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? AprDate { get; set; }
//	[JsonConverter(typeof(DateOnlyJsonConverter))]
//	public DateOnly JoinDate { get; set; }
//	public string Region { get; set; }
//	public string Wing { get; set; }
//	public string Unit { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? PhyFitTest { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? LeadLabDateP { get; set; }
//	public int LeadLabScore { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? AEDateP { get; set; }
//	public int AEScore { get; set; }
//	public string AEModuleOrTest { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? CharacterDevelopment { get; set; }
//	public bool ActivePart { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? ActiveParticipationDate { get; set; }
//	public bool CadetOath { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? CadetOathDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? LeadershipExpectationsDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? UniformDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? SpecialActivityDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? NextApprovalDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? DrillDate { get; set; }
//	public int DrillScore { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? WelcomeCourseDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? EssayDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? SpeechDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? AEInteractiveDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? LeadershipInteractiveDate { get; set; }
//	public string AEInteractiveModule { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? StaffServiceDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? OralPresentationDate { get; set; }
//	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
//	public DateOnly? TechnicalWritingAssignmentDate { get; set; }
//	public string TechnicalWritingAssignment { get; set; }
//}
//class DateOnlyJsonConverter : JsonConverter<DateOnly>
//{
//	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
//		DateOnly.Parse(reader.GetString()!);
//
//	public override void Write(Utf8JsonWriter writer, DateOnly dateTimeValue, JsonSerializerOptions options) =>
//		writer.WriteStringValue(dateTimeValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
//}
//class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
//{
//	public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//	{
//		return reader.GetString()!.ToDateOnly();
//	}
//
//	public override void Write(Utf8JsonWriter writer, DateOnly? dateTimeValue, JsonSerializerOptions options)
//	{
//		writer.WriteStringValue(dateTimeValue.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
//	}
//}
//static class StringHelper
//{
//	public static DateOnly? ToDateOnly(this string value)
//	{
//		if (DateOnly.TryParse(value!, out var result))
//		{
//			return new DateOnly?(result);
//		};
//		return new DateOnly?();
//	}
//}