using Ardalis.Result;

namespace MediFlow.ClinicManagement.Domain.ValueObjects;

public class TimeRange
{
	public TimeOnly From { get; private set; }
	public TimeOnly To { get; private set; }

	public TimeRange(TimeOnly from, TimeOnly to)
	{
		From = from;
		To = to;
	}

	public static Result<TimeRange> Create(TimeOnly from, TimeOnly to)
	{
		if (from == to)
		{
			return Result.Invalid(new ValidationError(
				nameof(to),
				$"'{nameof(to)}: {to}' should not be equal to '{nameof(from)}: {from}'"
			));
		}

		if (from > to)
		{
			return Result.Invalid(new ValidationError(
				nameof(from),
				$"'{nameof(from)}: {from}' should not be after '{nameof(to)}': {to}"
			));
		}

		return Result.Success(new TimeRange(from, to));
	}

	public bool ConflictsWith(TimeRange other)
	{
		// Overlap: A < B.End && B < A.End
		return From < other.To && other.From < To;
	}
}
