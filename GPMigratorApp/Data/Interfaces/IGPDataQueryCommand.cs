using System.Data;
using System.Numerics;
using GPMigratorApp.Data.IntermediaryModels;
using GPMigratorApp.DTOs;
using GPMigratorApp.QueryDTOs;
using Microsoft.Data.SqlClient;

namespace GPMigratorApp.Data.Interfaces;

public interface IGPDataQueryCommand
{
    Task<IEnumerable<BloodPressureQuery>> GetGPDataQuery(int age, int yearsSinceLastReading,
        CancellationToken cancellationToken);

}