using System.Data;
using GPMigratorApp.Data;
using GPMigratorApp.Data.Database.Providers.Interfaces;
using GPMigratorApp.DTOs;
using GPMigratorApp.QueryDTOs;
using GPMigratorApp.Services.Interfaces;

namespace GPMigratorApp.Services;

public class GPDataQueryService : IGPDataQueryService
{
    private readonly IAzureSqlDbConnectionFactory _connectionFactory;

    public GPDataQueryService(IAzureSqlDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<BloodPressureQuery>> GetGPDataQuery(int age, int yearsSinceLastReading, CancellationToken cancellationToken)
    {
        var gpDataQueryCommand = new GPDataQueryCommand(_connectionFactory);

        var existingRecord = await gpDataQueryCommand.GetGPDataQuery(age, yearsSinceLastReading, cancellationToken);

        return existingRecord;
    }
}