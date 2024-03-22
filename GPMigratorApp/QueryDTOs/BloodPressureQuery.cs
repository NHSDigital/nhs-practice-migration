namespace GPMigratorApp.QueryDTOs;

public class BloodPressureQuery
{
    public string NHSNumber { get; set; }
    public Guid PatientId { get; set; }
    public int Age { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int YearsSinceLastBPReading { get; set; }
    public DateTime? LastRefusedBPReading { get; set; }
}