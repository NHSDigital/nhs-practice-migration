using System.Data;
using GPMigratorApp.DTOs;
using GPMigratorApp.Models;

namespace GPMigratorApp.Services.Interfaces;

public interface IOrganizationService
{
    Task PutOrganizations(IEnumerable<OrganizationDTO> organizations, IDbConnection connection,
        IDbTransaction transaction, CancellationToken cancellationToken);

    Task<IEnumerable<OrganizationDTO>> GetAllOrganizationsAsync(CancellationToken cancellationToken);

    Task<PaginatedData<OrganizationDTO>> GetAllOrganizationsPaginatedAsync(int offset, int limit,
        CancellationToken cancellationToken);

    Task<OrganizationDTO> GetOrganizationAsync(string originalId, CancellationToken cancellationToken);

}