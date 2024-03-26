using GPMigratorApp.Models;
using Microsoft.AspNetCore.Mvc;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;

public class GPDataQueryController : Controller
{
    private readonly IGPDataQueryService _gpDataQueryService;

    public GPDataQueryController(IGPDataQueryService gpDataQueryService)
    {
        _gpDataQueryService = gpDataQueryService;
    }

    public async Task<IActionResult> Index(int age, int yearsSinceLastReading,CancellationToken cancellationToken)
    {
        var gpdataquery = await _gpDataQueryService.GetGPDataQuery(age, yearsSinceLastReading,cancellationToken);
    
    
        return View(gpdataquery);
    }
}