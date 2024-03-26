using System.Data;
using GPMigratorApp.Data;
using GPMigratorApp.Data.Interfaces;
using GPMigratorApp.DTOs;
using GPMigratorApp.Services.Interfaces;
using GPMigratorApp.Data.Database.Providers.Interfaces;
using System.Linq;
using GPMigratorApp.Models;

namespace GPMigratorApp.Services;

public class ObservationService: IObservationService
{
    private readonly ICodingService _codingService;
    private readonly IAzureSqlDbConnectionFactory _connectionFactory;
    public ObservationService(ICodingService codingService, IAzureSqlDbConnectionFactory connectionFactory)
    {
        _codingService = codingService;
        _connectionFactory = connectionFactory;
    }
    
    public async Task<PaginatedData<ObservationDTO>>  GetAllObservationsPaginatedAsync(Guid patientId, int offset, int limit, CancellationToken cancellationToken)
    {
        var connection = await _connectionFactory.GetReadOnlyConnectionAsync(cancellationToken);
        var observationCommand = new ObservationCommand(connection);
        var observation = await observationCommand.GetObservationsPaginatedAsync(patientId, offset, limit, cancellationToken);

        return observation;
    }
    
    public async Task PutObservations(IEnumerable<ObservationDTO> observations, IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken)
    {
        var observationCommand = new ObservationCommand(connection);
        foreach (var observation in observations)
        {
            if (observation.Code is not null)
                observation.Code.Id = await _codingService.PutCoding(observation.Code, connection, transaction, cancellationToken);

            if (observation.Components != null)
                foreach (var component in observation.Components)
                {
                    if (component.Code is not null)
                        component.Code.Id = await _codingService.PutCoding(component.Code, connection, transaction,
                            cancellationToken);
                }

            try
            {
                var existingRecord =
                    await observationCommand.GetObservationAsync(observation.OriginalId, cancellationToken,
                        transaction);
                if (existingRecord is null)
                {
                    await observationCommand.InsertObservationAsync(observation, cancellationToken, transaction);
                }
                else
                {
                    existingRecord.OriginalId = observation.OriginalId;
                    existingRecord.Status = observation.Status;
                    existingRecord.Category = observation.Category;
                    existingRecord.EffectiveDate = observation.EffectiveDate;
                    existingRecord.EffectiveDateFrom = observation.EffectiveDateFrom;
                    existingRecord.EffectiveDateTo = observation.EffectiveDateTo;
                    existingRecord.Issued = observation.Issued;
                    existingRecord.Interpretation = observation.Interpretation;
                    existingRecord.DataAbsentReason = observation.DataAbsentReason;
                    existingRecord.Comment = observation.Comment;
                    existingRecord.BodySite = observation.BodySite;
                    existingRecord.Method = observation.Method;
                    existingRecord.ReferenceRangeLow = observation.ReferenceRangeLow;
                    existingRecord.ReferenceRangeHigh = observation.ReferenceRangeHigh;
                    existingRecord.ReferenceRangeType = observation.ReferenceRangeType;
                    existingRecord.ReferenceRangeAppliesTo = observation.ReferenceRangeAppliesTo;
                    existingRecord.ReferenceRangeAgeHigh = observation.ReferenceRangeAgeHigh;
                    existingRecord.ReferenceRangeAgeLow = observation.ReferenceRangeAgeLow;

                    await observationCommand.UpdateObservationAsync(existingRecord, cancellationToken, transaction);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}