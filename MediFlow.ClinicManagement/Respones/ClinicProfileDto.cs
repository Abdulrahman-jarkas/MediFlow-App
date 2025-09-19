using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.Respones;


public sealed record ClinicProfileDto(Guid Id, string Name);

public class ClinicDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Guid UserId { get; set; }
	public List<DayAvailabilityDto> Availability { get; set; } = new();
}

public class DayAvailabilityDto
{
	public DayOfWeek Day { get; set; }
	public List<TimeRangeDto> TimeRanges { get; set; } = new();
}

public class TimeRangeDto
{
	public TimeOnly From { get; set; }
	public TimeOnly To { get; set; }
}




