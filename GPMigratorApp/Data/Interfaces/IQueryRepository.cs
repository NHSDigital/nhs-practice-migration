using System.Data;
using System.Numerics;
using GPMigratorApp.Data.IntermediaryModels;
using GPMigratorApp.DTOs;
using GPMigratorApp.QueryDTOs;
using Microsoft.Data.SqlClient;

namespace GPMigratorApp.Data.Interfaces;

public interface IQueryRepository
{
    Task<IEnumerable<BloodPressureQuery>> RunBloodPressureQuery(int age, int yearsSinceLastReading,
        CancellationToken cancellationToken);
}