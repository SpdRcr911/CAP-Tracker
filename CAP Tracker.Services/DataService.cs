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
                AEResults = new AEResults (
                    GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                    GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}"),
                    GetCellValue(workbookPart, cells, $"{GetCellRef(colIndex++, rowIndex)}")),
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


}
