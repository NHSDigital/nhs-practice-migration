using System.Data;
using Dapper;
using GPMigratorApp.Data.Interfaces;
using GPMigratorApp.DTOs;
using GPMigratorApp.QueryDTOs;

namespace GPMigratorApp.Data;
using System.Data;
using Dapper;
using GPMigratorApp.Data.Database.Providers.Interfaces;

public class GPDataQueryCommand : IGPDataQueryCommand
{
    private readonly IAzureSqlDbConnectionFactory _connectionFactory;
    
    public GPDataQueryCommand(IAzureSqlDbConnectionFactory connection)
    {
        _connectionFactory = connection;
    }

    
    public async Task<IEnumerable<BloodPressureQuery>> GetGPDataQuery(int age, int yearsSinceLastReading,CancellationToken cancellationToken)
    {
	    string getExisting =
		    @$"IF OBJECT_ID('tempdb..##TEMPDiary') IS NOT NULL
				DROP TABLE ##TEMPDiary

				SELECT
				x.Cluster_Description,
				x.SNOMED_code_description,
				x.NhsNumber,
				(CONVERT(int,CONVERT(char(8),GETDATE(),112))-CONVERT(char(8),x.DateOfBirthUTC,112))/10000 AS Age,
				x.DateOfBirthUTC AS DateOfBirth,
				x.SnomedCode,
				x.Occurence AS LastRefusedGPQIC
				INTO ##TEMPDiary
				FROM
				(
				SELECT
				refset.Cluster_Description,
				refset.SNOMED_code_description,
				coding. SnomedCode,
				patient. NhsNumber,
				patient.DateOfBirthUTC,
				diaryEntry.Occurence,
				row_number() over (partition by patient.NhsNumber order by Occurence desc) as _rn
				FROM dbo.DiaryEntry diaryEntry
				JOIN Coding coding on coding.Id = diaryEntry.Code
				JOIN Refset refset on refset.SNOMED_code = coding.SnomedCode
				JOIN Patient patient on patient.Id= diaryEntry.Subject
				WHERE refset.PCD_Refset_ID = 999012611000230106
				) x
				where x._rn = 1


				SELECT
				x.Cluster_Description,
				x.SNOMED_code_description,
				x.PatientId,
				x.NhsNumber,
				x.Age,
				x.DateOfBirthUTC AS DateOfBirth,
				x.SnomedCode,
				x.YearsSinceLastBPReading,
				x.LastRefusedGPQIC AS LastRefusedBPReading
				FROM
				(
				SELECT
				refset.Cluster_Description,
				refset.SNOMED_code_description,
				coding.SnomedCode,
				patient.Id AS PatientId,
				patient.NhsNumber,
				(CONVERT(int,CONVERT(char(8),GETDATE(),112))-CONVERT(char(8),DateOfBirthUTC,112))/10000 AS Age,
				patient.DateOfBirthUTC,
				(CONVERT(int,CONVERT(char(8),GETDATE(),112))-CONVERT(char(8),observation.EffectiveDate,112))/10000 AS YearsSinceLastBPReading,
				tempdr.LastRefusedGPQIC,
				row_number() over (partition by patient.NhsNumber order by EffectiveDate desc) as _rn
				FROM dbo.Observation observation
				JOIN Coding coding on coding.Id = observation.Code
				JOIN Refset refset on refset.SNOMED_code = coding.SnomedCode
				JOIN Patient patient on patient.Id= observation.SubjectId
				LEFT JOIN ##TEMPDiary tempdr on tempdr.NhsNumber = patient.NhsNumber
				WHERE refset.PCD_Refset_ID = 999012731000230108
				) x
				where x._rn = 1 AND x.Age > @Age AND x.YearsSinceLastBPReading <= @YearsSinceLastReading
";
	    var connection = await _connectionFactory.GetReadWriteConnectionAsync(cancellationToken);
	    connection.Open();
	    var reader = await connection.QueryMultipleAsync(getExisting, new
	    {
		    Age = age,
		    YearsSinceLastReading = yearsSinceLastReading
	    });

	    var result = reader.Read<BloodPressureQuery>();

	    return result;
    }
}