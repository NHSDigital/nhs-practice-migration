using GPMigratorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMigratorApp.ViewComponents
{
    public class Location: ViewComponent
    {
        private readonly ILogger<Location> _logger;
        
        private readonly ILocationService _locationService;

        public Location(ILogger<Location> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var location = await _locationService.GetAllLocationsAsync(cancellationToken);
            return View(location);
            
            
        }

    }


}