using System.Reflection;
using System.Runtime.Serialization;
using DotNetGPSystem;
using GPConnect.Provider.AcceptanceTests.Http;
using GPMigratorApp.Data.Types;
using GPMigratorApp.DTOs;
using GPMigratorApp.GPConnect.Helpers;
using Hl7.Fhir.Introspection;
using Hl7.Fhir.Model;

namespace GPMigratorApp.GPConnect.Profiles;

[Serializable]
[DataContract]
[FhirType("Encounter","http://hl7.org/fhir/StructureDefinition/ProcedureRequest", IsResource=true)]
public class GPConnectProcedureRequest : ProcedureRequest
{
    private EpisodeOfCare? _episodeOfCare;
    private IEnumerable<PracticionerDTO>? _practicioners;
    private IEnumerable<PatientDTO>? _patients;
    private IEnumerable<OrganizationDTO> _organizations;
    private IEnumerable<ConditionDTO> _conditions;
    private IEnumerable<ObservationDTO> _observations;
    public GPConnectProcedureRequest(ProcedureRequest procedureRequest, FhirResponse bundle)
    {       
        InitInhertedProperties(procedureRequest);
        _practicioners = bundle.Practitioners;
        _patients = bundle.Patients;
        _organizations = bundle.Organizations;
        _conditions = bundle.Conditions;
        _observations = bundle.Observations;
    }

    public ProcedureRequestDTO GetDTO()
    {
        var dto = new ProcedureRequestDTO() 
        {
            OriginalId = this.Id,
            Identifier = new IdentifierDTO(this.Identifier.FirstOrDefault()),
            Intent = (int)this.Intent.Value,
            Status = this.Status.Value.ToString(),
            Category = this.Category?.FirstOrDefault()?.Coding?.FirstOrDefault()?.Code,
            Subject = _patients.FirstOrDefault(x=> x.OriginalId == ReferenceHelper.GetId(this.Subject.Reference)), 
            Context = null,
            OnBehalfOf = _organizations.FirstOrDefault(x=> x.OriginalId == ReferenceHelper.GetId(this.Requester?.OnBehalfOf?.Reference)),
            Reason = this.ReasonCode?.FirstOrDefault()?.Coding?.FirstOrDefault()?.Code,
            BodySite = this.BodySite?.FirstOrDefault()?.Coding.FirstOrDefault()?.Code,
            NoteText = this.Note?.FirstOrDefault()?.Text,
            Occurence = DateTime.Parse(this.Occurrence?.FirstOrDefault().Value.ToString() ?? string.Empty),
            AuthoredOn = DateTime.Parse(this.AuthoredOn)
        };

        if (this.Note?.FirstOrDefault()?.Time is not null)
        {
            dto.NoteAuthored = DateTime.Parse(this.Note?.FirstOrDefault()?.Time);
        }
        
        var requester = this.Requester?.Agent?.FirstOrDefault().Value.ToString();
        if (requester != null)
        {
            dto.Requester = new OutboundRelationship
            {
                OriginalId = ReferenceHelper.GetId(requester),
                Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(requester))
            };
        }

        if (this.Performer != null && this.Performer.Any())
        {
            dto.Performer = new OutboundRelationship
            {
                OriginalId = ReferenceHelper.GetId(this.Performer.Reference),
                Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(this.Performer.Reference))
            };
        }

        if (this.ReasonReference != null && this.ReasonReference.Any())
        {
            dto.ReasonReference = new OutboundRelationship
            {
                OriginalId = ReferenceHelper.GetId(this.ReasonReference?.FirstOrDefault()?.Reference),
                Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(this.ReasonReference?.FirstOrDefault()?.Reference))
            };
        }
        
        if (this.SupportingInfo != null)
        {
            dto.SupportingInfo = _observations.FirstOrDefault(x =>
                x.OriginalId == ReferenceHelper.GetId(this.SupportingInfo?.FirstOrDefault()?.Reference));
        }
        
        if (this.Code is not null)
        {
            dto.Code = new CodeDTO
            {
                ReadCode = this.Code.Coding?.FirstOrDefault(x => x.System == "http://read.info/readv2")?.Code,

                Description = this.Code.Coding?.FirstOrDefault(x=> x.System == "http://snomed.info/sct")?.Display
            };
            var code = this.Code.Coding?.FirstOrDefault(x => x.System == "http://snomed.info/sct")?.Code;
            if (code != null)
                dto.Code.SnomedCode =code;
        }
        
        if (this.Note?.FirstOrDefault()?.Author != null)
        {
            dto.NoteText = this.Note.FirstOrDefault().Text;
            dto.NoteAuthored = DateTime.Parse(this.Note.FirstOrDefault().Time);
            var author = (ResourceReference?) this.Note?.FirstOrDefault()?.Author;
            if (author is not null)
            {
                dto.NoteAuthor = new OutboundRelationship
                {
                    OriginalId = ReferenceHelper.GetId(author.Reference),
                    Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(author.Reference))
                };
            }
        }
        
        return dto;
    }
    private void InitInhertedProperties (object encounter)
    {
        foreach (var propertyInfo in encounter.GetType().GetProperties())
        {
            var props = typeof(ProcedureRequest).GetProperties().Where(p => !p.GetIndexParameters().Any());
            foreach (var prop in props)
            {
                if (prop.CanWrite)
                    prop.SetValue(this, prop.GetValue(encounter));
            }
        }
    }

    
}