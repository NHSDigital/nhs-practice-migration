using GPMigratorApp.Models;
using Microsoft.AspNetCore.Mvc;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;

public class OrganizationController : Controller
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public async Task<IActionResult> Index(Guid organizationId, CancellationToken cancellationToken, int page = 1)
    {
        const int pageSize = 20;
        int offset = (page - 1) * pageSize;

        var paginatedResult = await _organizationService.GetAllOrganizationsPaginatedAsync(offset, pageSize, cancellationToken);
        var paginationModel = new PaginationViewModel
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = paginatedResult.TotalCount,
            PageHandler = "Index", 
        };

        var viewModel = new OrganizationViewModel
        {
            Organization = paginatedResult.Data,
            Pagination = paginationModel,
            OrganizationId = organizationId
        };

        return View(viewModel);
    }
    
    public async Task<IActionResult> Organization(string originalId, CancellationToken cancellationToken)
    {

        var organization  = await _organizationService.GetOrganizationAsync(originalId, cancellationToken);
    

        return View(organization);
    }
}