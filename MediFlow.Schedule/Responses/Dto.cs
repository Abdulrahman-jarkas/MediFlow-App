namespace MediFlow.Schedule.Responses;

public class DoctorDto
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public AvailabilityDto FreeTime { get; set; } = new();
	public List<Guid> ClinicIds { get; set; } = new();
}


public class AvailabilityDto
{
	public Dictionary<DayOfWeek, List<TimeRangeDto>> Data { get; set; } = new();
}

public class TimeRangeDto
{
	public TimeOnly From { get; set; }
	public TimeOnly To { get; set; }
}

public class ClinicDto
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public AvailabilityDto FreeTime { get; set; } = new();
	public List<Guid> DoctorsIds { get; set; } = new();
}
