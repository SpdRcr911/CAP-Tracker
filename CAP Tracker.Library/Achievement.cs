namespace CAP_Tracker.Library;
public record Achievement(string CadetAchvID, string Name, string Grade, string Phase, bool NeedsAE, bool NeedsDrill, bool NeedsSDA, bool NeedsCD, Uri? Insignia);