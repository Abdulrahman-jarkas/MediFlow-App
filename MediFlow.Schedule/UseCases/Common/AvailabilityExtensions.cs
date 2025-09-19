namespace MediFlow.Schedule.UseCases.Common;

public static class AvailabilityExtensions
{
	public static Dictionary<DayOfWeek, List<(TimeOnly From, TimeOnly To)>> ToAvailabilityDictionary(
		this List<DayAvailabilityCommand> availabilityItems)
	{
		return availabilityItems
			.GroupBy(x => x.Day)
			.ToDictionary(
				g => g.Key,
				g => g.SelectMany(x => x.TimeRanges)
					  .Select(tr => (tr.From, tr.To))
					  .ToList()
			);
	}
}