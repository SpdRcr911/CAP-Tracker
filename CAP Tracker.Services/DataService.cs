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
            int colIndex = 1;
            var rowIndex = row.RowIndex!.Value;
            var cells = row.Elements<Cell>();

            data.Add(new CAPTrackerData
            {
                CAPID = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToInt32(),
                NameLast = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                NameFirst = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                Email = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                AchvName = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                AprDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                JoinDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly()!.Value,
                Region = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                Wing = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                Unit = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                PhyFitTest = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                LeadLabDateP = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                LeadLabScore = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToInt32(),
                AEDateP = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                AEScore = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToInt32(),
                AEModuleOrTest = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                CharacterDevelopment = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                ActivePart = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToBoolean(),
                ActiveParticipationDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                CadetOath = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToBoolean(),
                CadetOathDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                LeadershipExpectationsDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                UniformDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                SpecialActivityDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                NextApprovalDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                StaffServiceDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                OralPresentationDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                TechnicalWritingAssignmentDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                TechnicalWritingAssignment = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                DrillDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                DrillScore = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToInt32(),
                WelcomeCourseDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                EssayDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                SpeechDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                AEInteractiveDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly(),
                AEInteractiveModule = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                LeadershipInteractiveDate = GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}").ToDateOnly()
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

    private static string? GetCellValue(WorkbookPart wbPart, IEnumerable<Cell> cells, string cellRef)
    {
        string value = string.Empty;
        Cell? theCell = cells.FirstOrDefault(c => c.CellReference == cellRef);

        if (theCell != null && theCell.InnerText.Length > 0)
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
                            value = stringTable.SharedStringTable
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
        return value;
    }
    private static string GetCellRef(long iCol, long? iRow = default)
    {
        long a, b;
        string GetCellRef = string.Empty;
        while (iCol > 0)
        {
            a = (int)((iCol - 1) / 26);
            b = (iCol - 1) % 26;
            GetCellRef = (char)(b + 65) + GetCellRef;
            iCol = a;
        }
        return $"{GetCellRef}{iRow}";
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
