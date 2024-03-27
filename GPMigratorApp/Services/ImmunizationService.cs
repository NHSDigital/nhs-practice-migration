using System.Data;
using GPConnect.Provider.AcceptanceTests.Http;
using GPMigratorApp.Data;
using GPMigratorApp.Data.Database.Providers.Interfaces;
using GPMigratorApp.Data.Interfaces;
using GPMigratorApp.Data.Types;
using GPMigratorApp.DTOs;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;
using Microsoft.IdentityModel.Tokens;
using Task = System.Threading.Tasks.Task;

namespace GPMigratorApp.Services;

public class ImmunizationService: IImmunizationService
{
    private readonly ICodingService _codingService;
    public ImmunizationService(ICodingService codingService)
    {
        _codingService = codingService;
    }
    
    public async Task PutImmunizations(IEnumerable<ImmunizationDTO> immunizations,IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
    {  
        var immunizationCommand = new ImmunizationCommand(connection);

        foreach (var immunization in immunizations)
        {
            var existing = await immunizationCommand.GetImmunizationAsync(immunization.OriginalId, cancellationToken, transaction);
            if (existing is null)
            {
                if (immunization.Site is not null)
                    immunization.Site.Id = await _codingService.PutCoding(immunization.Site, connection, transaction, cancellationToken);
                
                if (immunization.Route is not null)
                    immunization.Route.Id = await _codingService.PutCoding(immunization.Route, connection, transaction, cancellationToken);
                
                if (immunization.VaccinationProcedure is not null)
                    immunization.VaccinationProcedure.Id = await _codingService.PutCoding(immunization.VaccinationProcedure, connection, transaction, cancellationToken);
                
                if (immunization.VaccinationCode is not null)
                    immunization.VaccinationCode.Id = await _codingService.PutCoding(immunization.VaccinationCode, connection, transaction, cancellationToken);
                
                await immunizationCommand.InsertImmunizationAsync(immunization, cancellationToken, transaction);
            }

        }
    }

    private async Task PutCondition(ConditionDTO condition,IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
    {
        var locationCommand = new ConditionCommand(connection);
        
        if (condition.Code is not null)
            condition.Code.Id = await _codingService.PutCoding(condition.Code, connection, transaction, cancellationToken);
            
        var existingRecord = await locationCommand.GetConditionAsync(condition.OriginalId, cancellationToken, transaction);
        if (existingRecord is null)
        {
            await locationCommand.InsertConditionAsync(condition, cancellationToken,transaction);
        }
        else
        {
            //await locationCommand.UpdateLocationAsync(existingRecord, cancellationToken, transaction);
        }
    }
}