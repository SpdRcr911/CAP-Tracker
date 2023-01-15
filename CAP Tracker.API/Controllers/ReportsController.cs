using CAP_Tracker.Library;
using CAP_Tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAP_Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly TrackerService trackerService;
    public ReportsController(TrackerService tracker) => trackerService = tracker;

    [HttpGet("groupByPhase", Name = nameof(GroupByPhase))]
    [ProducesResponseType(typeof(IEnumerable<Cadet>), 200)]
    public IActionResult GroupByPhase() => Ok(trackerService.GroupByPhase());

    [HttpGet("groupByAchievement", Name = nameof(GroupByAchievement))]
    [ProducesResponseType(typeof(IEnumerable<Cadet>), 200)]
    public IActionResult GroupByAchievement() => Ok(trackerService.GroupByPhase());
}