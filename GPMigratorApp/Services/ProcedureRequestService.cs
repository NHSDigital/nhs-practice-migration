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

public class ProcedureRequestService: IProcedureRequestService
{
    private readonly ICodingService _codingService;
    
    public ProcedureRequestService(ICodingService codingService)
    {
        _codingService = codingService;
    }
    
    public async Task PutProcedureRequests(IEnumerable<ProcedureRequestDTO> procedureRequests,IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
    { 
        var procedureRequestCommand = new ProcedureRequestCommand(connection);
        // Request intent of Plan is a diary entry, Order is a procedure request
        foreach (var procedure in procedureRequests.Where(x=> x.Intent == (int)RequestIntent.Plan).OrderBy(x=> x.AuthoredOn))
        {
            if (procedure.Code is not null)
                procedure.Code.Id = await _codingService.PutCoding(procedure.Code, connection, transaction, cancellationToken);

            if (procedure.Intent == (int) RequestIntent.Plan)
            {
                var existingRecord = await procedureRequestCommand.GetDiaryEntryAsync(procedure.OriginalId, cancellationToken, transaction);
                if (existingRecord is null)
                {
                    await procedureRequestCommand.InsertDiaryEntryAsync(procedure, cancellationToken, transaction);
                }
                else
                {
                }
            }
            else if (procedure.Intent == (int) RequestIntent.Order)
            {
                var existingRecord = await procedureRequestCommand.GetProcedureRequestAsync(procedure.OriginalId, cancellationToken, transaction);
                if (existingRecord is null)
                {
                    await procedureRequestCommand.InsertProcedureRequestAsync(procedure, cancellationToken, transaction);
                }
                else
                {
                }
            }
            

        }
    }
}