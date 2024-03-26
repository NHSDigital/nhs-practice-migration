using System.Reflection;
using System.Runtime.Serialization;
using DotNetGPSystem;
using GPConnect.Provider.AcceptanceTests.Http;
using GPMigratorApp.DTOs;
using GPMigratorApp.GPConnect.Helpers;
using Hl7.Fhir.Introspection;
using Hl7.Fhir.Model;

namespace GPMigratorApp.GPConnect.Profiles;

[Serializable]
[DataContract]
[FhirType("Encounter", "http://hl7.org/fhir/StructureDefinition/Encounter", IsResource = true)]
public class GPConnectImmunization
{
    private Immunization _immunization;
    private EpisodeOfCare? _episodeOfCare;
    private IEnumerable<PracticionerDTO>? _practicioners;
    private IEnumerable<OrganizationDTO>? _organizations;
    private IEnumerable<ObservationDTO>? _observations;
    private IEnumerable<LocationDTO>? _locations;
    private IEnumerable<PatientDTO>? _patients;

    public GPConnectImmunization(Immunization encounter, FhirResponse bundle)
    {
        _immunization = encounter;
        _practicioners = bundle.Practitioners;
        _patients = bundle.Patients;
        _locations = bundle.Locations;
        _observations = bundle.Observations;
        _organizations = bundle.Organizations;
    }

    public ImmunizationDTO GetDTO()
    {
        var dto = new ImmunizationDTO()
        {
            OriginalId = _immunization.Id,
            Status = _immunization.Status?.ToString(),
            Subject = _patients.FirstOrDefault(x =>
                x.OriginalId == ReferenceHelper.GetId(_immunization.Patient.Reference)),
            Encounter = null,
            Actor = _practicioners.FirstOrDefault(X =>
                X.OriginalId == ReferenceHelper.GetId(_immunization.Practitioner.FirstOrDefault(x => x.Actor != null)
                    ?.Actor.Reference)),
            Location = _locations.FirstOrDefault(x =>
                x.OriginalId == ReferenceHelper.GetId(_immunization.Location.Reference)),
            PrimarySource = _immunization.PrimarySource,
            LotNumber = _immunization.LotNumber,
            NoteText = string.Join(", ", _immunization.Note.Select(x => x.Text)),
            DoseQuantity = _immunization.DoseQuantity?.Value,
            ReactionReported = _immunization.Reaction.FirstOrDefault()?.Reported,
            ReactionDetail = _observations.FirstOrDefault(x =>
                x.OriginalId == ReferenceHelper.GetId(_immunization.Reaction.FirstOrDefault()?.Detail.Reference)),

            DoseSequence = _immunization.VaccinationProtocol.FirstOrDefault()?.DoseSequence,
            Description = _immunization.VaccinationProtocol.FirstOrDefault()?.Description,
            Authority = _organizations.FirstOrDefault(x =>
                x.OriginalId ==
                ReferenceHelper.GetId(_immunization.VaccinationProtocol.FirstOrDefault()?.Authority.Reference)),
            Series = _immunization.VaccinationProtocol.FirstOrDefault()?.Series,
            SeriesDoses = _immunization.VaccinationProtocol.FirstOrDefault()?.SeriesDoses,
            TargetDisease = _immunization.VaccinationProtocol.FirstOrDefault()?.TargetDisease.FirstOrDefault()?.Coding
                .FirstOrDefault()?.Code,
            DoseStatus = _immunization.VaccinationProtocol.FirstOrDefault()?.DoseStatus.FirstOrDefault().Value
                .ToString(),
            DoseStatusReason = _immunization.VaccinationProtocol.FirstOrDefault()?.DoseStatusReason.FirstOrDefault()
                .Value.ToString(),
        };
        var present = _immunization.Extension.FirstOrDefault(x =>
            x.Url == "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-ParentPresent-1")?.Value;

        if (present is not null)
            dto.ParentPresent = bool.Parse(present.ToString());

        
        if (_immunization.Site is not null)
        {
            dto.Site = new CodeDTO
            {
                ReadCode = _immunization.Site.Coding?.FirstOrDefault(x => x.System == "http://read.info/readv2")
                    ?.Code,

                Description = _immunization.Site.Coding
                    ?.FirstOrDefault(x => x.System == "http://snomed.info/sct")?.Display
            };
            var code = _immunization.Site.Coding?.FirstOrDefault(x => x.System == "http://snomed.info/sct")
                ?.Code;
            if (code != null)
                dto.Site .SnomedCode = code;
        }
        
        if (_immunization.Route is not null)
        {
            dto.Route = new CodeDTO
            {
                ReadCode = _immunization.Site.Coding?.FirstOrDefault(x => x.System == "http://read.info/readv2")
                    ?.Code,

                Description = _immunization.Site.Coding
                    ?.FirstOrDefault(x => x.System == "http://snomed.info/sct")?.Display
            };
            var code = _immunization.Site.Coding?.FirstOrDefault(x => x.System == "http://snomed.info/sct")
                ?.Code;
            if (code != null)
                dto.Route .SnomedCode = code;
        }

        if (_immunization.Reaction.FirstOrDefault()?.Date is not null)
        {
            var success = DateTime.TryParse(_immunization.Reaction.FirstOrDefault()?.Date, out var date);
            if (success)
                dto.ReactionDate = date;
            else
            {
                dto.ReactionDate = new DateTime(int.Parse(_immunization.Reaction.FirstOrDefault()?.Date), 01, 01);
            }
        }

        if (_immunization.Note?.FirstOrDefault()?.Time is not null)
            if (_immunization.Date is not null)
            {
                var success = DateTime.TryParse(_immunization.Note?.FirstOrDefault()?.Time, out var date);
                if (success)
                    dto.NoteDate = date;
                else
                {
                    dto.NoteDate =
                        new DateTime(int.Parse(_immunization.Note?.FirstOrDefault()?.Time.ToString() ?? string.Empty),
                            01, 01);
                }
            }

        if (_immunization.ExpirationDate is not null)
        {
            var success = DateTime.TryParse(_immunization.ExpirationDate, out var date);
            if (success)
                dto.ExpirationDate = date;
            else
            {
                dto.ExpirationDate = new DateTime(int.Parse(_immunization.ExpirationDate.ToString()), 01, 01);
            }
        }


        if (_immunization.Date is not null)
        {
            var success = DateTime.TryParse(_immunization.Date, out var date);
            if (success)
                dto.RecordedDate = date;
            else
            {
                dto.RecordedDate = new DateTime(int.Parse(_immunization.Date.ToString()), 01, 01);
            }
        }

        if (_immunization.VaccineCode is not null)
        {
            dto.VaccinationCode = new CodeDTO
            {
                ReadCode = _immunization.VaccineCode.Coding?.FirstOrDefault(x => x.System == "http://read.info/readv2")
                    ?.Code,

                Description = _immunization.VaccineCode.Coding
                    ?.FirstOrDefault(x => x.System == "http://snomed.info/sct")?.Display
            };
            var code = _immunization.VaccineCode.Coding?.FirstOrDefault(x => x.System == "http://snomed.info/sct")
                ?.Code;
            if (code != null)
                dto.VaccinationCode.SnomedCode = code;
        }

        var procedure = _immunization.Extension.FirstOrDefault(x =>
            x.Url == "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-VaccinationProcedure-1");
        if (procedure is not null)
        {
            var coding = (CodeableConcept) procedure.Value;
            dto.VaccinationProcedure = new CodeDTO
            {
                ReadCode = coding.Coding?.FirstOrDefault(x => x.System == "http://read.info/readv2")?.Code,

                Description = coding.Coding?.FirstOrDefault(x => x.System == "http://snomed.info/sct")?.Display
            };
            var code = coding.Coding?.FirstOrDefault(x => x.System == "http://snomed.info/sct")?.Code;
            if (code != null)
                dto.VaccinationProcedure.SnomedCode = code;
        }

        return dto;
    }
}

