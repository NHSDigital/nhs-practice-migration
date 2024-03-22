using System.Data;
using GPMigratorApp.Data.IntermediaryModels;
using GPMigratorApp.DTOs;
using Microsoft.Data.SqlClient;

namespace GPMigratorApp.Data.Interfaces;

public interface IConditionCommand
{
    Task<ConditionDTO?> GetConditionAsync(string originalId, CancellationToken cancellationToken,
        IDbTransaction transaction);
    Task<Guid> InsertConditionAsync(ConditionDTO condition, CancellationToken cancellationToken,
        IDbTransaction transaction);
    
}