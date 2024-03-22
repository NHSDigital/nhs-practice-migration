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
[FhirType("Encounter","http://hl7.org/fhir/StructureDefinition/Condition", IsResource=true)]
public class GPConnectCondition : Condition
{
    private EpisodeOfCare? _episodeOfCare;
    private IEnumerable<PracticionerDTO>? _practicioners;
    private IEnumerable<PatientDTO>? _patients;
    public GPConnectCondition(Condition condition, FhirResponse bundle)
    {       
        InitInhertedProperties(condition);
        _practicioners = bundle.Practitioners;
        _patients = bundle.Patients;
    }

    public ConditionDTO GetDTO()
    {
        var dto = new ConditionDTO 
        {
            OriginalId = this.Id,
            Identifier = new IdentifierDTO(this.Identifier.FirstOrDefault()),
            ActualProblem = new OutboundRelationship {OriginalId = this.Extension.FirstOrDefault(x=> x.Url == "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ActualProblem-1")?.Value.FirstOrDefault().Value.ToString()},
            ProblemSignificance = this.Extension.FirstOrDefault(x=> x.Url == "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-ProblemSignificance-1")?.Value.ToString(),
            Episode = null,
            ClinicalStatus = this.ClinicalStatus?.ToString(),
            VerificationStatus = this.VerificationStatus?.ToString(),
            Severity = this.Severity?.Coding?.FirstOrDefault()?.Code,
            BodySite = this.BodySite?.FirstOrDefault()?.Coding?.FirstOrDefault()?.Code,
            Subject = _patients?.FirstOrDefault(x=> x.OriginalId == ReferenceHelper.GetId(this.Subject.Reference)),
            Context = null, // TODO ENCOUNTERS
            OnsetAge = null,
            OnsetPeriodStart = null,
            OnsetPeriodEnd = null,

            AbatementAge = null,
            AbatementPeriodStart = null,
            AbatementPeriodEnd = null,
            Abatement = null,
            AssertedDate = DateTime.Parse(this.AssertedDate),
            Asserter = _practicioners?.FirstOrDefault(x=> x.OriginalId == ReferenceHelper.GetId(this.Asserter.Reference)),
            NoteText = this.Note?.FirstOrDefault()?.Text,
            NoteAuthored = null,
            NoteAuthor = null
        };
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
        
        dto.Evidence = this.Evidence?
            .Select(problem =>
                new OutboundRelationship
                {
                    Code = problem.Code?.FirstOrDefault()?.Coding?.FirstOrDefault()?.Code,
                    OriginalId = ReferenceHelper.GetId(problem?.Detail?.FirstOrDefault()?.Reference),
                    Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(problem?.Detail?.FirstOrDefault()?.Reference))
                }).ToList();
        try
        {
            var extensions = this.Extension?.Where(x =>
                x.Url ==
                "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-RelatedProblemHeader-1");
            dto.RelatedProblem = this.Extension?.Where(x =>
                    x.Url ==
                    "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-RelatedProblemHeader-1")
                .Select(x => (ResourceReference) x.Extension.FirstOrDefault().Value)
                .Select(problem =>
                    new OutboundRelationship
                    {
                        OriginalId = ReferenceHelper.GetId(problem.Reference),
                        Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(problem.Reference))
                    }).ToList();
        }
        catch (Exception ex)
        {
            
        }

        dto.RelatedClinicalConditions = this.Extension?.Where(x =>
                x.Url ==
                "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-RelatedClinicalContent-1")
            .Select(x => (ResourceReference) x.Value)
            .Select(problem =>
                new OutboundRelationship
                {
                    OriginalId = ReferenceHelper.GetId(problem.Reference),
                    Type = (int?) EntityTypeHelper.GetEnumForType(ReferenceHelper.GetType(problem.Reference))
                }).ToList();
        
       

        if (this.Onset?.FirstOrDefault().Value is not null)
        {
            var success = DateTime.TryParse(this.Onset.ToString(), out var date);
            if (success)
                dto.OnsetDate = date;
            else
            {
                dto.OnsetDate = new DateTime(int.Parse(this.Onset.ToString()), 01, 01);
            }
        }
        
        if (this.Abatement?.FirstOrDefault().Value is not null)
        {
            var success = DateTime.TryParse(this.Abatement.ToString(), out var abatementDate);
            if (success)
                dto.AbatementDate = abatementDate;
            else
            {
                dto.AbatementDate = new DateTime(int.Parse(this.Abatement.ToString()), 01, 01);
            }
        }

        return dto;
    }
    private void InitInhertedProperties (object encounter)
    {
        foreach (var propertyInfo in encounter.GetType().GetProperties())
        {
            var props = typeof(Condition).GetProperties().Where(p => !p.GetIndexParameters().Any());
            foreach (var prop in props)
            {
                if (prop.CanWrite)
                    prop.SetValue(this, prop.GetValue(encounter));
            }
        }
    }

    
}