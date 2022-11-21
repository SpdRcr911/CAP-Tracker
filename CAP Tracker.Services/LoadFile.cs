using CAP_Tracker.Library;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.Json;
using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CAP_Tracker.Services;
public class DataService
{
    public static List<CAPTrackerData> LoadFile(string fileName)
    {
        var data = new List<CAPTrackerData>();

        using (var doc = SpreadsheetDocument.Open(fileName, false))
        {
            WorkbookPart workbookPart = doc.WorkbookPart!;
            WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            for (int r = 1; r < sheetData.Elements<Row>().Count(); r++)
            {
                var row = (Row)sheetData.ElementAt(r);
                int col = 0;

                data.Add(new CAPTrackerData
                {
                    CAPID = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToInt32(),
                    NameLast = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    NameFirst = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    Email = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    AchvName = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    AprDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    JoinDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly()!.Value,
                    Region = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    Wing = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    Unit = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    PhyFitTest = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    LeadLabDateP = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    LeadLabScore = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToInt32(),
                    AEDateP = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    AEScore = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToInt32(),
                    AEModuleOrTest = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    CharacterDevelopment = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    ActivePart = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToBoolean(),
                    ActiveParticipationDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    CadetOath = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToBoolean(),
                    CadetOathDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    LeadershipExpectationsDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    UniformDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    SpecialActivityDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    NextApprovalDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    StaffServiceDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    OralPresentationDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    TechnicalWritingAssignmentDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    TechnicalWritingAssignment = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    DrillDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    DrillScore = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToInt32(),
                    WelcomeCourseDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    EssayDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    SpeechDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    AEInteractiveDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly(),
                    AEInteractiveModule = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)),
                    LeadershipInteractiveDate = GetCellValue(workbookPart, (Cell)row.ElementAt(col++)).ToDateOnly()
                }
                );
                //var row = (Row)sheetData.ElementAt(r);
                //for (int c = 0; c < row.Elements<Cell>().Count(); c++)
                //{
                //	var cell = (Cell)row.ElementAt(c);
                //	Console.Write($"'{GetCellValue(workbookPart, cell)}' ");
                //}
            }
            return data;
        }
    }

    public static string GetCellValue(WorkbookPart wbPart, Cell theCell)
    {
        string value = string.Empty;

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

        return value;
    }


    static public List<CAPTrackerData> DeserializeFile(string binFileName)
    {
        var resultBytes = File.ReadAllBytes(binFileName);
        return JsonSerializer.Deserialize<List<CAPTrackerData>>(new ReadOnlySpan<byte>(resultBytes)) ?? new List<CAPTrackerData>();
    }

    static public void SerializeFile(List<CAPTrackerData> data, string binFileName)
    {
        var resultBytes = JsonSerializer.SerializeToUtf8Bytes(data,
                            new JsonSerializerOptions { WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

        File.WriteAllBytes(binFileName, resultBytes);
    }

    public static SortedDictionary<string, Achievement> GetAchievements()
        => new SortedDictionary<string, Achievement>
        {
            { "Achievement 0", new Achievement("Achievement 0", "0", "C/AB", false, false) },
            { "Achievement 1", new Achievement("Achievement 1", "1", "C/Amn", false, true) },
            { "Achievement 2", new Achievement("Achievement 2", "2", "C/A1C", false, true) },
            { "Achievement 3", new Achievement("Achievement 3", "3", "C/SrA", false, true) },
            { "Wright Brothers", new Achievement("Wright Brothers", "Wright", "C/SSgt", false, false) },
            { "Achievement 4", new Achievement("Achievement 4", "4", "C/TSgt", false, true) },
            { "Achievement 5", new Achievement("Achievement 5", "5", "C/MSgt", false, true) },
            { "Achievement 6", new Achievement("Achievement 6", "6", "C/SMSgt", false, true) },
            { "Achievement 7", new Achievement("Achievement 7", "7", "C/CMSgt (1)", false, true) },
            { "Achievement 8", new Achievement("Achievement 8", "8", "C/CMSgt (2)", true, true) },
            { "Billy Mitchell", new Achievement("Billy Mitchell", "Mitchell", "C/2d Lt (1)", false, false) },
            { "Achievement 9", new Achievement("Achievement 9", "9", "C/2d Lt (2)", true, true) },
            { "Achievement 10", new Achievement("Achievement 10", "10", "C/1st Lt (1)", true, true) },
            { "Achievement 11", new Achievement("Achievement 11", "11", "C/1st Lt (2)", true, true) },
            { "Amelia Earhart", new Achievement("Amelia Earhart", "Earhart", "C/Capt (1)", false, false) },
            { "Achievement 12", new Achievement("Achievement 12", "12", "C/Capt (2)", true, true) },
            { "Achievement 13", new Achievement("Achievement 13", "13", "C/Capt (3)", true, true) },
            { "Achievement 14", new Achievement("Achievement 14", "14", "C/Maj (1)", true, true) },
            { "Achievement 15", new Achievement("Achievement 15", "15", "C/Maj (2)", true, true) },
            { "Achievement 16", new Achievement("Achievement 16", "16", "C/Maj (3)", true, true) },
            { "Gen Ira C Eaker", new Achievement("Gen Ira C Eaker", "Eaker", "C/Lt Col", false, false) },
            { "Gen Carl A Spaatz", new Achievement("Gen Carl A Spaatz", "Spaatz", "C/Col", false, false) },
    };

}
