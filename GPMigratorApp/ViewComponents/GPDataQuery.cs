using GPMigratorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMigratorApp.ViewComponents
{
    public class GPDataQuery: ViewComponent
    {
        private readonly ILogger<GPDataQuery> _logger;
        
        private readonly IGPDataQueryService _gpDataQueryService;

        public GPDataQuery(ILogger<GPDataQuery> logger, IGPDataQueryService gpDataQueryService)
        {
            _logger = logger;
            _gpDataQueryService = gpDataQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int age, int yearsSinceLastReading, CancellationToken cancellationToken)
        {
            var gpDataQuery= await _gpDataQueryService.GetGPDataQuery(age, yearsSinceLastReading, cancellationToken);
     
            return View(gpDataQuery);
            
            
        }

    }


}