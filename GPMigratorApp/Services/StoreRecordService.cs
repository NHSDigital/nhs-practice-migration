using System.Data;
using GPConnect.Provider.AcceptanceTests.Http;
using GPMigratorApp.Data;
using GPMigratorApp.Data.Database.Providers;
using GPMigratorApp.Data.Database.Providers.Interfaces;
using GPMigratorApp.Data.Database.Providers.RetryPolicy;
using GPMigratorApp.Data.Interfaces;
using GPMigratorApp.DTOs;
using GPMigratorApp.Services.Interfaces;
using Microsoft.Identity.Client;

namespace GPMigratorApp.Services;

public class StoreRecordService : IStoreRecordService
{
    private readonly IAzureSqlDbConnectionFactory _connectionFactory;
    private readonly IOrganizationService _organizationService;
    private readonly ILocationService _locationService;
    private readonly IPracticionerService _practicionerService;
    private readonly IPatientService _patientService;
    private readonly IObservationService _observationService;
    private readonly IConditionService _conditionService;
    private readonly IProcedureRequestService _procedureRequestService;
    private readonly IImmunizationService _immunizationService;
    
    public StoreRecordService(IAzureSqlDbConnectionFactory connectionFactory, IOrganizationService organizationService,
        ILocationService locationService, IPracticionerService practicionerService, IPatientService patientService, IObservationService observationService, IConditionService conditionService,
        IProcedureRequestService procedureRequestService, IImmunizationService immunizationService)
    {
        _connectionFactory = connectionFactory;
        _organizationService = organizationService;
        _locationService = locationService;
        _practicionerService = practicionerService;
        _patientService = patientService;
        _observationService = observationService;
        _conditionService = conditionService;
        _procedureRequestService = procedureRequestService;
        _immunizationService = immunizationService;

    }

    public async Task StoreRecord(FhirResponse fhirResponse, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.GetReadWriteConnectionAsync(cancellationToken);
        connection.Open();
        var transaction = connection.BeginTransaction();
        try
        {
            await _organizationService.PutOrganizations(fhirResponse.Organizations, connection, transaction,
                cancellationToken);


            await _locationService.PutLocations(fhirResponse.Locations, connection, transaction,
                cancellationToken);
            
            await _practicionerService.PutPracticioners(fhirResponse.Practitioners, fhirResponse.PractitionerRoles, connection, transaction,
                cancellationToken);

            await _patientService.PutPatientsAsync(fhirResponse.Patients, connection, transaction,
                cancellationToken);
            
            await _patientService.PutPatientsAsync(fhirResponse.Patients, connection, transaction,
                cancellationToken);

            await _observationService.PutObservations(fhirResponse.Observations, connection, transaction,
                cancellationToken);

            await _conditionService.PutConditions(fhirResponse.Conditions, connection, transaction,cancellationToken);

            await _procedureRequestService.PutProcedureRequests(fhirResponse.ProcedureRequests, connection, transaction,cancellationToken);
            
            await _immunizationService.PutImmunizations(fhirResponse.Immunizations, connection, transaction,cancellationToken);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            connection.Dispose();
        }
    }
}