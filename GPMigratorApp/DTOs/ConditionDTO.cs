using Hl7.Fhir.Model;

namespace GPMigratorApp.DTOs;

public class ConditionDTO
{
    public Guid Id { get; set; }
    
    public string? OriginalId { get; set; }
    public IdentifierDTO? Identifier { get; set; }
    public OutboundRelationship ActualProblem { get; set; }
    public IEnumerable<OutboundRelationship>? RelatedProblem { get; set; }
    public string? ProblemSignificance { get; set; }
    
    public string? Category { get; set; }
    public IEnumerable<OutboundRelationship>? RelatedClinicalConditions { get; set; }
    public string? Episode { get; set; }
    public string? ClinicalStatus { get; set; }
    public string? VerificationStatus { get; set; }
    public string? Severity { get; set; }
    public CodeDTO? Code { get; set; }
    public string? BodySite { get; set; }
    public PatientDTO? Subject { get; set; }
    public EncounterDTO? Context { get; set; }
    public DateTime? OnsetDate { get; set; }
    public decimal? OnsetAge { get; set; }
    public DateTime? OnsetPeriodStart { get; set; }
    public DateTime? OnsetPeriodEnd { get; set; }
    public DateTime? AbatementDate { get; set; }
    public decimal? AbatementAge { get; set; }
    public DateTime? AbatementPeriodStart { get; set; }
    public DateTime? AbatementPeriodEnd { get; set; }
    public bool? Abatement { get; set; }
    public DateTime? AssertedDate{ get; set; }
    public PracticionerDTO? Asserter { get; set; }
    public IEnumerable<OutboundRelationship>? Evidence { get; set; }
    public string? NoteText { get; set; }
    public DateTime? NoteAuthored { get; set; }
    public OutboundRelationship? NoteAuthor { get; set; }
    public Guid EntityId { get; set; }


}