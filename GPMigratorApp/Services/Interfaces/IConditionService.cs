using System.Data;
using GPMigratorApp.DTOs;

namespace GPMigratorApp.Services.Interfaces;

public interface IConditionService
{
    Task PutConditions(IEnumerable<ConditionDTO> conditions, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);

}