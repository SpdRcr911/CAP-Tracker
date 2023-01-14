namespace CAP_Tracker.Library;
public record Achievement(string CadetAchvID, string Name, string Grade, string Phase, bool NeedsAE, bool NeedsDrill, bool NeedsSDA, bool NeedsCD, Uri? Insignia)
{
    public static readonly SortedDictionary<int, Achievement> All = new()
    {
        { 0, new Achievement("New Cadet", "0", "C/AB", "Trial", false, false, false, false, null) },
        { 1, new Achievement("Achievement 1", "1", "C/Amn", "PHASE I - The Learning Phase", false, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/curry_insig_F3AB50D13AA8E.jpg")) },
        { 2, new Achievement("Achievement 2", "2", "C/A1C", "PHASE I - The Learning Phase", true, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/arnold_insig_65858DA05810F.jpg")) },
        { 3, new Achievement("Achievement 3", "3", "C/SrA", "PHASE I - The Learning Phase", true, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/feik_insig_2CBC98B37573A.jpg")) },
        { 4, new Achievement("Wright Brothers", "Wright", "C/SSgt", "PHASE I - The Learning Phase", false, true, false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/wright_insig_48E8C7C1AA1B1.jpg")) },
        { 5, new Achievement("Achievement 4", "4", "C/TSgt", "PHASE II - The Leadership Phase", true, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/EVR_insig_A14EB349FFF79.jpg")) },
        { 6, new Achievement("Achievement 5", "5", "C/MSgt", "PHASE II - The Leadership Phase", true, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/lindburgh_insig_FEADDF248147A.jpg")) },
        { 7, new Achievement("Achievement 6", "6", "C/SMSgt", "PHASE II - The Leadership Phase", true, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/doolittle_insig_3703604194B56.jpg")) },
        { 8, new Achievement("Achievement 7", "7", "C/CMSgt (1)", "PHASE II - The Leadership Phase", true, true, false, true, new Uri("https://www.gocivilairpatrol.com/media/cms/goddard_armstrong_insig_3F44AA2E9617C.jpg")) },
        { 9, new Achievement("Achievement 8", "8", "C/CMSgt (2)", "PHASE II - The Leadership Phase", true, true, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/goddard_armstrong_insig_3F44AA2E9617C.jpg")) },
        { 10, new Achievement("Billy Mitchell", "Mitchell", "C/2d Lt (1)", "PHASE II - The Leadership Phase", true, false, false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_1b_6C4760028DE68.gif")) },
        { 11, new Achievement("Achievement 9", "9", "C/2d Lt (2)", "PHASE III - The Command Phase", true, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_1b_6C4760028DE68.gif")) },
        { 12, new Achievement("Achievement 10", "10", "C/1st Lt (1)", "PHASE III - The Command Phase", true, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_2b_6558A9577C5EA.jpg")) },
        { 13, new Achievement("Achievement 11", "11", "C/1st Lt (2)", "PHASE III - The Command Phase", true, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_2b_6558A9577C5EA.jpg")) },
        { 14, new Achievement("Amelia Earhart", "Earhart", "C/Capt (1)", "PHASE III - The Command Phase", false, false, false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_3b_6A8431B7E1FD1.gif")) },
        { 15, new Achievement("Achievement 12", "12", "C/Capt (2)", "PHASE IV - The Executive Phase", false, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_3b_6A8431B7E1FD1.gif")) },
        { 16, new Achievement("Achievement 13", "13", "C/Capt (3)", "PHASE IV - The Executive Phase", false, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_3b_6A8431B7E1FD1.gif")) },
        { 17, new Achievement("Achievement 14", "14", "C/Maj (1)", "PHASE IV - The Executive Phase", true, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_4b_35E3F2B152B0B.jpg")) },
        { 18, new Achievement("Achievement 15", "15", "C/Maj (2)", "PHASE IV - The Executive Phase", true, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_4b_35E3F2B152B0B.jpg")) },
        { 19, new Achievement("Achievement 16", "16", "C/Maj (3)", "PHASE IV - The Executive Phase", true, false, true, true, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_4b_35E3F2B152B0B.jpg")) },
        { 20, new Achievement("Gen Ira C Eaker", "Eaker", "C/Lt Col", "PHASE IV - The Executive Phase", false, false, false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_5b_E75C6247F7174.jpg")) },
        { 21, new Achievement("Gen Carl A Spaatz", "Spaatz", "C/Col", "Pinnacle", true, false, false, false, new Uri("https://www.gocivilairpatrol.com/media/cms/Cadet_Officer_6b_F03B21162A70A.jpg")) }
    };
}
