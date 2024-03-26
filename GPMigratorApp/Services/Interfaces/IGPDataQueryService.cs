using System.Data;
using GPMigratorApp.DTOs;
using GPMigratorApp.Models;
using GPMigratorApp.QueryDTOs;

namespace GPMigratorApp.Services.Interfaces;

public interface IGPDataQueryService
{
    Task<IEnumerable<BloodPressureQuery>> GetGPDataQuery(int age, int yearsSinceLastReading,CancellationToken cancellationToken);

}