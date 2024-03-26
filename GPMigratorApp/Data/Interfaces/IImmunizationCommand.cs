using System.Data;
using GPMigratorApp.Data.IntermediaryModels;
using GPMigratorApp.DTOs;
using Microsoft.Data.SqlClient;

namespace GPMigratorApp.Data.Interfaces;

public interface IImmunizationCommand
{
    Task<ImmunizationDTO?> GetImmunizationAsync(string originalId, CancellationToken cancellationToken,
        IDbTransaction transaction);
    Task<Guid> InsertImmunizationAsync(ImmunizationDTO immunization, CancellationToken cancellationToken,
        IDbTransaction transaction);
    
}