using GPMigratorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMigratorApp.ViewComponents
{
    public class AllPractitioners: ViewComponent
    {
        private readonly ILogger<AllPractitioners> _logger;
        
        private readonly IPracticionerService _practicionerService;

        public AllPractitioners(ILogger<AllPractitioners> logger, IPracticionerService practicionerService)
        {
            _logger = logger;
            _practicionerService = practicionerService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var allPractitioners= await _practicionerService.GetAllPractitionersAsync(cancellationToken);
     
            return View(allPractitioners);
            
            
        }

    }


}