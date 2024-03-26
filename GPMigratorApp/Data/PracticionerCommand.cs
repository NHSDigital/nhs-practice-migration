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

public class PracticionerCommand : IPracticionerCommand
{
    private readonly IDbConnection _connection;
    
    public PracticionerCommand(IDbConnection connection)
    {
        _connection = connection;
    }
    
        public async Task<PracticionerDTO> GetSinglePracticionerAsync(string originalId,
	    CancellationToken cancellationToken)
    {
	    string getExisting =
		    @$"SELECT  [{nameof(PracticionerRoleDTO.Id)}]						= pracRole.Id
                      ,[{nameof(PracticionerRoleDTO.OriginalId)}]               = pracRole.OriginalId
                      ,[{nameof(PracticionerRoleDTO.Active)}]                   = pracRole.Active
                      ,[{nameof(PracticionerRoleDTO.PeriodStart)}]              = pracRole.PeriodStart
                      ,[{nameof(PracticionerRoleDTO.PeriodEnd)}]                = pracRole.PeriodEnd
                      ,[{nameof(PracticionerRoleDTO.SDSJobRoleName)}]           = pracRole.SDSJobRoleName
					  ,[{nameof(PracticionerRoleDTO.Speciality)}]               = pracRole.Speciality
                      ,[{nameof(PracticionerRoleDTO.Telecom)}]					= pracRole.Telecom
                      ,[{nameof(PracticionerDTO.Id)}]							= prac.Id
                      ,[{nameof(PracticionerDTO.OriginalId)}]                  	= prac.OriginalId
                      ,[{nameof(PracticionerDTO.SdsUserId)}]                    = prac.SdsUserId
                      ,[{nameof(PracticionerDTO.SdsRoleProfileId)}]             = prac.SdsRoleProfileId
                      ,[{nameof(PracticionerDTO.Title)}]                        = prac.Title
                      ,[{nameof(PracticionerDTO.GivenName)}]                    = prac.GivenName
					  ,[{nameof(PracticionerDTO.MiddleNames)}]                  = prac.MiddleNames
                      ,[{nameof(PracticionerDTO.Surname)}]						= prac.Surname
                      ,[{nameof(PracticionerDTO.Gender)}]						= prac.Gender
  					  ,[{nameof(PracticionerDTO.DateOfBirthUtc)}]               = prac.DateOfBirthUtc
					  ,[{nameof(OrganizationDTO.Id)}]							= org.Id
                      ,[{nameof(OrganizationDTO.ODSCode)}]                      = org.ODSCode
                      ,[{nameof(OrganizationDTO.PeriodStart)}]                  = org.PeriodStart
                      ,[{nameof(OrganizationDTO.PeriodEnd)}]                    = org.PeriodEnd
                      ,[{nameof(OrganizationDTO.Type)}]                         = org.Type
                      ,[{nameof(OrganizationDTO.Name)}]                         = org.Name
                      ,[{nameof(OrganizationDTO.Telecom)}]                      = org.Telecom
					  ,[{nameof(OrganizationDTO.EntityId)}]                     = org.EntityId
					  ,[{nameof(LocationDTO.Id)}]								= loc.Id
                      ,[{nameof(LocationDTO.OriginalId)}]                  	    = loc.OriginalId
                      ,[{nameof(LocationDTO.ODSSiteCode)}]                  	= loc.ODSSiteCodeID
                      ,[{nameof(LocationDTO.Status)}]                  			= loc.Status
                      ,[{nameof(LocationDTO.OperationalStatus)}]                = loc.OperationalStatus
                      ,[{nameof(LocationDTO.Name)}]                         	= loc.Name
                      ,[{nameof(LocationDTO.Alias)}]                         	= loc.Alias
                      ,[{nameof(LocationDTO.Description)}]                      = loc.Description
					  ,[{nameof(LocationDTO.Type)}]                     		= loc.Type
                      ,[{nameof(LocationDTO.Telecom)}]							= loc.Telecom
                      ,[{nameof(LocationDTO.PhysicalType)}]						= loc.PhysicalType
  					  ,[{nameof(LocationDTO.EntityId)}]                     	= loc.EntityId
                  FROM [dbo].[PracticionerRole] pracRole
				  LEFT JOIN [dbo].[Practicioner] prac ON prac.Id = pracRole.Practicioner
				  LEFT JOIN [dbo].[Organization] org ON org.Id = pracRole.Organization
				  LEFT JOIN [dbo].[Location] loc ON loc.Id = pracRole.Location
				  WHERE pracRole.OriginalId = @OriginalId";

	    var reader = await _connection.QueryMultipleAsync(getExisting, new
		    {
			    OriginalId = originalId
			    
		    }
	 

	    );
	    var patient = reader.Read<PracticionerDTO>().FirstOrDefault();
	    return patient;
    }


    public async Task<IEnumerable<PracticionerRoleDTO>> GetAllPractitionerRecordsAsync(CancellationToken cancellationToken)
    {
	    string getExisting =
		    @$"SELECT
				     [{nameof(PracticionerRoleDTO.Id)}]							= practicionerRole.Id
					,[{nameof(PracticionerRoleDTO.OriginalId)}]					= practicionerRole.OriginalId
					,[{nameof(PracticionerRoleDTO.Active)}]						= practicionerRole.Active
					,[{nameof(PracticionerRoleDTO.PeriodStart)}]				= practicionerRole.PeriodStart
					,[{nameof(PracticionerRoleDTO.PeriodEnd)}]					= practicionerRole.PeriodEnd
					,[{nameof(PracticionerRoleDTO.SDSJobRoleName)}]				= practicionerRole.SDSJobRoleName
					,[{nameof(PracticionerRoleDTO.Speciality)}]					= practicionerRole.Speciality
					,[{nameof(PracticionerRoleDTO.Telecom)}]					= practicionerRole.Telecom
  					,[{nameof(PracticionerDTO.Id)}]							    = practicioner.Id
					,[{nameof(PracticionerDTO.Title)}]							= practicioner.Title
					,[{nameof(PracticionerDTO.GivenName)}]						= practicioner.GivenName
					,[{nameof(PracticionerDTO.MiddleNames)}]					= practicioner.MiddleNames
					,[{nameof(PracticionerDTO.Surname)}]						= practicioner.Surname
					,[{nameof(PracticionerDTO.OriginalId)}]					    = practicioner.OriginalId
				  	,[{nameof(OrganizationDTO.Id)}]							    = organization.Id
                    ,[{nameof(OrganizationDTO.ODSCode)}]                        = organization.ODSCode
                    ,[{nameof(OrganizationDTO.PeriodStart)}]                    = organization.PeriodStart
                    ,[{nameof(OrganizationDTO.PeriodEnd)}]                      = organization.PeriodEnd
                    ,[{nameof(OrganizationDTO.Type)}]                           = organization.Type
                    ,[{nameof(OrganizationDTO.Name)}]                           = organization.Name
                    ,[{nameof(OrganizationDTO.Telecom)}]                        = organization.Telecom
				    ,[{nameof(OrganizationDTO.EntityId)}]                       = organization.EntityId

	FROM [GPData].[dbo].[PracticionerRole] practicionerRole
	LEFT JOIN [dbo].[Practicioner] practicioner 
	ON practicioner.[Id] = practicionerRole.[practicioner]
	LEFT JOIN [dbo].[Organization] organization 
	ON organization.[Id] = practicionerRole.[Organization]

";

	    var reader = await _connection.QueryMultipleAsync(getExisting);
	 
	    var practicionerRoles = reader.Read<PracticionerRoleDTO, PracticionerDTO, OrganizationDTO?, PracticionerRoleDTO>(
		    (practicionerRole, practicioner, organization) =>
		    {
			    practicionerRole.Practicioner = practicioner;

			    if (organization is not null)
			    {
				    practicionerRole.Organization = organization;
			    }
			    
	                
			    return practicionerRole;
		    }, splitOn: $"{nameof(PracticionerDTO.Id)},{nameof(OrganizationDTO.Id)}");

	    return practicionerRoles;
    }

    public async Task<PracticionerDTO?> GetPracticionerAsync(string originalId, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    string getExisting =
		    @$"SELECT [{nameof(PracticionerDTO.Id)}]							= prac.Id
                      ,[{nameof(PracticionerDTO.OriginalId)}]                  	= prac.OriginalId
                      ,[{nameof(PracticionerDTO.SdsUserId)}]                    = prac.SdsUserId
                      ,[{nameof(PracticionerDTO.SdsRoleProfileId)}]             = prac.SdsRoleProfileId
                      ,[{nameof(PracticionerDTO.Title)}]                        = prac.Title
                      ,[{nameof(PracticionerDTO.GivenName)}]                    = prac.GivenName
					  ,[{nameof(PracticionerDTO.MiddleNames)}]                  = prac.MiddleNames
                      ,[{nameof(PracticionerDTO.Surname)}]						= prac.Surname
                      ,[{nameof(PracticionerDTO.Gender)}]						= prac.Gender
  					  ,[{nameof(PracticionerDTO.DateOfBirthUtc)}]               = prac.DateOfBirthUtc
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

                  FROM [dbo].[Practicioner] prac
				  LEFT JOIN [dbo].[Address] address ON address.Id = prac.AddressID
				  WHERE prac.OriginalId = @OriginalId";
        
            var reader = await _connection.QueryMultipleAsync(getExisting, new
            {
                OriginalId = originalId
            }, transaction: transaction);
            var locations = reader.Read<PracticionerDTO, AddressDTO?, PracticionerDTO>(
                (practicioner, address) =>
                {
	                if (address is not null)
                    {
	                    practicioner.Address = address;
                    }
	                
	                return practicioner;
                }, splitOn: $"{nameof(AddressDTO.Id)}");

            return locations.FirstOrDefault();
    }

    public async Task<Guid> InsertPracticionerAsync(PracticionerDTO practicioner, CancellationToken cancellationToken, IDbTransaction transaction)
    {
	    practicioner.Id = Guid.NewGuid();
	    practicioner.EntityId = Guid.NewGuid();
                    
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
            Id = practicioner.EntityId,
            Type = EntityTypes.Practitioner,
            OriginalId = practicioner.OriginalId
        }, cancellationToken: cancellationToken, transaction: transaction);
                    
        await _connection.ExecuteAsync(entityDefinition);
        
        

        if (practicioner.Address is not null)
        {
	        practicioner.Address.Id = Guid.NewGuid();
	        
	        var address = @"INSERT INTO [dbo].[Address]
                       ([Id]
                       ,[Use]
                       ,[HouseNameFlatNumber]
                       ,[NumberAndStreet]
                       ,[Village]
                       ,[Town]
                       ,[County]
                       ,[Postcode]
                       ,[From]
                       ,[To])
                 VALUES
                       (@Id
                       ,@Use
                       ,@HouseName
                       ,@Number
                       ,@Village
                       ,@Town
                       ,@County
                       ,@Postcode
                       ,@From
                       ,@To)";

	        var practicionerAddressDefinition = new CommandDefinition(address, new
	        {
		        Id = practicioner.Address.Id,
		        Use = practicioner.Address.Use,
		        HouseName = practicioner.Address.HouseNameFlatNumber,
		        Number = practicioner.Address.NumberAndStreet,
		        Village = practicioner.Address.Village,
		        Town = practicioner.Address.Town,
		        County = practicioner.Address.County,
		        Postcode = practicioner.Address.Postcode,
		        From = practicioner.Address.From,
		        To = practicioner.Address.To
	        }, cancellationToken: cancellationToken, transaction: transaction);

	        await _connection.ExecuteAsync(practicionerAddressDefinition);
        }

        const string insertPracticioner =
                @"INSERT INTO [dbo].[Practicioner]
                       	  ([Id],
						  [OriginalId],
						  [SdsUserId],
						  [SdsRoleProfileId],
						  [Title],
						  [GivenName],
						  [MiddleNames],
						  [Surname],
						  [Gender],
						  [DateOfBirthUtc],
						  [AddressID])
                 VALUES
                       (@Id,
                       @OriginalId,
                       @SdsUserId,
                       @SdsRoleProfileId,
                       @Title,
                       @GivenName,
                       @MiddleNames,
                       @Surname,
                       @Gender,
                       @DateOfBirthUtc,
                       @AddressID)";
            
            var commandDefinition = new CommandDefinition(insertPracticioner, new
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
                DateOfBirthUtc = practicioner.DateOfBirthUtc,
                AddressID = practicioner.Address?.Id
                
            }, cancellationToken: cancellationToken, transaction: transaction);
            
            var result = await _connection.ExecuteAsync(commandDefinition);
            if (result == 0)
            {
                throw new DataException("Error: User request was not successful.");
            }

            return practicioner.Id;
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