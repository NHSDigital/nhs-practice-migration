using System.Data;
using GPConnect.Provider.AcceptanceTests.Http;
using GPMigratorApp.Data;
using GPMigratorApp.Data.Database.Providers.Interfaces;
using GPMigratorApp.Data.Interfaces;
using GPMigratorApp.DTOs;
using GPMigratorApp.Services.Interfaces;
using Hl7.Fhir.Model;
using Microsoft.IdentityModel.Tokens;
using Task = System.Threading.Tasks.Task;

namespace GPMigratorApp.Services;

public class ConditionService: IConditionService
{
    private readonly ICodingService _codingService;
    public ConditionService(ICodingService codingService)
    {
        _codingService = codingService;
    }
    
    public async Task PutConditions(IEnumerable<ConditionDTO> conditions,IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
    { 
        var locationCommand = new ConditionCommand(connection);
        foreach (var condition in conditions)
        {
            if (condition.Code is not null)
                condition.Code.Id = await _codingService.PutCoding(condition.Code, connection, transaction, cancellationToken);
            
            await locationCommand.InsertConditionAsync(condition, cancellationToken,transaction);
            
            /*var existingRecord = await locationCommand.GetLocationAsync(location.OriginalId, cancellationToken, transaction);
            if (existingRecord is null)
            {
               
            }
            else
            {
                if (location.Address != null && existingRecord.Address != null)
                {
                    location.Address.Id = existingRecord.Address.Id;
                }

                existingRecord.ODSSiteCode = location.ODSSiteCode;
                existingRecord.Status = location.Status;
                existingRecord.OperationalStatus = location.OperationalStatus;
                existingRecord.Name = location.Name;
                existingRecord.Alias = location.Alias;
                existingRecord.Description = location.Description;
                existingRecord.Type = location.Type;
                existingRecord.Telecom = location.Telecom;
                existingRecord.PhysicalType = location.PhysicalType;

                await locationCommand.UpdateLocationAsync(existingRecord, cancellationToken, transaction);
            }*/
        }
    }
}