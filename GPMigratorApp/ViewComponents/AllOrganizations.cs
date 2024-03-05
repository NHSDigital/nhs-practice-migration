using GPMigratorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMigratorApp.ViewComponents
{
    public class AllOrganizations: ViewComponent
    {
        private readonly ILogger<AllOrganizations> _logger;
        
        private readonly IOrganizationService _organizationService;

        public AllOrganizations(ILogger<AllOrganizations> logger, IOrganizationService organizationService)
        {
            _logger = logger;
            _organizationService = organizationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var allOrganizations = await _organizationService.GetAllOrganizationsAsync(cancellationToken);
            return View(allOrganizations);
            
            
        }

    }


}