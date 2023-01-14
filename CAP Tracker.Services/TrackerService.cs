using CAP_Tracker.Library;

namespace CAP_Tracker.Services;

public class TrackerService
{
    private readonly IEnumerable<CAPTrackerData> Data;
    private readonly IEnumerable<int>? InactiveCadets;

    private IEnumerable<CapTracker>? tracker;
    private IEnumerable<Cadet>? cadets;

    public IEnumerable<CapTracker> Tracker
    {
        get
        {
            tracker ??= CapTracker.GetTracker(Data, InactiveCadets);
            return tracker;
        }
    }
    public IEnumerable<Cadet> Cadets
    {
        get
        {
            cadets ??= Cadet.GetCadets(Data);
            return cadets;
        }
    }

    public TrackerService()
    {
        Data = new List<CAPTrackerData>();
        InactiveCadets = new List<int>();
    }
    public TrackerService(IEnumerable<CAPTrackerData> data, IEnumerable<int>? inactiveCadets = default)
    {
        Data = data;
        InactiveCadets = inactiveCadets;
    }
}
