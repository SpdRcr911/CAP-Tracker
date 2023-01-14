using CAP_Tracker.Library;
using CAP_Tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAP_Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CadetController : ControllerBase
{
    TrackerService trackerService { get; set; }
    public CadetController(TrackerService tracker)
    {
        trackerService = tracker;
    }

    [HttpGet(Name = "GetCadets")]
    [ProducesResponseType(typeof(IEnumerable<Cadet>),200)]
    public IActionResult Get()
    {
        return Ok(trackerService.Cadets);
    }
}
