namespace GPMigratorApp.DTOs;

public class ObservationComponentDTO
{
    public Guid Id { get; set; }
    public Guid ObservationId { get; set; }
    public CodeDTO? Code { get; set; }
    public decimal? ValueQuantity { get; set; }
    public string? ValueQuantityUnit { get; set; }
    public string? ValueCode { get; set; }
    public string? ValueString { get; set; }
    public int? ValueRangeLow  { get; set; }
    public int? ValueRangeHigh  { get; set; }
    public int? ValueRatioNumerator  { get; set; }
    public int? ValueRatioDenominator { get; set; }
    public int? SampledDataOrigin { get; set; }
    public decimal? SampledDataPeriod { get; set; }
    public decimal? SampledDataFactor { get; set; }
    public decimal? SampledDataLowerLimit { get; set; }
    public decimal? SampledDataUpperLimit { get; set; }
    public string? SampledData { get; set; }
    public DateTime? ValueTime { get; set; }
    public DateTime? ValueDateTime { get; set; }
    public DateTime? ValueDateTimeStart { get; set; }
    public DateTime? ValueDateTimeEnd { get; set; }
    public string? DataAbsentReason { get; set; }
    
}