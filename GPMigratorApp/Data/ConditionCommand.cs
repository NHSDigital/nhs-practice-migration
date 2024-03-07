using System.Data;
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


    /*public async Task<ConditionDTO?> GetConditionAsync(string originalId, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    string getExisting =
		    @$"SELECT [{nameof(ConditionDTO.Id)}]								= loc.Id
                      ,[{nameof(ConditionDTO.OriginalId)}]                  	    = loc.OriginalId
                      ,[{nameof(ConditionDTO.ODSSiteCode)}]                  	= loc.ODSSiteCodeID
                      ,[{nameof(ConditionDTO.Status)}]                  			= loc.Status
                      ,[{nameof(ConditionDTO.OperationalStatus)}]                = loc.OperationalStatus
                      ,[{nameof(ConditionDTO.Name)}]                         	= loc.Name
                      ,[{nameof(ConditionDTO.Alias)}]                         	= loc.Alias
                      ,[{nameof(ConditionDTO.Description)}]                      = loc.Description
					  ,[{nameof(ConditionDTO.Type)}]                     		= loc.Type
                      ,[{nameof(ConditionDTO.Telecom)}]							= loc.Telecom
                      ,[{nameof(ConditionDTO.PhysicalType)}]						= loc.PhysicalType
  					  ,[{nameof(ConditionDTO.EntityId)}]                     	= loc.EntityId
                      ,[{nameof(AddressDTO.Id)}]								= address.Id
					  ,[{nameof(AddressDTO.Use)}]								= address.[Use]
					  ,[{nameof(AddressDTO.HouseNameFlatNumber)}]				= address.HouseNameFlatNumber
					  ,[{nameof(AddressDTO.NumberAndStreet)}]					= address.NumberAndStreet
					  ,[{nameof(AddressDTO.Village)}]							= address.Village
					  ,[{nameof(AddressDTO.Town )}]								= address.Town
					  ,[{nameof(AddressDTO.County )}]							= address.County
					  ,[{nameof(AddressDTO.Postcode)}]							= address.Postcode
					  ,[{nameof(AddressDTO.From)}]								= address.[From]
					  ,[{nameof(AddressDTO.To )}]								= address.[To]
					  ,[{nameof(OrganizationDTO.Id)}]							= org.Id
                      ,[{nameof(OrganizationDTO.ODSCode)}]                      = org.ODSCode
                      ,[{nameof(OrganizationDTO.PeriodStart)}]                  = org.PeriodStart
                      ,[{nameof(OrganizationDTO.PeriodEnd)}]                    = org.PeriodEnd
                      ,[{nameof(OrganizationDTO.Type)}]                         = org.Type
                      ,[{nameof(OrganizationDTO.Name)}]                         = org.Name
                      ,[{nameof(OrganizationDTO.Telecom)}]                      = org.Telecom
					  ,[{nameof(OrganizationDTO.EntityId)}]                     = org.EntityId
                      

                  FROM [dbo].[Location] loc
				  LEFT JOIN [dbo].[Organization] org ON loc.ManagingOrganizationID = org.Id
				  LEFT JOIN [dbo].[Address] address ON address.Id = loc.AddressID
				  LEFT JOIN [dbo].[Location] partof ON partof.Id = loc.PartOfID
				  WHERE loc.OriginalId = @OriginalId";
        
            var reader = await _connection.QueryMultipleAsync(getExisting, new
            {
                OriginalId = originalId
            }, transaction: transaction);
            var locations = reader.Read<LocationDTO, AddressDTO, OrganizationDTO,LocationDTO>(
                (location, address,organization) =>
                {
	                

	                if (address is not null)
                    {
	                    location.Address = address;
                    }

	                if (organization is not null)
	                    location.ManagingOrganization = organization;
                    
                   

                    return location;
                }, splitOn: $"{nameof(AddressDTO.Id)},{nameof(OrganizationDTO.Id)}");

            return locations.FirstOrDefault();
    }
    */

    public async Task<Guid> InsertConditionAsync(ConditionDTO condition, CancellationToken cancellationToken, IDbTransaction transaction)
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
		           ,[NoteAuthor])
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
		           ,(select Id from dbo.Entity where OriginalId = @NoteAuthor))";
            
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
                SubjectId = condition.Subject.OriginalId,
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
                Asserter = condition.Asserter,
                NoteText = condition.NoteText,
                NoteAuthored = condition.NoteAuthored,
                NoteAuthor = condition.NoteAuthor?.OriginalId
                
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
	            ,@Code";
            
	            
            foreach (var evidence in condition.Evidence)
            {
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
            
            const string insertRelatedProblem =
	            @"INSERT INTO [dbo].[Condition_RelatedProblems]
            	([ConditionId]
	            ,[EntityId])
            	VALUES
	            (@Id
	            ,(select Id from dbo.Entity where OriginalId = @EntityId)";
            
	            
            foreach (var related in condition.RelatedProblem)
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
            
            const string insertRelatedClinical =
	            @"INSERT INTO [dbo].[Condition_RelatedClinicalConditions]
            	([ConditionId]
	            ,[EntityId])
            	VALUES
	            (@Id
	            ,(select Id from dbo.Entity where OriginalId = @EntityId)";
            
	            
            foreach (var related in condition.RelatedClinicalConditions)
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
            

            return condition.Id;
    }

    public async Task UpdateLocationAsync(LocationDTO location, CancellationToken cancellationToken, IDbTransaction transaction)
    {
         const string updateOrganization =
                @"UPDATE [dbo].[Location]
				  SET
                       [ODSSiteCodeID] = @ODSCode,
                       [Status] = @Status,
                       [OperationalStatus] = @OperationalStatus,
                       [Name] = @Name,
                       [Alias] = @Alias,
                       [Description] = @Description,
                       [Type] = @Type,
                       [Telecom] = @Telecom,
                       [AddressID] = @Address,
                       [PhysicalType] = @PhysicalType,
				       [ManagingOrganizationID] = @ManagingOrg,
                       [PartOfID] = @PartOf
                 WHERE Id = @Id";
            
            var commandDefinition = new CommandDefinition(updateOrganization, new
            {
	            Id = location.Id,
	            ODSCode = location.ODSSiteCode,
	            Status = location.Status,
	            OperationalStatus = location.OperationalStatus,
	            Name = location.Name,
	            Alias = location.Alias,
	            Description = location.Description,
	            Type = location.Type,
	            Telecom = location.Telecom,
	            Address = location.Address?.Id,
	            PhysicalType = location.PhysicalType,
	            ManagingOrg = location.ManagingOrganization?.Id,
	            PartOf = location.PartOf?.Id,
                
            }, cancellationToken: cancellationToken, transaction: transaction);
            
            var result = await _connection.ExecuteAsync(commandDefinition);
            if (result == 0)
            {
                throw new DataException("Error: User request was not successful.");
            }
    }

}