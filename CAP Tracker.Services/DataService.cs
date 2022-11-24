using CAP_Tracker.Library;
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

        using var doc = SpreadsheetDocument.Open(fileName, false);
        WorkbookPart workbookPart = doc.WorkbookPart!;
        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

        for (int r = 1; r < sheetData.Elements<Row>().Count(); r++)
        {
            var row = (Row)sheetData.ElementAt(r);
            int col = 0;

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
        }
        var zeroData = (from c in data
                        group c by new { c.CAPID, c.NameLast, c.NameFirst, c.JoinDate } into g
                        select new CAPTrackerData(g.Key.CAPID, g.Key.NameLast, g.Key.NameFirst, g.Key.JoinDate, "Achievement 0", g.Key.JoinDate, g.Key.JoinDate)).ToList();

        zeroData.AddRange(data);
        data = zeroData;

        return data;
    }

    public static string? GetCellValue(WorkbookPart wbPart, Row row, int colIndex)
    {
        string value = string.Empty;
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
                            value = value switch
                            {
                                "0" => "FALSE",
                                _ => "TRUE",
                            };
                            break;
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return value;
    }

    public static List<CAPTrackerData> DeserializeFile(string binFileName)
    {
        var resultBytes = File.ReadAllBytes(binFileName);
        return JsonSerializer.Deserialize<List<CAPTrackerData>>(new ReadOnlySpan<byte>(resultBytes)) ?? new List<CAPTrackerData>();
    }
    public static void ClearCachedFile(string binFileName)
    {
        if (File.Exists(binFileName))
        {
            File.Delete(binFileName);
        }
    }

    public static void SerializeFile(List<CAPTrackerData> data, string binFileName)
    {
        var resultBytes = JsonSerializer.SerializeToUtf8Bytes(data,
                            new JsonSerializerOptions { WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

        File.WriteAllBytes(binFileName, resultBytes);
    }

    public static SortedDictionary<string, Achievement> GetAchievements()
        => new()
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
