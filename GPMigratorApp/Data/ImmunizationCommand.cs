using System.Data;
using System.Linq.Expressions;
using Dapper;
using GPMigratorApp.Data.Database.Providers.Interfaces;
using GPMigratorApp.Data.Interfaces;
using GPMigratorApp.Data.IntermediaryModels;
using GPMigratorApp.Data.Types;
using GPMigratorApp.DTOs;
using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using Microsoft.Data.SqlClient;
using Task = System.Threading.Tasks.Task;

namespace GPMigratorApp.Data;

public class ImmunizationCommand : IImmunizationCommand
{
    private readonly IDbConnection _connection;
    
    public ImmunizationCommand(IDbConnection connection)
    {
        _connection = connection;
    }


    public async Task<ImmunizationDTO?> GetImmunizationAsync(string originalId, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    string getExisting =
		    @$"SELECT 
  						 [{nameof(ImmunizationDTO.Id)}]							  	= immunization.Id
						 
				  FROM [dbo].[Immunization] immunization
				  WHERE immunization.OriginalId = @OriginalId";
        
            var reader = await _connection.QueryMultipleAsync(getExisting, new
            {
                OriginalId = originalId
            }, transaction: transaction);
            var conditions = await reader.ReadAsync<ImmunizationDTO>();

            return conditions.FirstOrDefault();
    }


    public async Task<Guid> InsertImmunizationAsync(ImmunizationDTO immunization, CancellationToken cancellationToken,
	    IDbTransaction transaction)
    {
	    immunization.Id = Guid.NewGuid();
	    immunization.EntityId = Guid.NewGuid();

	    var entity = @"INSERT INTO [dbo].[Entity]
                        ([Id],
                        [OriginalId],
                        [EntityType])
                       VALUES
                        (@Id,
                        @OriginalId,
                        @Type)";

	    var entityDefinition = new CommandDefinition(entity, new
	    {
		    Id = immunization.EntityId,
		    Type = EntityTypes.Immunization,
		    OriginalId = immunization.OriginalId
	    }, cancellationToken: cancellationToken, transaction: transaction);

	    await _connection.ExecuteAsync(entityDefinition);

	    const string insertOrganization =
		    @"INSERT INTO [dbo].[Immunization]
		           ([Id]
		           ,[OriginalId]
		           ,[Status]
		           ,[Type]
		           ,[Class]
		           ,[SubjectId]
		           ,[EncounterId]
		           ,[ActorId]
		           ,[LocationId]
		           ,[ParentPresent]
		           ,[RecordedDate]
		           ,[Date]
		           ,[PrimarySource]
		           ,[LotNumber]
		           ,[Site]
		           ,[Route]
		           ,[RouteText]
		           ,[NoteAuthorId]
		           ,[NoteText]
		           ,[NoteDate]
		           ,[ExpirationDate]
		           ,[VaccinationProcedure]
		           ,[VaccinationCode]
		           ,[DoseQuantity]
		           ,[ReactionReported]
		           ,[ReactionDetailId]
		           ,[ReactionDate]
		           ,[DoseSequence]
		           ,[Description]
		           ,[AuthorityId]
		           ,[Series]
		           ,[SeriesDoses]
		           ,[TargetDisease]
		           ,[DoseStatus]
		           ,[DoseStatusReason]
		           ,[EntityId])
		     VALUES
		           (@Id
		           ,@OriginalId
		           ,@Status
		           ,@Type
		           ,@Class
		           ,(SELECT Id from Patient WHERE OriginalId = @SubjectId)
		           ,(SELECT Id from Encounter WHERE OriginalId = @EncounterId)
		           ,(SELECT Id from Practicioner WHERE OriginalId = @ActorId)
		           ,(SELECT Id from Location WHERE OriginalId = @LocationId)
		           ,@ParentPresent
		           ,@RecordedDate
		           ,@Date
		           ,@PrimarySource
		           ,@LotNumber
		           ,@Site
		           ,@Route
		           ,@RouteText
		           ,(SELECT Id from Practicioner WHERE OriginalId = @NoteAuthorId)
		           ,@NoteText
		           ,@NoteDate
		           ,@ExpirationDate
		           ,@VaccinationProcedure
		           ,@VaccinationCode
		           ,@DoseQuantity
		           ,@ReactionReported
		           ,(SELECT Id from Observation WHERE OriginalId = @ReactionDetailId)
		           ,@ReactionDate
		           ,@DoseSequence
		           ,@Description
		           ,(SELECT Id from Organization WHERE OriginalId = @AuthorityId)
		           ,@Series
		           ,@SeriesDoses
		           ,@TargetDisease
		           ,@DoseStatus
		           ,@DoseStatusReason
		           ,@EntityId)";

	    var commandDefinition = new CommandDefinition(insertOrganization, new
	    {
		    Id = immunization.Id,
		    OriginalId = immunization.OriginalId,
		    Status = immunization.Status,
		    Type = immunization.Type,
		    Class = immunization.Class,
		    SubjectId = immunization.Subject?.OriginalId,
		    EncounterId = immunization.Encounter?.OriginalId,
		    ActorId = immunization.Actor?.OriginalId,
		    LocationId = immunization.Location?.OriginalId,
		    ParentPresent = immunization.ParentPresent,
		    RecordedDate = immunization.RecordedDate,
		    Date = immunization.Date,
		    PrimarySource = immunization.PrimarySource,
		    LotNumber = immunization.LotNumber,
		    Site = immunization.Site?.Id,
		    Route = immunization.Route?.Id,
		    RouteText = immunization.RouteText,
		    NoteAuthorId = immunization.NoteAuthor?.OriginalId,
		    NoteText = immunization.NoteText,
		    NoteDate = immunization.NoteDate,
		    ExpirationDate = immunization.ExpirationDate,
		    VaccinationProcedure = immunization.VaccinationProcedure?.Id,
		    VaccinationCode = immunization.VaccinationCode?.Id,
		    DoseQuantity = immunization.DoseQuantity,
		    ReactionReported = immunization.ReactionReported,
		    ReactionDetailId = immunization.ReactionDetail?.OriginalId,
		    ReactionDate = immunization.ReactionDate,
		    DoseSequence = immunization.DoseSequence,
		    Description = immunization.Description,
		    AuthorityId = immunization.Authority?.OriginalId,
		    Series = immunization.Series,
		    SeriesDoses = immunization.SeriesDoses,
		    TargetDisease = immunization.TargetDisease,
		    DoseStatus = immunization.DoseStatus,
		    DoseStatusReason = immunization.DoseStatusReason,
		    EntityId = immunization.EntityId

	    }, cancellationToken: cancellationToken, transaction: transaction);

	    var result = await _connection.ExecuteAsync(commandDefinition);
	    if (result == 0)
	    {
		    throw new DataException("Error: User request was not successful.");
	    }
	    
	    return immunization.Id;
    }
}