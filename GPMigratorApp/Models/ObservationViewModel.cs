using GPMigratorApp.DTOs;

namespace GPMigratorApp.Models;

public class ObservationViewModel
{
    public IEnumerable<ObservationDTO> Observations { get; set; }
    public PaginationViewModel Pagination { get; set; }
    
    public Guid PatientId {get; set;}
}