using CAP_Tracker.Library;
using CAP_Tracker.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

namespace CAP_Tracker.App.Pages;

public partial class CadetOverview
{
    public IEnumerable<Cadet>? Cadets { get; set; } = default;

    [Inject]
    protected ILogger<CadetOverview> Logger { get; set; } = default!;
    [Inject]
    protected HttpClient Http { get; set; } = default!;
    protected async override Task OnInitializedAsync()
    {
        Cadets = await Http.GetFromJsonAsync<IEnumerable<Cadet>>("data/cadets.json");
        Logger.LogDebug($"Cadets: {Cadets?.Count()}");
    }
}
