﻿using CAP_Tracker.Library;
using CAP_Tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CAP_Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CadetsController : ControllerBase
{
    private readonly TrackerService trackerService;
    public CadetsController(TrackerService tracker) => trackerService = tracker;

    [HttpGet(Name = nameof(GetCadets))]
    [ProducesResponseType(typeof(IEnumerable<Cadet>), 200)]
    public IActionResult GetCadets() => Ok(trackerService.Cadets);

    [HttpGet("{capId:int}", Name = nameof(GetCadet))]
    [ProducesResponseType(typeof(Cadet), 200)]
    public IActionResult GetCadet(int capId)
    {
        var cadet = trackerService.GetCadet(capId);
        return cadet is null ? NotFound() : Ok(cadet);
    }
}
