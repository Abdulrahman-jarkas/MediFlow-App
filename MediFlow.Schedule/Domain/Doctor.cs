using Ardalis.Result;
using MediFlow.ClinicManagement.Domain.ValueObjects;
using SharedKernal;

namespace MediFlow.Schedule.Domain;

public class Doctor : Entity
{
	public Guid UserId { get; init; }

	public Availability FreeTime { get; set; }

	private List<ClinicDoctor> _clinics = new();
	public IReadOnlyList<ClinicDoctor> Clinics => _clinics.AsReadOnly();

	public Doctor(Guid userId, Availability freeTime)
	{
		UserId = userId;
		FreeTime = freeTime;
	}

	public Result<ClinicDoctor> AddClinic(Clinic clinic)
	{
		var newClinicDoctorAvailabilty = ClinicDoctor.Create(clinic, this);

		if (newClinicDoctorAvailabilty.ValidationErrors.Any())
			return newClinicDoctorAvailabilty;

		_clinics.Add(newClinicDoctorAvailabilty);

		return Result.Success(newClinicDoctorAvailabilty);
	}

	public Result<Availability> ResetFreeTime(Dictionary<DayOfWeek, List<(TimeOnly From, TimeOnly To)>> data)
	{
		var resetResult = FreeTime.Reset(data);

		if (!resetResult.IsSuccess)
			return Result.Invalid(resetResult.ValidationErrors);

		var recalculateResult = RecalculateAvailabilties();

		if (!recalculateResult.IsSuccess)
			return Result.Invalid(recalculateResult.ValidationErrors);

		return Result.Success(FreeTime);
	}

	private Result RecalculateAvailabilties()
	{
		foreach (var clinic in _clinics)
		{
			var updateClinicAndDoctorAvailabiltyResult = clinic.Update(this);

			if (!updateClinicAndDoctorAvailabiltyResult.IsSuccess)
				return Result.Invalid(updateClinicAndDoctorAvailabiltyResult.ValidationErrors);
		}

		return Result.Success();
	}

	private Doctor()
	{
	}
}
