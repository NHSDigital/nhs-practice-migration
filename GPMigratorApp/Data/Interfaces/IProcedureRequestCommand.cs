using System.Data;
using GPMigratorApp.Data.IntermediaryModels;
using GPMigratorApp.DTOs;
using Microsoft.Data.SqlClient;

namespace GPMigratorApp.Data.Interfaces;

public interface IProcedureRequestCommand
{
    Task<ProcedureRequestDTO?> GetDiaryEntryAsync(string originalId, CancellationToken cancellationToken,
        IDbTransaction transaction);
    
    Task<ProcedureRequestDTO?> GetProcedureRequestAsync(string originalId, CancellationToken cancellationToken,
        IDbTransaction transaction);

    Task<Guid> InsertDiaryEntryAsync(ProcedureRequestDTO diaryEntry, CancellationToken cancellationToken,
        IDbTransaction transaction);

    Task<Guid> InsertProcedureRequestAsync(ProcedureRequestDTO procedureRequest, CancellationToken cancellationToken,
        IDbTransaction transaction);
    Task UpdatePracticionerAsync(PracticionerDTO practicioner, CancellationToken cancellationToken,
        IDbTransaction transaction);
    
}