<Query Kind="Program">
  <NuGetReference>DocumentFormat.OpenXml</NuGetReference>
  <Namespace>DocumentFormat.OpenXml</Namespace>
  <Namespace>DocumentFormat.OpenXml.Spreadsheet</Namespace>
  <Namespace>DocumentFormat.OpenXml.Packaging</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	const string FileName = @"C:\Users\rafae\Downloads\Cadet_Full_Track_Report-11_19_2022.xlsx";
	var data = new List<CAPTrackerData>();

	using (var doc = SpreadsheetDocument.Open(FileName, false))
	{
		WorkbookPart workbookPart = doc.WorkbookPart;
		WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
		SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

		for (int r = 1; r < sheetData.Elements<Row>().Count(); r++)
		{
			var row = (Row)sheetData.ElementAt(r);
			int col = 0;

			//GetCellValue(workbookPart, row, col++);
			data.Add(new CAPTrackerData
			{
				CAPID = GetCellValue(workbookPart, row, col++).ToInt32(),
				NameLast = GetCellValue(workbookPart, row, col++),
				NameFirst = GetCellValue(workbookPart, row, col++),
				Email = GetCellValue(workbookPart, row, col++),
				AchvName = GetCellValue(workbookPart, row, col++),
				AprDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				JoinDate = GetCellValue(workbookPart, row, col++).ToDateOnly()!.Value,
				Region = GetCellValue(workbookPart, row, col++),
				Wing = GetCellValue(workbookPart, row, col++),
				Unit = GetCellValue(workbookPart, row, col++),
				PhyFitTest = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				LeadLabDateP = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				LeadLabScore = GetCellValue(workbookPart, row, col++).ToInt32(),
				AEDateP = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				AEScore = GetCellValue(workbookPart, row, col++).ToInt32(),
				AEModuleOrTest = GetCellValue(workbookPart, row, col++),
				CharacterDevelopment = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				ActivePart = GetCellValue(workbookPart, row, col++).ToBoolean(),
				ActiveParticipationDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				CadetOath = GetCellValue(workbookPart, row, col++).ToBoolean(),
				CadetOathDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				LeadershipExpectationsDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				UniformDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				SpecialActivityDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				NextApprovalDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				StaffServiceDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				OralPresentationDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				TechnicalWritingAssignmentDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				TechnicalWritingAssignment = GetCellValue(workbookPart, row, col++),
				DrillDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				DrillScore = GetCellValue(workbookPart, row, col++).ToInt32(),
				WelcomeCourseDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				EssayDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				SpeechDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				AEInteractiveDate = GetCellValue(workbookPart, row, col++).ToDateOnly(),
				AEInteractiveModule = GetCellValue(workbookPart, row, col++),
				LeadershipInteractiveDate = GetCellValue(workbookPart, row, col++).ToDateOnly()
			}
			);
			//var row = (Row)sheetData.ElementAt(r);
			//for (int c = 0; c < row.Elements<Cell>().Count(); c++)
			//{
			//	var cell = (Cell)row.ElementAt(c);
			//	Console.Write($"'{GetCellValue(workbookPart, cell)}' ");
			//}
		}
		data.Dump();
	}
}

public static string GetCellValue(WorkbookPart wbPart, Row row, int colIndex)
{
	string value = null;
	try
	{
		Cell theCell = (Cell)row.ElementAt(colIndex);
		if (theCell.InnerText.Length > 0)
		{
			value = theCell.InnerText;

			if (theCell.DataType != null)
			{
				switch (theCell.DataType.Value)
				{
					case CellValues.SharedString:

						var stringTable =
							wbPart.GetPartsOfType<SharedStringTablePart>()
							.FirstOrDefault();

						if (stringTable != null)
						{
							value =
								stringTable.SharedStringTable
								.ElementAt(int.Parse(value)).InnerText;
						}
						break;

					case CellValues.Boolean:
						switch (value)
						{
							case "0":
								value = "FALSE";
								break;
							default:
								value = "TRUE";
								break;
						}
						break;
				}
			}
		}
	}
	catch (Exception ex)
	{
	}
	return value;
}

public class CAPTrackerData
{
	public CAPTrackerData() { }
	public CAPTrackerData(int capId, string nameLast, string nameFirst, DateOnly joinDate, string achvName, DateOnly? aprDate, DateOnly? nextApprovalDate)
	{
		CAPID = capId;
		NameLast = nameLast;
		NameFirst = nameFirst;
		JoinDate = joinDate;
		AchvName = achvName;
		AprDate = aprDate;
		NextApprovalDate = nextApprovalDate;
	}
	public int? CAPID { get; set; }
	public string? NameLast { get; set; }
	public string? NameFirst { get; set; }
	public string? Email { get; set; }
	public string? AchvName { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? AprDate { get; set; }
	[JsonConverter(typeof(DateOnlyJsonConverter))]
	public DateOnly JoinDate { get; set; }
	public string? Region { get; set; }
	public string? Wing { get; set; }
	public string? Unit { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? PhyFitTest { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? LeadLabDateP { get; set; }
	public int? LeadLabScore { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? AEDateP { get; set; }
	public int? AEScore { get; set; }
	public string? AEModuleOrTest { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? CharacterDevelopment { get; set; }
	public bool? ActivePart { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? ActiveParticipationDate { get; set; }
	public bool? CadetOath { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? CadetOathDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? LeadershipExpectationsDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? UniformDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? SpecialActivityDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? NextApprovalDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? DrillDate { get; set; }
	public int? DrillScore { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? WelcomeCourseDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? EssayDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? SpeechDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? AEInteractiveDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? LeadershipInteractiveDate { get; set; }
	public string? AEInteractiveModule { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? StaffServiceDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? OralPresentationDate { get; set; }
	[JsonConverter(typeof(NullableDateOnlyJsonConverter))]
	public DateOnly? TechnicalWritingAssignmentDate { get; set; }
	public string? TechnicalWritingAssignment { get; set; }
}
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		DateOnly.Parse(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, DateOnly dateTimeValue, JsonSerializerOptions options) =>
		writer.WriteStringValue(dateTimeValue.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
}

public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
	public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.GetString()!.ToDateOnly();
	}

	public override void Write(Utf8JsonWriter writer, DateOnly? dateTimeValue, JsonSerializerOptions options)
	{
		writer.WriteStringValue(dateTimeValue!.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
	}
}
public static class ExtensionMethods
{
	public static DateOnly? ToDateOnly(this string value)
	{
		if (DateOnly.TryParse(value!, out var result))
		{
			return new DateOnly?(result);
		};
		return new DateOnly?();
	}
	public static int? ToInt32(this string value)
	{
		if (Int32.TryParse(value, out var result))
		{
			return new int?(result);
		}
		return new int?();
	}
	public static bool? ToBoolean(this string value)
	{
		if (Boolean.TryParse(value, out var result))
		{
			return new Boolean?(result);
		};
		return new Boolean?();
	}
}