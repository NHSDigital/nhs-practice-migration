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

public class ConditionCommand : IConditionCommand
{
    private readonly IDbConnection _connection;
    
    public ConditionCommand(IDbConnection connection)
    {
        _connection = connection;
    }


    public async Task<ConditionDTO?> GetConditionAsync(string originalId, CancellationToken cancellationToken, IDbTransaction transaction)
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


    public async Task<Guid> InsertConditionAsync(ConditionDTO condition, CancellationToken cancellationToken,
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
		    @"INSERT INTO [dbo].[Condition]
		           ([Id]
		           ,[OriginalId]
		           ,[ActualProblem]
		           ,[ProblemSignificance]
		           ,[Episode]
		           ,[ClinicalStatus]
		           ,[VerificationStatus]
		           ,[Severity]
		           ,[Code]
		           ,[BodySite]
		           ,[Subject]
		           ,[Context]
		           ,[OnsetDate]
		           ,[OnsetAge]
		           ,[OnsetPeriodStart]
		           ,[OnsetPeriodEnd]
		           ,[AbatementDate]
		           ,[AbatementAge]
		           ,[AbatementPeriodStart]
		           ,[AbatementPeriodEnd]
		           ,[Abatement]
		           ,[AssertedDate]
		           ,[Asserter]
		           ,[NoteText]
		           ,[NoteAuthored]
		           ,[NoteAuthor]
		           ,[EntityId])
		     VALUES
		           (@Id
		       	   ,@OriginalId
		           ,(select Id from dbo.Entity where OriginalId = @ActualProblem)
		           ,@ProblemSignificance
		           ,@Episode
		           ,@ClinicalStatus
		           ,@VerificationStatus
		           ,@Severity
		           ,@Code
		           ,@BodySite
		           ,(select Id from dbo.Patient where OriginalId = @SubjectId)
		           ,(select Id from dbo.Encounter where OriginalId = @Context)
		           ,@OnsetDate
		           ,@OnsetAge
		           ,@OnsetPeriodStart
		           ,@OnsetPeriodEnd
		           ,@AbatementDate
		           ,@AbatementAge
		           ,@AbatementPeriodStart
		           ,@AbatementPeriodEnd
		           ,@Abatement
		           ,@AssertedDate
		           ,(select Id from dbo.Practicioner where OriginalId = @Asserter)
		           ,@NoteText
		           ,@NoteAuthored
		           ,(select Id from dbo.Entity where OriginalId = @NoteAuthor)
		           ,@EntityId)";

	    var commandDefinition = new CommandDefinition(insertOrganization, new
	    {
		    Id = condition.Id,
		    OriginalId = condition.OriginalId,
		    ActualProblem = condition.ActualProblem.OriginalId,
		    ProblemSignificance = condition.ProblemSignificance,
		    Episode = condition.Episode,
		    ClinicalStatus = condition.ClinicalStatus,
		    VerificationStatus = condition.VerificationStatus,
		    Severity = condition.Severity,
		    Code = condition.Code?.Id,
		    BodySite = condition.BodySite,
		    SubjectId = condition.Subject?.OriginalId,
		    Context = condition.Context?.Id,
		    OnsetDate = condition.OnsetDate,
		    OnsetAge = condition.OnsetAge,
		    OnsetPeriodStart = condition.OnsetPeriodStart,
		    OnsetPeriodEnd = condition.OnsetPeriodEnd,
		    AbatementDate = condition.AbatementDate,
		    AbatementAge = condition.AbatementAge,
		    AbatementPeriodStart = condition.AbatementPeriodStart,
		    AbatementPeriodEnd = condition.AbatementPeriodEnd,
		    Abatement = condition.Abatement,
		    AssertedDate = condition.AssertedDate,
		    Asserter = condition.Asserter?.OriginalId,
		    NoteText = condition.NoteText,
		    NoteAuthored = condition.NoteAuthored,
		    NoteAuthor = condition.NoteAuthor?.OriginalId,
		    EntityId = condition.EntityId

	    }, cancellationToken: cancellationToken, transaction: transaction);

	    var result = await _connection.ExecuteAsync(commandDefinition);
	    if (result == 0)
	    {
		    throw new DataException("Error: User request was not successful.");
	    }

	    const string insertEvidence =
		    @"INSERT INTO [dbo].[Condition_Evidence]
            	([ConditionId]
	            ,[EntityId]
	            ,[Code])
            	VALUES
	            (@Id
	            ,(select Id from dbo.Entity where OriginalId = @EntityId)
	            ,@Code)";


	    if (condition.Evidence is not null)
	    {
		    foreach (var evidence in condition.Evidence)
		    {
			    try{
			    var evidenceCommandDefinition = new CommandDefinition(insertEvidence, new
			    {
				    Id = condition.Id,
				    EntityId = evidence.OriginalId,
				    Code = evidence.Code,

			    }, cancellationToken: cancellationToken, transaction: transaction);

			    result = await _connection.ExecuteAsync(evidenceCommandDefinition);
			    if (result == 0)
			    {
				    throw new DataException("Error: User request was not successful.");
			    }
			    }
			    catch (Exception ex)
			    {
		    
			    }
		    }
	    }

	    const string insertRelatedProblem =
		    @"INSERT INTO [dbo].[Condition_RelatedProblems]
            	([ConditionId]
	            ,[EntityId])
            	VALUES
	            (@Id
	            ,(select Id from dbo.Entity where OriginalId = @EntityId))";
	    
		    if (condition.RelatedProblem is not null)
		    {
			    try
			    {
			    foreach (var related in condition.RelatedProblem.Where(x => x.Type is not null))
			    {
				    var relatedCommandDefinition = new CommandDefinition(insertRelatedProblem, new
				    {
					    Id = condition.Id,
					    EntityId = related.OriginalId

				    }, cancellationToken: cancellationToken, transaction: transaction);

				    result = await _connection.ExecuteAsync(relatedCommandDefinition);
				    if (result == 0)
				    {
					    throw new DataException("Error: User request was not successful.");
				    }
			    }
		    }
		    catch (Exception ex)
		    {
		    
		    }
		    }

	    const string insertRelatedClinical =
		    @"INSERT INTO [dbo].[Condition_RelatedClinicalConditions]
            	([ConditionId]
	            ,[EntityId])
            	VALUES
	            (@Id
	            ,(select Id from dbo.Entity where OriginalId = @EntityId))";

	    try
	    {
		    if (condition.RelatedClinicalConditions is not null)
		    {
			    foreach (var related in condition.RelatedClinicalConditions.Where(x => x.Type is not null))
			    {
				    var relatedCommandDefinition = new CommandDefinition(insertRelatedClinical, new
				    {
					    Id = condition.Id,
					    EntityId = related.OriginalId

				    }, cancellationToken: cancellationToken, transaction: transaction);

				    result = await _connection.ExecuteAsync(relatedCommandDefinition);
				    if (result == 0)
				    {
					    throw new DataException("Error: User request was not successful.");
				    }
			    }
		    }
	    }
	    catch (Exception ex)
	    {
		    
	    }

	    return condition.Id;
    }
}