namespace CAP_Tracker.Library;

public interface ITrackerService
{
    IEnumerable<Cadet> Cadets { get; }
    IEnumerable<CapTracker> Tracker { get; }
    IEnumerable<CAPTrackerData> Data { get; }

    Cadet? GetCadet(int capId);

    IEnumerable<CadetsByAchievement> GroupByAchievement();

    IEnumerable<CadetsByPhase> GroupByPhase();
}
