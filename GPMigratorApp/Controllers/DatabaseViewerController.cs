using GPMigratorApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using FutureNHS.Api.Configuration;
using GPMigratorApp.Data;
using GPMigratorApp.GPConnect;
using GPMigratorApp.Services;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GPMigratorApp.Controllers
{
    public class DatabaseViewerController : Controller
    {
        private readonly ILogger<DatabaseViewerController> _logger;
        private readonly IGPConnectService _gpConnectService;
        private readonly AppSettings _appSettings;
        private readonly IStoreRecordService _storeRecordService;
        private readonly IPatientService _patientService;

        
        public DatabaseViewerController(ILogger<DatabaseViewerController> logger, IPatientService patientService)
        {
            _logger = logger;
            _patientService = patientService;
        }


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var patientrecords = await _patientService.GetAllPatientsAsync(cancellationToken);
            
            
            return View(patientrecords);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}