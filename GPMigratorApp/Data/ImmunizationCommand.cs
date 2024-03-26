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
  						 [{nameof(ConditionDTO.Id)}]							  	= condition.Id
						,[{nameof(ConditionDTO.OriginalId)}]					  	= condition.OriginalId
						,[{nameof(ConditionDTO.ProblemSignificance)}]			  	= condition.ProblemSignificance
						,[{nameof(ConditionDTO.Episode)}]						  	= condition.Episode
  						,[{nameof(ConditionDTO.ClinicalStatus)}]				  	= condition.ClinicalStatus
  						,[{nameof(ConditionDTO.VerificationStatus)}]			  	= condition.VerificationStatus
  						,[{nameof(ConditionDTO.Severity)}]					    = condition.Severity
						,[{nameof(ConditionDTO.BodySite)}]					    = condition.BodySite
						,[{nameof(ConditionDTO.Context)}]					      	= condition.Context
						,[{nameof(ConditionDTO.OnsetDate)}]					    = condition.OnsetDate
						,[{nameof(ConditionDTO.OnsetAge)}]					    = condition.OnsetAge
						,[{nameof(ConditionDTO.OnsetPeriodStart)}]				= condition.OnsetPeriodStart
						,[{nameof(ConditionDTO.OnsetPeriodEnd)}]				  	= condition.OnsetPeriodEnd
						,[{nameof(ConditionDTO.AbatementDate)}]				 	= condition.AbatementDate
						,[{nameof(ConditionDTO.AbatementAge)}]				 	= condition.AbatementAge
						,[{nameof(ConditionDTO.AbatementPeriodStart)}]			= condition.AbatementPeriodStart
						,[{nameof(ConditionDTO.AbatementPeriodEnd)}]			  	= condition.AbatementPeriodEnd
						,[{nameof(ConditionDTO.Abatement)}]			  			= condition.Abatement
						,[{nameof(ConditionDTO.AssertedDate)}]			  		= condition.AssertedDate
						,[{nameof(ConditionDTO.NoteText)}]			  		 	= condition.NoteText
						,[{nameof(ConditionDTO.NoteAuthored)}]			  		= condition.NoteAuthored
						,[{nameof(ConditionDTO.EntityId)}]			  		    = condition.EntityId
        			    ,[{nameof(CodeDTO.Id)}]                  					= coding.Id
          			    ,[{nameof(CodeDTO.ReadCode)}]                  				= coding.ReadCode
          			    ,[{nameof(CodeDTO.SnomedCode)}]                  			= coding.SnomedCode
          			    ,[{nameof(CodeDTO.Description)}]                  			= coding.Description
				  	    ,[{nameof(PatientDTO.Id)}]									= patient.Id
                      	,[{nameof(PatientDTO.OriginalId)}]                  		= patient.OriginalId
                      	,[{nameof(PatientDTO.Gender)}]                    			= patient.Gender
                      	,[{nameof(PatientDTO.Title)}]                        		= patient.Title
                      	,[{nameof(PatientDTO.GivenName)}]                   		= patient.GivenName
					  	,[{nameof(PatientDTO.MiddleNames)}]                  		= patient.MiddleNames
                      	,[{nameof(PatientDTO.Surname)}]								= patient.Surname
  					  	,[{nameof(PatientDTO.DateOfBirthUTC)}]               		= patient.DateOfBirthUtc
                      	,[{nameof(PatientDTO.DateOfDeathUTC)}]                		= patient.DateOfDeathUTC
                      	,[{nameof(PatientDTO.DateOfRegistrationUTC)}]         		= patient.DateOfRegistrationUTC
                      	,[{nameof(PatientDTO.NhsNumber)}]             				= patient.NhsNumber
                      	,[{nameof(PatientDTO.PatientTypeDescription)}]        		= patient.PatientTypeDescription
                      	,[{nameof(PatientDTO.DummyType)}]                    		= patient.DummyType
					  	,[{nameof(PatientDTO.ResidentialInstituteCode)}]      		= patient.ResidentialInstituteCode
                      	,[{nameof(PatientDTO.NHSNumberStatus)}]						= patient.NHSNumberStatus
                      	,[{nameof(PatientDTO.CarerName)}]							= patient.CarerName
  					  	,[{nameof(PatientDTO.DateOfBirthUTC)}]                		= patient.DateOfBirthUtc
                      	,[{nameof(PatientDTO.OriginalId)}]                  		= patient.OriginalId
                      	,[{nameof(PatientDTO.CarerRelation)}]                 		= patient.CarerRelation
                      	,[{nameof(PatientDTO.PersonGuid)}]             				= patient.PersonGuid
                      	,[{nameof(PatientDTO.DateOfDeactivation)}]                	= patient.DateOfDeactivation
                      	,[{nameof(PatientDTO.Deleted)}]                    			= patient.Deleted
					  	,[{nameof(PatientDTO.Active)}]                  			= patient.Active
						,[{nameof(PracticionerDTO.Id)}]								= practicioner.Id
                      	,[{nameof(PracticionerDTO.OriginalId)}]                  	= practicioner.OriginalId
                      	,[{nameof(PracticionerDTO.SdsUserId)}]                   	= practicioner.SdsUserId
                      	,[{nameof(PracticionerDTO.SdsRoleProfileId)}]             	= practicioner.SdsRoleProfileId
                      	,[{nameof(PracticionerDTO.Title)}]                        	= practicioner.Title
                      	,[{nameof(PracticionerDTO.GivenName)}]                    	= practicioner.GivenName
					  	,[{nameof(PracticionerDTO.MiddleNames)}]                  	= practicioner.MiddleNames
                      	,[{nameof(PracticionerDTO.Surname)}]						= practicioner.Surname
                      	,[{nameof(PracticionerDTO.Gender)}]							= practicioner.Gender
  					  	,[{nameof(PracticionerDTO.DateOfBirthUtc)}]               	= practicioner.DateOfBirthUtc  
				  FROM [dbo].[Condition] condition
				  LEFT JOIN [dbo].[Coding] coding on coding.Id = condition.Code
				  LEFT JOIN [dbo].[Patient] patient on patient.Id = condition.Subject
				  LEFT JOIN [dbo].[Encounter] encounter on encounter.Id = condition.Context
				  LEFT JOIN [dbo].[Practicioner] practicioner ON practicioner.Id = condition.Asserter
				  LEFT JOIN [dbo].[Entity] author ON author.Id = condition.NoteAuthor
				  WHERE condition.OriginalId = @OriginalId";
        
            var reader = await _connection.QueryMultipleAsync(getExisting, new
            {
                OriginalId = originalId
            }, transaction: transaction);
            var conditions = reader.Read<ConditionDTO, CodeDTO?, PatientDTO?, PracticionerDTO?, ConditionDTO>(
                (condition, code, patient, practicioner) =>
                {
	                if (code is not null)
	                    condition.Code = code;
	                
	                if (patient is not null)
		                condition.Subject = patient;
	                
	                if (practicioner is not null)
		                condition.Asserter = practicioner;
                   

                    return condition;
                }, splitOn: $"{nameof(CodeDTO.Id)},{nameof(PatientDTO.Id)},{nameof(PracticionerDTO.Id)}");

            return conditions.FirstOrDefault();
    }


    public async Task<Guid> InsertImmunizationAsync(ImmunizationDTO condition, CancellationToken cancellationToken,
	    IDbTransaction transaction)
    {
	    condition.Id = Guid.NewGuid();
	    condition.EntityId = Guid.NewGuid();

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
		    Id = condition.EntityId,
		    Type = EntityTypes.Location,
		    OriginalId = condition.OriginalId
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
		           ,@SubjectId
		           ,@EncounterId
		           ,@ActorId
		           ,@LocationId
		           ,@ParentPresent
		           ,@RecordedDate
		           ,@Date
		           ,@PrimarySource
		           ,@LotNumber
		           ,@Site
		           ,@Route
		           ,@RouteText
		           ,@NoteAuthorId
		           ,@NoteText
		           ,@NoteDate,
		           ,@ExpirationDate
		           ,@VaccinationProcedure
		           ,@VaccinationCode
		           ,@DoseQuantity
		           ,@ReactionReported
		           ,@ReactionDetailId
		           ,@ReactionDate
		           ,@DoseSequence
		           ,@Description
		           ,@AuthorityId
		           ,@Series
		           ,@SeriesDoses
		           ,@TargetDisease
		           ,@DoseStatus
		           ,@DoseStatusReason
		           ,@EntityId))";

	    var commandDefinition = new CommandDefinition(insertOrganization, new
	    {
		    Id = condition.Id,
		    OriginalId = condition.OriginalId,
		    Status = condition.ActualProblem.OriginalId,
		    Type = condition.ProblemSignificance,
		    Class = condition.Episode,
		    SubjectId = condition.ClinicalStatus,
		    EncounterId = condition.VerificationStatus,
		    ActorId = condition.Severity,
		    LocationId = condition.Code?.Id,
		    ParentPresent = condition.BodySite,
		    RecordedDate = condition.Subject?.OriginalId,
		    Date = condition.Context?.Id,
		    PrimarySource = condition.OnsetDate,
		    LotNumber = condition.OnsetAge,
		    Site = condition.OnsetPeriodStart,
		    Route = condition.OnsetPeriodEnd,
		    RouteText = condition.AbatementDate,
		    NoteAuthorId = condition.AbatementAge,
		    NoteText = condition.AbatementPeriodStart,
		    NoteDate = condition.AbatementPeriodEnd,
		    ExpirationDate = condition.Abatement,
		    VaccinationProcedure = condition.AssertedDate,
		    VaccinationCode = condition.Asserter?.OriginalId,
		    DoseQuantity = condition.NoteText,
		    ReactionReported = condition.NoteAuthored,
		    ReactionDetailId = condition.NoteAuthor?.OriginalId,
		    ReactionDate = condition.EntityId,
		    DoseSequence = condition.Abatement,
		    Description = condition.AssertedDate,
		    AuthorityId = condition.Asserter?.OriginalId,
		    Series = condition.NoteText,
		    SeriesDoses = condition.NoteAuthored,
		    TargetDisease = condition.NoteAuthor?.OriginalId,
		    DoseStatus = condition.EntityId,
		    DoseStatusReason = condition.NoteAuthor?.OriginalId,
		    EntityId = condition.EntityId

	    }, cancellationToken: cancellationToken, transaction: transaction);

	    var result = await _connection.ExecuteAsync(commandDefinition);
	    if (result == 0)
	    {
		    throw new DataException("Error: User request was not successful.");
	    }
	    
	    return condition.Id;
    }
}