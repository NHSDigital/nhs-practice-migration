using GPMigratorApp.Models;
using Microsoft.AspNetCore.Mvc;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;

public class PracticionerController : Controller
{
    private readonly IPracticionerService _practicionerService;

    public PracticionerController(IPracticionerService practicionerService)
    {
        _practicionerService = practicionerService;
    }

    public async Task<IActionResult> Index(string originalId, CancellationToken cancellationToken)
    {
        var practicioner = await _practicionerService.GetSinglePracticionerAsync(originalId, cancellationToken);
    
    
        return View(practicioner);
    }
}