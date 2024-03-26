using GPMigratorApp.Models;
using Microsoft.AspNetCore.Mvc;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;

public class ObservationController : Controller
{
    private readonly IObservationService _observationService;
    private readonly IPatientService _patientService;

    public ObservationController(IObservationService observationService, IPatientService patientService)
    {
        _observationService = observationService;
        _patientService = _patientService;
    }

    public async Task<IActionResult> Index(Guid patientId, CancellationToken cancellationToken, int page = 1)
{
    const int pageSize = 20;
    int offset = (page - 1) * pageSize;

    var paginatedResult = await _observationService.GetAllObservationsPaginatedAsync(patientId, offset, pageSize, cancellationToken);
    var paginationModel = new PaginationViewModel
    {
        CurrentPage = page,
        PageSize = pageSize,
        TotalItems = paginatedResult.TotalCount,
        PageHandler = "Index", 
    };

    var viewModel = new ObservationViewModel
    {
        Observations = paginatedResult.Data,
        Pagination = paginationModel,
        PatientId = patientId
    };

    return View(viewModel);
}
}