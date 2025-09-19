using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Domain;

namespace MediFlow.Schedule.UseCases.Common;

public class DayAvailabilityCommand
{
	public DayOfWeek Day { get; set; }
	public List<TimeRangeCommand> TimeRanges { get; set; } = new();
}

public class TimeRangeCommand
{
	public TimeOnly From { get; set; }
	public TimeOnly To { get; set; }
}

public class SetClinicAvailabilityCommand : IRequest<Result<Clinic>>
{
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
	public List<DayAvailabilityCommand> AvailabilityItems { get; set; } = new();
}

public class SetDoctorAvailabilityCommand : IRequest<Result<Doctor>>
{
	public Guid DoctorId { get; set; }
	public Guid UserId { get; set; }
	public List<DayAvailabilityCommand> AvailabilityItems { get; set; } = new();
}