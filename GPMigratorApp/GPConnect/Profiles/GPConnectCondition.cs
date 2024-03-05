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
[FhirType("Encounter","http://hl7.org/fhir/StructureDefinition/Encounter", IsResource=true)]
public class GPConnectCondition
{
    private Condition _encounter;
    private EpisodeOfCare? _episodeOfCare;
    private IEnumerable<PracticionerDTO>? _practicioners;
    private IEnumerable<PatientDTO>? _patients;
    public GPConnectCondition(Condition encounter, FhirResponse bundle)
    {
        _encounter = encounter;
        _practicioners = bundle.Practitioners;
        _patients = bundle.Patients;
    }
    

    
}