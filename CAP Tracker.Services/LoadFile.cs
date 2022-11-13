using CAP_Tracker.Library;
namespace CAP_Tracker.Services;
public class DataService
{
    void LoadFile()
{
	var data = new List<CAPTrackerData>();
	var excel = new Excel.Application();
	var workbook = excel.Workbooks.Open(FileName);
	Excel.Worksheet worksheet = workbook.Worksheets["Cadet Promotions Full Track"];

	for (int i = 2; i <= worksheet.UsedRange.Rows.Count; i++)
	{
		int j = 1;
		data.Add(new CAPTrackerData
		{
			CAPID = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
			NameLast = worksheet.Cells[i, j++].Value2,
			NameFirst = worksheet.Cells[i, j++].Value2,
			Email = worksheet.Cells[i, j++].Value2,
			AchvName = worksheet.Cells[i, j++].Value2,
			AprDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			JoinDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly().Value,
			Region = worksheet.Cells[i, j++].Value2,
			Wing = worksheet.Cells[i, j++].Value2,
			Unit = worksheet.Cells[i, j++].Value2,
			PhyFitTest = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			LeadLabDateP = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			LeadLabScore = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
			AEDateP = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			AEScore = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
			AEModuleOrTest = worksheet.Cells[i, j++].Value2,
			CharacterDevelopment = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			ActivePart = worksheet.Cells[i, j++].Value2,
			ActiveParticipationDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			CadetOath = worksheet.Cells[i, j++].Value2,
			CadetOathDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			LeadershipExpectationsDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			UniformDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			SpecialActivityDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			NextApprovalDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			StaffServiceDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			OralPresentationDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			TechnicalWritingAssignmentDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			TechnicalWritingAssignment = worksheet.Cells[i, j++].Value2,
			DrillDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			DrillScore = Convert.ToInt32(worksheet.Cells[i, j++].Value2),
			WelcomeCourseDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			EssayDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			SpeechDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			AEInteractiveDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly(),
			AEInteractiveModule = worksheet.Cells[i, j++].Value2,
			LeadershipInteractiveDate = ((string)worksheet.Cells[i, j++].Value2).ToDateOnly()
		}
		);
	}
	workbook.Close(false, FileName);
	var zeroData = (from c in data
					group c by new { c.CAPID, c.NameLast, c.NameFirst, c.JoinDate } into g
					select new CAPTrackerData(g.Key.CAPID, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, "Achievement 0", g.Key.JoinDate, g.Key.JoinDate)).ToList();

	zeroData.AddRange(data);
	Data = zeroData;
}
}
