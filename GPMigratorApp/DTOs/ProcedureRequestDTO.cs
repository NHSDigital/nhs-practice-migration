using Hl7.Fhir.Model;

namespace GPMigratorApp.DTOs;

public class ProcedureRequestDTO
{
    public Guid Id { get; set; }
    public string OriginalId { get; set; }
    public int Intent { get; set; }
    public IdentifierDTO? Identifier { get; set; }
    public string? Status { get; set; }
    public DateTime? AuthoredOn { get; set; }
    public DateTime? Occurence { get; set; }
    public string? Category{ get; set; }
    public CodeDTO? Code{ get; set; }
    public PatientDTO? Subject { get; set; }
    public OutboundRelationship? Context { get; set; }
    public EncounterDTO? Encounter { get; set; }
    public OutboundRelationship? Requester{ get; set; }
    public OrganizationDTO? OnBehalfOf { get; set; }
    public OutboundRelationship? Performer { get; set; }
    public string? Reason { get; set; }
    public OutboundRelationship? ReasonReference{ get; set; }
    public ObservationDTO? SupportingInfo { get; set; }
    public SpecimenDTO? Specimen { get; set; }
    public string? BodySite { get; set; }
    public string? NoteText { get; set; }
    public DateTime? NoteAuthored { get; set; }
    public OutboundRelationship? NoteAuthor{ get; set; }
    public Guid EntityId { get; set; }

}