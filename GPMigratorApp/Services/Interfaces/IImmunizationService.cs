using System.Data;
using GPMigratorApp.DTOs;

namespace GPMigratorApp.Services.Interfaces;

public interface IImmunizationService
{
    Task PutImmunizations(IEnumerable<ImmunizationDTO> immunizations, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);

}