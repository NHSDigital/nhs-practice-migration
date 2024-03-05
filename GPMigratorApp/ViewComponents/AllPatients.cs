using GPMigratorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMigratorApp.ViewComponents
{
    public class AllPatients: ViewComponent
    {
        private readonly ILogger<AllPatients> _logger;
        
        private readonly IPatientService _patientService;

        public AllPatients(ILogger<AllPatients> logger, IPatientService patientService)
        {
            _logger = logger;
            _patientService = patientService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var allPatients = await _patientService.GetAllPatientsAsync(cancellationToken);
            return View(allPatients);
            
            
        }

    }


}