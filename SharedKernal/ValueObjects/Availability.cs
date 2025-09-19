using Ardalis.Result;

namespace MediFlow.ClinicManagement.Domain.ValueObjects;

public class Availability
{
	private readonly Dictionary<DayOfWeek, List<TimeRange>> _availability;

	public Availability()
	{
		_availability = new Dictionary<DayOfWeek, List<TimeRange>>();
	}

	public IReadOnlyDictionary<DayOfWeek, List<TimeRange>> Data =>
		_availability;

	public static Result<Availability> Create(Dictionary<DayOfWeek, List<TimeRange>> value)
	{
		var newFreeTime = new Availability();

		foreach(var freeTimeForDay in value)
		{
			foreach(var tr in value[freeTimeForDay.Key])
			{
				var resultOfAddingNewTimeRange = newFreeTime.AddTimeRange(freeTimeForDay.Key, tr.From, tr.To);

				if (!resultOfAddingNewTimeRange.IsSuccess)
					return resultOfAddingNewTimeRange;
			}
		}

		return Result.Success(newFreeTime);
	}
	
	public Result AddTimeRange(DayOfWeek day, TimeOnly from, TimeOnly to)
	{
		if (!Enum.IsDefined(typeof(DayOfWeek), day))
			return Result.Invalid(new ValidationError(nameof(day), $"{day} is invalid day"));

		var newTimeRangeResult = TimeRange.Create(from, to);

		if (!newTimeRangeResult.ValidationErrors.Any())
		{
			if (!_availability.ContainsKey(day))
			{
				_availability[day] = new();
			}

			var _availabilityDay = _availability[day];
			var newTimeValue = newTimeRangeResult.Value;

			foreach (var existingRange in _availabilityDay)
			{
				if (existingRange.ConflictsWith(newTimeRangeResult))
				{
					return Result.Invalid(
						new ValidationError(
						$"({newTimeValue.From} - {newTimeValue.To}) conflicts with time ({existingRange.From} - {existingRange.To})"
					));
				}
			}

			_availabilityDay.Add(newTimeRangeResult);
			_availabilityDay.Sort((a, b) => a.From.CompareTo(b.From));

			return Result.Success();
		}

		return Result.Invalid(newTimeRangeResult.ValidationErrors);
	}

	public Result<List<TimeRange>> GetFreeTime(DayOfWeek day, List<TimeRange> timesRanges)
	{
		var freeTimeForDay = new List<TimeRange>();
		var currentDayFreeTime = _availability[day];

		foreach (var item in currentDayFreeTime)
		{
			foreach (var newItem in timesRanges)
			{
				if (item.From < newItem.To && newItem.From < item.To)
				{
					var overlapFrom = item.From > newItem.From ? item.From : newItem.From;
					var overlapTo = item.To < newItem.To ? item.To : newItem.To;

					if (overlapFrom < overlapTo)
					{
						var tr = TimeRange.Create(overlapFrom, overlapTo);
						freeTimeForDay.Add(tr);

						if (tr.ValidationErrors.Any())
							return Result.Invalid(new ValidationError($"Invalid Date Range (From: {overlapFrom}, To: {overlapTo})"));
					}
				}
			}
		}

		return Result.Success(freeTimeForDay);
	}

	public Result<Availability> Merge(Availability newAvailability)
	{
		var result = new Availability();
		var days = _availability.Keys;

		foreach (var day in days)
		{
			if (!newAvailability.Data.TryGetValue(day, out var newDayFreeTime))
				continue;

			var freetimeForDayResult = GetFreeTime(day, newDayFreeTime);

			if (!freetimeForDayResult.IsSuccess)
			{
				return Result.Invalid(freetimeForDayResult.ValidationErrors);
			}

			foreach (var newTimeRange in freetimeForDayResult.Value)
			{
				var validationOfAddNewTimeRange = result.AddTimeRange(day, newTimeRange.From, newTimeRange.To);

				if (validationOfAddNewTimeRange.ValidationErrors.Any())
					return validationOfAddNewTimeRange;
			}

		}

		return Result.Success(result);
	}

	public static Availability CloneAvailabilty(Availability origin) => InitAvailabilty(origin.Data);

	public static Availability InitAvailabilty(IReadOnlyDictionary<DayOfWeek, List<TimeRange>> data)
	{
		var availability = new Availability();

		foreach (var item in data)
		{
			foreach (var timeRange in item.Value)
			{
				var res = availability.AddTimeRange(item.Key, timeRange.From, timeRange.To);

				if (res.ValidationErrors.Any())
					throw new InvalidDataException($"Invalid data: {string.Join(',', res.ValidationErrors.Select(e => e.ErrorMessage))}");
			}
		}

		return availability!;
	}

	public Result<Availability> Reset(Dictionary<DayOfWeek, List<(TimeOnly From, TimeOnly To)>> data)
	{
		_availability.Clear();

		foreach (var freetimeForDay in data)
		{
			foreach (var tr in freetimeForDay.Value)
			{
				var resultOfAddingNewTime = AddTimeRange(freetimeForDay.Key, tr.From, tr.To);

				if (!resultOfAddingNewTime.IsSuccess)
					return Result.Invalid(resultOfAddingNewTime.ValidationErrors);
			}
		}

		return Result.Success(this);
	}

	public bool IsFreeAt(DateTime dateTime)
	{
		var day = dateTime.DayOfWeek;
		var time = TimeOnly.FromDateTime(dateTime);

		if (!_availability.TryGetValue(day, out var timeRanges))
			return false;

		return timeRanges.Any(range => time >= range.From && time < range.To);
	}


	public bool Clear(DayOfWeek dayOfWeek)
	{
		return _availability.Remove(dayOfWeek);
	}
}
