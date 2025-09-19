using MediFlow.Schedule.UseCases.Common;

namespace MediFlow.Schedule.Requests;

public class SetAvailabilityRequest
{
	public List<SingleDayAvailabilityRequest> Availabilities { get; set; } = new();
}

public class SingleDayAvailabilityRequest
{
	public DayOfWeek Day { get; set; }
	public List<TimeRangeRequest> TimeRanges { get; set; } = new();
}

public class TimeRangeRequest
{
	public TimeOnly From { get; set; }
	public TimeOnly To { get; set; }
}


public static class WeeklyAvailabilityMappings
{
	public static List<DayAvailabilityCommand> ToCommand(this List<SingleDayAvailabilityRequest> data)
	{
		return data
				.Select(dayRequest => new DayAvailabilityCommand
				{
					Day = dayRequest.Day,
					TimeRanges = dayRequest.TimeRanges
						.Select(tr => new TimeRangeCommand
						{
							From = tr.From,
							To = tr.To
						})
						.ToList()
				})
				.ToList();
	}
}