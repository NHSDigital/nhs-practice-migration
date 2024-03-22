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
        
        foreach (var condition in conditions.OrderByDescending(x=> x.AssertedDate.Value))
        {
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