using GPMigratorApp.DTOs;

namespace GPMigratorApp.Models;

public class OrganizationViewModel
{
    public IEnumerable<OrganizationDTO> Organization { get; set; }
    public PaginationViewModel Pagination { get; set; }
    
    public Guid OrganizationId {get; set;}
}