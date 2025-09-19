namespace MediFlow.DoctorManagement.Respones;

public sealed record DoctorProfileDto(Guid Id, string Name);

public class DoctorDto
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