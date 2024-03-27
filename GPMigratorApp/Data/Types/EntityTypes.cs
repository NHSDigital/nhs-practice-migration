namespace GPMigratorApp.Data.Types;

public enum EntityTypes
{
    Organization = 1,
    Location = 2,
    Patient = 3,
    Practitioner = 4,
    Observation = 5,
    Condition = 6,
    DiaryEntry = 7,
    ProcedureRequest = 8,
    Immunization = 9
}

public static class EntityTypeHelper
{

    public static EntityTypes? GetEnumForType(string? type)
    {
        return type switch
        {
            "Organization" => EntityTypes.Organization,
            "Location" => EntityTypes.Location,
            "Patient" => EntityTypes.Patient,
            "Practitioner" => EntityTypes.Practitioner,
            "Observation" => EntityTypes.Observation,
            "Condition" => EntityTypes.Condition,
            "DiaryEntry" => EntityTypes.DiaryEntry,
            "ProcedureRequest" => EntityTypes.ProcedureRequest,
            "Immunization" => EntityTypes.Immunization,
            _ => null
        };
    }
}