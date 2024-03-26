using System.Data;
using GPMigratorApp.DTOs;
using GPMigratorApp.Models;

namespace GPMigratorApp.Services.Interfaces;

public interface IObservationService
{
    Task PutObservations(IEnumerable<ObservationDTO> observations, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken);

    Task<PaginatedData<ObservationDTO>> GetAllObservationsPaginatedAsync(Guid patientId, int offset, int limit, CancellationToken cancellationToken);

}