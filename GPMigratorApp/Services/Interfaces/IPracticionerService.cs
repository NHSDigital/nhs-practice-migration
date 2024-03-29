using System.Data;
using GPMigratorApp.DTOs;

namespace GPMigratorApp.Services.Interfaces;

public interface IPracticionerService
{
    Task PutPracticioners(IEnumerable<PracticionerDTO> practicioners, IEnumerable<PracticionerRoleDTO> practicionerRoles, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);
    
    Task  <IEnumerable<PracticionerRoleDTO?>>GetAllPractitionersAsync(CancellationToken cancellationToken);

    Task<PracticionerDTO?> GetSinglePracticionerAsync(string originalId, CancellationToken cancellationToken);
}