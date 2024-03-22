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

public class ProcedureRequestCommand : IProcedureRequestCommand
{
    private readonly IDbConnection _connection;
    
    public ProcedureRequestCommand(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<ProcedureRequestDTO?> GetProcedureRequestAsync(string originalId, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    string getExisting =
		    @$"SELECT [{nameof(ProcedureRequestDTO.Id)}]					  = procedureRequest.[Id]
				      ,[{nameof(ProcedureRequestDTO.OriginalId)}]			  = procedureRequest.[OriginalId]
				      ,[{nameof(ProcedureRequestDTO.Intent)}]				  = procedureRequest.[Intent]
				      ,[{nameof(ProcedureRequestDTO.Status)}]				  = procedureRequest.[Status]
				      ,[{nameof(ProcedureRequestDTO.AuthoredOn)}]			  = procedureRequest.[AuthoredOn]
				      ,[{nameof(ProcedureRequestDTO.Occurence)}]			  = procedureRequest.[Occurence]
				      ,[{nameof(ProcedureRequestDTO.Category)}]				  = procedureRequest.[Category]
				      ,[{nameof(ProcedureRequestDTO.Reason)}]			      = procedureRequest.[Reason]
				      ,[{nameof(ProcedureRequestDTO.BodySite)}]				  = procedureRequest.[BodySite]
				      ,[{nameof(ProcedureRequestDTO.NoteText)}]				  = procedureRequest.[NoteText]
				      ,[{nameof(ProcedureRequestDTO.NoteAuthored)}]			  = procedureRequest.[NoteAuthored]
				      ,[{nameof(PracticionerDTO.EntityId)}]					  = procedureRequest.[EntityId]
					  ,[{nameof(PatientDTO.Id)}]							  = patient.Id
				      ,[{nameof(PatientDTO.OriginalId)}]                  	  = patient.OriginalId
				      ,[{nameof(PatientDTO.Gender)}]                    	  = patient.Gender
				      ,[{nameof(PatientDTO.Title)}]                        	  = patient.Title
				      ,[{nameof(PatientDTO.GivenName)}]                   	  = patient.GivenName
					  ,[{nameof(PatientDTO.MiddleNames)}]                  	  = patient.MiddleNames
				      ,[{nameof(PatientDTO.Surname)}]						  = patient.Surname
  					  ,[{nameof(PatientDTO.DateOfBirthUTC)}]               	  = patient.DateOfBirthUtc
				      ,[{nameof(PatientDTO.DateOfDeathUTC)}]                  = patient.DateOfDeathUTC
				      ,[{nameof(PatientDTO.DateOfRegistrationUTC)}]           = patient.DateOfRegistrationUTC
				      ,[{nameof(PatientDTO.NhsNumber)}]             		  = patient.NhsNumber
				      ,[{nameof(PatientDTO.PatientTypeDescription)}]          = patient.PatientTypeDescription
				      ,[{nameof(PatientDTO.DummyType)}]                       = patient.DummyType
					  ,[{nameof(PatientDTO.ResidentialInstituteCode)}]        = patient.ResidentialInstituteCode
				      ,[{nameof(PatientDTO.NHSNumberStatus)}]				  = patient.NHSNumberStatus
				      ,[{nameof(PatientDTO.CarerName)}]						  = patient.CarerName
  					  ,[{nameof(PatientDTO.DateOfBirthUTC)}]                  = patient.DateOfBirthUtc
				      ,[{nameof(PatientDTO.OriginalId)}]                  	  = patient.OriginalId
				      ,[{nameof(PatientDTO.CarerRelation)}]                   = patient.CarerRelation
				      ,[{nameof(PatientDTO.PersonGuid)}]             		  = patient.PersonGuid
				      ,[{nameof(PatientDTO.DateOfDeactivation)}]              = patient.DateOfDeactivation
				      ,[{nameof(PatientDTO.Deleted)}]                    	  = patient.Deleted
					  ,[{nameof(PatientDTO.Active)}]                  		  = patient.Active
				      ,[{nameof(PatientDTO.SpineSensitive)}]				  = patient.SpineSensitive
				      ,[{nameof(PatientDTO.IsConfidential)}]				  = patient.IsConfidential
				      ,[{nameof(PatientDTO.EmailAddress)}]               	  = patient.EmailAddress
				      ,[{nameof(PatientDTO.HomePhone)}]						  = patient.HomePhone
					  ,[{nameof(PatientDTO.MobilePhone)}]					  = patient.MobilePhone
					  ,[{nameof(PatientDTO.ProcessingId)}]					  = patient.ProcessingId
					  ,[{nameof(PatientDTO.Ethnicity)}]						  = patient.Ethnicity
					  ,[{nameof(PatientDTO.Religion)}]						  = patient.Religion
					  ,[{nameof(CodeDTO.Id)}]                  				  = code.Id
				      ,[{nameof(CodeDTO.ReadCode)}]                  		  = code.ReadCode
				      ,[{nameof(CodeDTO.SnomedCode)}]                  		  = code.SnomedCode
				      ,[{nameof(CodeDTO.Description)}]                  	  = code.Description
					  ,[{nameof(OutboundRelationship.Id)}]                    = performer.Id
					  ,[{nameof(OutboundRelationship.Type)}]                  = performer.EntityType
					  ,[{nameof(OrganizationDTO.Id)}]						  = organization.Id
				      ,[{nameof(OrganizationDTO.ODSCode)}]                    = organization.ODSCode
				      ,[{nameof(OrganizationDTO.PeriodStart)}]                = organization.PeriodStart
				      ,[{nameof(OrganizationDTO.PeriodEnd)}]                  = organization.PeriodEnd
				      ,[{nameof(OrganizationDTO.Type)}]                       = organization.Type
				      ,[{nameof(OrganizationDTO.Name)}]                       = organization.Name
				      ,[{nameof(OrganizationDTO.Telecom)}]                    = organization.Telecom
					  ,[{nameof(OrganizationDTO.EntityId)}]                   = organization.EntityId
					  ,[{nameof(OutboundRelationship.Id)}]                    = context.Id
					  ,[{nameof(OutboundRelationship.Type)}]                  = context.EntityType
					  ,[{nameof(ObservationDTO.Id)}]						  = observation.Id
				      ,[{nameof(ObservationDTO.OriginalId)}]                  = observation.OriginalId
				      ,[{nameof(ObservationDTO.Status)}]                  	  = observation.Status
				      ,[{nameof(ObservationDTO.Category)}]                    = observation.Category
				      ,[{nameof(ObservationDTO.Context)}]                  	  = observation.ContextId
				      ,[{nameof(ObservationDTO.EffectiveDate)}]               = observation.EffectiveDate
				      ,[{nameof(ObservationDTO.EffectiveDateFrom)}]           = observation.EffectiveDateFrom
				      ,[{nameof(ObservationDTO.EffectiveDateTo)}]             = observation.EffectiveDateTo
				      ,[{nameof(ObservationDTO.Issued)}]                  	  = observation.Issued
				      ,[{nameof(ObservationDTO.Interpretation)}]              = observation.Interpretation
				      ,[{nameof(ObservationDTO.DataAbsentReason)}]            = observation.DataAbsentReason
				      ,[{nameof(ObservationDTO.Comment)}]                  	  = observation.Comment
				      ,[{nameof(ObservationDTO.BodySite)}]                    = observation.BodySite
				      ,[{nameof(ObservationDTO.Method)}]                  	  = observation.Method
				      ,[{nameof(ObservationDTO.ReferenceRangeLow)}]           = observation.ReferenceRangeLow
				      ,[{nameof(ObservationDTO.ReferenceRangeHigh)}]          = observation.ReferenceRangeHigh
				      ,[{nameof(ObservationDTO.ReferenceRangeType)}]          = observation.ReferenceRangeType
				      ,[{nameof(ObservationDTO.ReferenceRangeAppliesTo)}]     = observation.ReferenceRangeAppliesTo
				      ,[{nameof(ObservationDTO.ReferenceRangeAgeHigh)}]       = observation.ReferenceRangeAgeHigh
				      ,[{nameof(ObservationDTO.ReferenceRangeAgeLow)}]        = observation.ReferenceRangeAgeLow

				  FROM [dbo].[ProcedureRequest] procedureRequest
				  LEFT JOIN [dbo].[Patient] patient ON patient.Id = procedureRequest.[Subject]
				  LEFT JOIN [dbo].[Coding] code ON code.Id = procedureRequest.Code
				  LEFT JOIN [dbo].[Entity] performer ON performer.Id = procedureRequest.Performer
				  LEFT JOIN [dbo].[Organization] organization ON organization.Id = procedureRequest.OnBehalfOf
				  LEFT JOIN [dbo].[Entity] context ON context.Id = procedureRequest.Context
				  LEFT JOIN [dbo].[Observation] observation ON observation.Id = procedureRequest.SupportingInfo
				  WHERE procedureRequest.OriginalId = @OriginalId";
        
            var reader = await _connection.QueryMultipleAsync(getExisting, new
            {
                OriginalId = originalId
            }, transaction: transaction);
            var locations = reader.Read<ProcedureRequestDTO, PatientDTO?, CodeDTO?,OutboundRelationship?,OrganizationDTO?,OutboundRelationship?,ObservationDTO?,ProcedureRequestDTO>(
                (diary, patient, code, performer, onBehalfOf, context, supportingInfo) =>
                {
	                if (patient is not null)
                    {
	                    diary.Subject = patient;
                    }
	                if (code is not null)
	                {
		                diary.Code = code;
	                }
	                if (performer is not null)
	                {
		                diary.Performer = performer;
	                }
	                if (onBehalfOf is not null)
	                {
		                diary.OnBehalfOf = onBehalfOf;
	                }
	                if (context is not null)
	                {
		                diary.Context = context;
	                }
	                if (supportingInfo is not null)
	                {
		                diary.SupportingInfo = supportingInfo;
	                }
	                
	                return diary;
                }, splitOn: $"{nameof(PatientDTO.Id)},{nameof(CodeDTO.Id)},{nameof(OutboundRelationship.Id)},{nameof(OrganizationDTO.Id)},{nameof(OutboundRelationship.Id)},{nameof(ObservationDTO.Id)}");

            return locations.FirstOrDefault();
    }
    
    public async Task<ProcedureRequestDTO?> GetDiaryEntryAsync(string originalId, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    string getExisting =
		    @$"SELECT [{nameof(ProcedureRequestDTO.Id)}]					  = diary.[Id]
				      ,[{nameof(ProcedureRequestDTO.OriginalId)}]			  = diary.[OriginalId]
				      ,[{nameof(ProcedureRequestDTO.Intent)}]				  = diary.[Intent]
				      ,[{nameof(ProcedureRequestDTO.Status)}]				  = diary.[Status]
				      ,[{nameof(ProcedureRequestDTO.AuthoredOn)}]			  = diary.[AuthoredOn]
				      ,[{nameof(ProcedureRequestDTO.Occurence)}]			  = diary.[Occurence]
				      ,[{nameof(ProcedureRequestDTO.Category)}]				  = diary.[Category]
				      ,[{nameof(ProcedureRequestDTO.Reason)}]			      = diary.[Reason]
				      ,[{nameof(ProcedureRequestDTO.BodySite)}]				  = diary.[BodySite]
				      ,[{nameof(ProcedureRequestDTO.NoteText)}]				  = diary.[NoteText]
				      ,[{nameof(ProcedureRequestDTO.NoteAuthored)}]			  = diary.[NoteAuthored]
				      ,[{nameof(PracticionerDTO.EntityId)}]					  = diary.[EntityId]
					  ,[{nameof(PatientDTO.Id)}]							  = patient.Id
				      ,[{nameof(PatientDTO.OriginalId)}]                  	  = patient.OriginalId
				      ,[{nameof(PatientDTO.Gender)}]                    	  = patient.Gender
				      ,[{nameof(PatientDTO.Title)}]                        	  = patient.Title
				      ,[{nameof(PatientDTO.GivenName)}]                   	  = patient.GivenName
					  ,[{nameof(PatientDTO.MiddleNames)}]                  	  = patient.MiddleNames
				      ,[{nameof(PatientDTO.Surname)}]						  = patient.Surname
  					  ,[{nameof(PatientDTO.DateOfBirthUTC)}]               	  = patient.DateOfBirthUtc
				      ,[{nameof(PatientDTO.DateOfDeathUTC)}]                  = patient.DateOfDeathUTC
				      ,[{nameof(PatientDTO.DateOfRegistrationUTC)}]           = patient.DateOfRegistrationUTC
				      ,[{nameof(PatientDTO.NhsNumber)}]             		  = patient.NhsNumber
				      ,[{nameof(PatientDTO.PatientTypeDescription)}]          = patient.PatientTypeDescription
				      ,[{nameof(PatientDTO.DummyType)}]                       = patient.DummyType
					  ,[{nameof(PatientDTO.ResidentialInstituteCode)}]        = patient.ResidentialInstituteCode
				      ,[{nameof(PatientDTO.NHSNumberStatus)}]				  = patient.NHSNumberStatus
				      ,[{nameof(PatientDTO.CarerName)}]						  = patient.CarerName
  					  ,[{nameof(PatientDTO.DateOfBirthUTC)}]                  = patient.DateOfBirthUtc
				      ,[{nameof(PatientDTO.OriginalId)}]                  	  = patient.OriginalId
				      ,[{nameof(PatientDTO.CarerRelation)}]                   = patient.CarerRelation
				      ,[{nameof(PatientDTO.PersonGuid)}]             		  = patient.PersonGuid
				      ,[{nameof(PatientDTO.DateOfDeactivation)}]              = patient.DateOfDeactivation
				      ,[{nameof(PatientDTO.Deleted)}]                    	  = patient.Deleted
					  ,[{nameof(PatientDTO.Active)}]                  		  = patient.Active
				      ,[{nameof(PatientDTO.SpineSensitive)}]				  = patient.SpineSensitive
				      ,[{nameof(PatientDTO.IsConfidential)}]				  = patient.IsConfidential
				      ,[{nameof(PatientDTO.EmailAddress)}]               	  = patient.EmailAddress
				      ,[{nameof(PatientDTO.HomePhone)}]						  = patient.HomePhone
					  ,[{nameof(PatientDTO.MobilePhone)}]					  = patient.MobilePhone
					  ,[{nameof(PatientDTO.ProcessingId)}]					  = patient.ProcessingId
					  ,[{nameof(PatientDTO.Ethnicity)}]						  = patient.Ethnicity
					  ,[{nameof(PatientDTO.Religion)}]						  = patient.Religion
					  ,[{nameof(CodeDTO.Id)}]                  				  = code.Id
				      ,[{nameof(CodeDTO.ReadCode)}]                  		  = code.ReadCode
				      ,[{nameof(CodeDTO.SnomedCode)}]                  		  = code.SnomedCode
				      ,[{nameof(CodeDTO.Description)}]                  	  = code.Description
					  ,[{nameof(OutboundRelationship.Id)}]                    = performer.Id
					  ,[{nameof(OutboundRelationship.Type)}]                  = performer.EntityType
					  ,[{nameof(OrganizationDTO.Id)}]						  = organization.Id
				      ,[{nameof(OrganizationDTO.ODSCode)}]                    = organization.ODSCode
				      ,[{nameof(OrganizationDTO.PeriodStart)}]                = organization.PeriodStart
				      ,[{nameof(OrganizationDTO.PeriodEnd)}]                  = organization.PeriodEnd
				      ,[{nameof(OrganizationDTO.Type)}]                       = organization.Type
				      ,[{nameof(OrganizationDTO.Name)}]                       = organization.Name
				      ,[{nameof(OrganizationDTO.Telecom)}]                    = organization.Telecom
					  ,[{nameof(OrganizationDTO.EntityId)}]                   = organization.EntityId
					  ,[{nameof(OutboundRelationship.Id)}]                    = context.Id
					  ,[{nameof(OutboundRelationship.Type)}]                  = context.EntityType
					  ,[{nameof(ObservationDTO.Id)}]						  = observation.Id
				      ,[{nameof(ObservationDTO.OriginalId)}]                  = observation.OriginalId
				      ,[{nameof(ObservationDTO.Status)}]                  	  = observation.Status
				      ,[{nameof(ObservationDTO.Category)}]                    = observation.Category
				      ,[{nameof(ObservationDTO.Context)}]                  	  = observation.ContextId
				      ,[{nameof(ObservationDTO.EffectiveDate)}]               = observation.EffectiveDate
				      ,[{nameof(ObservationDTO.EffectiveDateFrom)}]           = observation.EffectiveDateFrom
				      ,[{nameof(ObservationDTO.EffectiveDateTo)}]             = observation.EffectiveDateTo
				      ,[{nameof(ObservationDTO.Issued)}]                  	  = observation.Issued
				      ,[{nameof(ObservationDTO.Interpretation)}]              = observation.Interpretation
				      ,[{nameof(ObservationDTO.DataAbsentReason)}]            = observation.DataAbsentReason
				      ,[{nameof(ObservationDTO.Comment)}]                  	  = observation.Comment
				      ,[{nameof(ObservationDTO.BodySite)}]                    = observation.BodySite
				      ,[{nameof(ObservationDTO.Method)}]                  	  = observation.Method
				      ,[{nameof(ObservationDTO.ReferenceRangeLow)}]           = observation.ReferenceRangeLow
				      ,[{nameof(ObservationDTO.ReferenceRangeHigh)}]          = observation.ReferenceRangeHigh
				      ,[{nameof(ObservationDTO.ReferenceRangeType)}]          = observation.ReferenceRangeType
				      ,[{nameof(ObservationDTO.ReferenceRangeAppliesTo)}]     = observation.ReferenceRangeAppliesTo
				      ,[{nameof(ObservationDTO.ReferenceRangeAgeHigh)}]       = observation.ReferenceRangeAgeHigh
				      ,[{nameof(ObservationDTO.ReferenceRangeAgeLow)}]        = observation.ReferenceRangeAgeLow

				  FROM [dbo].[DiaryEntry] diary
				  LEFT JOIN [dbo].[Patient] patient ON patient.Id = diary.[Subject]
				  LEFT JOIN [dbo].[Coding] code ON code.Id = diary.Code
				  LEFT JOIN [dbo].[Entity] performer ON performer.Id = diary.Performer
				  LEFT JOIN [dbo].[Organization] organization ON organization.Id = diary.OnBehalfOf
				  LEFT JOIN [dbo].[Entity] context ON context.Id = diary.Context
				  LEFT JOIN [dbo].[Observation] observation ON observation.Id = diary.SupportingInfo
				  WHERE diary.OriginalId = @OriginalId";
        
            var reader = await _connection.QueryMultipleAsync(getExisting, new
            {
                OriginalId = originalId
            }, transaction: transaction);
            var locations = reader.Read<ProcedureRequestDTO, PatientDTO?, CodeDTO?,OutboundRelationship?,OrganizationDTO?,OutboundRelationship?,ObservationDTO?,ProcedureRequestDTO>(
                (diary, patient, code, performer, onBehalfOf, context, supportingInfo) =>
                {
	                if (patient is not null)
                    {
	                    diary.Subject = patient;
                    }
	                if (code is not null)
	                {
		                diary.Code = code;
	                }
	                if (performer is not null)
	                {
		                diary.Performer = performer;
	                }
	                if (onBehalfOf is not null)
	                {
		                diary.OnBehalfOf = onBehalfOf;
	                }
	                if (context is not null)
	                {
		                diary.Context = context;
	                }
	                if (supportingInfo is not null)
	                {
		                diary.SupportingInfo = supportingInfo;
	                }
	                
	                return diary;
                }, splitOn: $"{nameof(PatientDTO.Id)},{nameof(CodeDTO.Id)},{nameof(OutboundRelationship.Id)},{nameof(OrganizationDTO.Id)},{nameof(OutboundRelationship.Id)},{nameof(ObservationDTO.Id)}");

            return locations.FirstOrDefault();
    }

    public async Task<Guid> InsertDiaryEntryAsync(ProcedureRequestDTO diaryEntry, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    diaryEntry.Id = Guid.NewGuid();
	    diaryEntry.EntityId = Guid.NewGuid();
                    
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
            Id = diaryEntry.EntityId,
            Type = EntityTypes.DiaryEntry,
            OriginalId = diaryEntry.OriginalId
        }, cancellationToken: cancellationToken, transaction: transaction);
                    
        await _connection.ExecuteAsync(entityDefinition);

        const string insertDiaryEntry =
                @"INSERT INTO [dbo].[DiaryEntry]
		           ([Id]
		           ,[OriginalId]
		           ,[Intent]
		           ,[Status]
		           ,[AuthoredOn]
		           ,[Occurence]
		           ,[Category]
		           ,[Code]
		           ,[Subject]
		           ,[Context]
		           ,[Encounter]
		           ,[Requester]
		           ,[OnBehalfOf]
		           ,[Performer]
		           ,[Reason]
		           ,[ReasonReference]
		           ,[SupportingInfo]
		           ,[BodySite]
		           ,[NoteText]
		           ,[NoteAuthored]
		           ,[NoteAuthor]
		           ,[EntityId])
		          VALUES
		           (@Id
		           ,@OriginalId
		           ,@Intent
		           ,@Status
		           ,@AuthoredOn
		           ,@Occurence
		           ,@Category
		           ,@Code
		           ,(select Id from Patient where OriginalId = @Subject)
		           ,(select Id from Entity where OriginalId = @Context)
		           ,(select Id from Encounter where OriginalId = @Encounter)
		           ,(select Id from Entity where OriginalId = @Requester)
		           ,(select Id from Organization where OriginalId = @OnBehalfOf)
		           ,(select Id from Entity where OriginalId = @Performer)
		           ,@Reason
		           ,(select Id from Entity where OriginalId = @ReasonReference)
		           ,(select Id from Observation where OriginalId = @SupportingInfo)
		           ,@BodySite
		           ,@NoteText
		           ,@NoteAuthored
		           ,(select Id from Entity where OriginalId = @NoteAuthor)
		           ,@EntityId)";
            
            var commandDefinition = new CommandDefinition(insertDiaryEntry, new
            {
                Id = diaryEntry.Id,
                OriginalId = diaryEntry.OriginalId,
                Intent = diaryEntry.Intent,
                Status = diaryEntry.Status,
                AuthoredOn = diaryEntry.AuthoredOn,
                Occurence = diaryEntry.Occurence,
                Category = diaryEntry.Category,
                Code = diaryEntry.Code?.Id,
                Subject = diaryEntry.Subject?.OriginalId,
                Context = diaryEntry.Context?.OriginalId,
                Encounter = diaryEntry.Encounter?.OriginalId,
                Requester = diaryEntry.Requester?.OriginalId,
                OnBehalfOf = diaryEntry.OnBehalfOf?.OriginalId,
                Performer = diaryEntry.Performer?.OriginalId,
                Reason = diaryEntry.Reason,
                ReasonReference = diaryEntry.ReasonReference?.OriginalId,
                SupportingInfo = diaryEntry.SupportingInfo?.OriginalId,
                BodySite = diaryEntry.BodySite,
                NoteText = diaryEntry.NoteText,
                NoteAuthored = diaryEntry.NoteAuthored,
                NoteAuthor = diaryEntry.NoteAuthor?.OriginalId,
                EntityId = diaryEntry.EntityId
                
            }, cancellationToken: cancellationToken, transaction: transaction);
            
            var result = await _connection.ExecuteAsync(commandDefinition);
            if (result == 0)
            {
                throw new DataException("Error: User request was not successful.");
            }

            return diaryEntry.Id;
    }
    
    public async Task<Guid> InsertProcedureRequestAsync(ProcedureRequestDTO procedureRequest, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    procedureRequest.Id = Guid.NewGuid();
	    procedureRequest.EntityId = Guid.NewGuid();
                    
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
            Id = procedureRequest.EntityId,
            Type = EntityTypes.ProcedureRequest,
            OriginalId = procedureRequest.OriginalId
        }, cancellationToken: cancellationToken, transaction: transaction);
                    
        await _connection.ExecuteAsync(entityDefinition);

        const string insertProcedureRequest =
                @"INSERT INTO [dbo].[ProcedureRequest]
		           ([Id]
		           ,[OriginalId]
		           ,[Intent]
		           ,[Status]
		           ,[AuthoredOn]
		           ,[Occurence]
		           ,[Category]
		           ,[Code]
		           ,[Subject]
		           ,[Context]
		           ,[Encounter]
		           ,[Requester]
		           ,[OnBehalfOf]
		           ,[Performer]
		           ,[Reason]
		           ,[ReasonReference]
		           ,[SupportingInfo]
		           ,[BodySite]
		           ,[NoteText]
		           ,[NoteAuthored]
		           ,[NoteAuthor]
		           ,[EntityId])
		          VALUES
		           (@Id
		           ,@OriginalId
		           ,@Intent
		           ,@Status
		           ,@AuthoredOn
		           ,@Occurence
		           ,@Category
		           ,@Code
		           ,(select Id from Patient where OriginalId = @Subject)
		           ,(select Id from Entity where OriginalId = @Context)
		           ,(select Id from Encounter where OriginalId = @Encounter)
		           ,(select Id from Entity where OriginalId = @Requester)
		           ,(select Id from Organization where OriginalId = @OnBehalfOf)
		           ,(select Id from Entity where OriginalId = @Performer)
		           ,@Reason
		           ,(select Id from Entity where OriginalId = @ReasonReference)
		           ,(select Id from Observation where OriginalId = @SupportingInfo)
		           ,@BodySite
		           ,@NoteText
		           ,@NoteAuthored
		           ,(select Id from Entity where OriginalId = @NoteAuthor)
		           ,@EntityId)";
            
            var commandDefinition = new CommandDefinition(insertProcedureRequest, new
            {
                Id = procedureRequest.Id,
                OriginalId = procedureRequest.OriginalId,
                Intent = procedureRequest.Intent,
                Status = procedureRequest.Status,
                AuthoredOn = procedureRequest.AuthoredOn,
                Occurence = procedureRequest.Occurence,
                Category = procedureRequest.Category,
                Code = procedureRequest.Code?.Id,
                Subject = procedureRequest.Subject?.OriginalId,
                Context = procedureRequest.Context?.OriginalId,
                Encounter = procedureRequest.Encounter?.OriginalId,
                Requester = procedureRequest.Requester?.OriginalId,
                OnBehalfOf = procedureRequest.OnBehalfOf?.OriginalId,
                Performer = procedureRequest.Performer?.OriginalId,
                Reason = procedureRequest.Reason,
                ReasonReference = procedureRequest.ReasonReference?.OriginalId,
                SupportingInfo = procedureRequest.SupportingInfo?.OriginalId,
                BodySite = procedureRequest.BodySite,
                NoteText = procedureRequest.NoteText,
                NoteAuthored = procedureRequest.NoteAuthored,
                NoteAuthor = procedureRequest.NoteAuthor?.OriginalId,
                EntityId = procedureRequest.EntityId
                
            }, cancellationToken: cancellationToken, transaction: transaction);
            
            var result = await _connection.ExecuteAsync(commandDefinition);
            if (result == 0)
            {
                throw new DataException("Error: User request was not successful.");
            }

            return procedureRequest.Id;
    }

    public async Task UpdatePracticionerAsync(PracticionerDTO practicioner, CancellationToken cancellationToken, IDbTransaction transaction)
    {
         const string updateOrganization =
                @"UPDATE [dbo].[Practicioner]
				  SET
                       [Id] = @Id,
					   [OriginalId] = @OriginalId,
					   [SdsUserId] = @SdsUserId,
					   [SdsRoleProfileId] = @SdsRoleProfileId,
					   [Title] = @Title,
					   [GivenName] = @GivenName,
					   [MiddleNames] = @MiddleNames,
					   [Surname] = @Surname,
					   [Gender] = @Gender,
					   [DateOfBirthUtc] = @DateOfBirthUtc,
					   [AddressID] = @AddressID
                 WHERE Id = @Id";
            
            var commandDefinition = new CommandDefinition(updateOrganization, new
            {
	            Id = practicioner.Id,
	            OriginalId = practicioner.OriginalId,
	            SdsUserId = practicioner.SdsUserId,
	            SdsRoleProfileId = practicioner.SdsRoleProfileId,
	            Title = practicioner.Title,
	            GivenName = practicioner.GivenName,
	            MiddleNames = practicioner.MiddleNames,
	            Surname = practicioner.Surname,
	            Gender = practicioner.Gender,
	            Address = practicioner.Address?.Id,
	            DateOfBirthUtc = practicioner.DateOfBirthUtc,
	            AddressID = practicioner.Address?.Id

            }, cancellationToken: cancellationToken, transaction: transaction);
            
            var result = await _connection.ExecuteAsync(commandDefinition);
            if (result == 0)
            {
                throw new DataException("Error: User request was not successful.");
            }
    }

}