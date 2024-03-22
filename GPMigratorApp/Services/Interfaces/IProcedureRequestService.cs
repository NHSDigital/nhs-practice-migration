using System.Data;
using GPMigratorApp.DTOs;

namespace GPMigratorApp.Services.Interfaces;

public interface IProcedureRequestService
{
    Task PutProcedureRequests(IEnumerable<ProcedureRequestDTO> procedureRequests, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);

}