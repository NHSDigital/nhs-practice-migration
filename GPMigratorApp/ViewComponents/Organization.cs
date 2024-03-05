using GPMigratorApp.DTOs;
using GPMigratorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GPMigratorApp.ViewComponents
{
    public class Organization: ViewComponent
    {
        private readonly ILogger<Organization> _logger;
        
        private readonly IOrganizationService _organizationService;

        public Organization(ILogger<Organization> logger, IOrganizationService organizationService)
        {
            _logger = logger;
            _organizationService = organizationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var organization = await _organizationService.GetAllOrganizationsAsync(cancellationToken);
            return View(organization);
            
            
        }

    }


}