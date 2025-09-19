using Ardalis.Result;
using MediFlow.ClinicManagement.Domain.ValueObjects;
using MediFlow.Schedule.Domain.ValueObjects;
using SharedKernal;

namespace MediFlow.Schedule.Domain;

public class Clinic : Entity
{
	public Guid UserId { get; init; }
	public Availability FreeTime { get; private set; } = new();

	private List<ClinicDoctor> _doctors = new();
	public IReadOnlyList<ClinicDoctor> Doctors => _doctors.AsReadOnly();

	public Calender Calendar { get; private set; }

	public Clinic(Guid userId, Availability freeTime)
	{
		UserId = userId;
		FreeTime = freeTime;
		Calendar = Calender.Create(new()).Value;
	}

	public Result<ClinicDoctor> AddDoctor(Doctor doctor)
	{
		var newClinicDoctorAvailabilty = ClinicDoctor.Create(this, doctor);

		if (newClinicDoctorAvailabilty.ValidationErrors.Any())
			return newClinicDoctorAvailabilty;

		_doctors.Add(newClinicDoctorAvailabilty);

		return Result.Success(newClinicDoctorAvailabilty);
	}

	public Result<Calender> AddAppointemnt(Appointment appointment)
	{
		var result = Calendar.Add(appointment.ClinicId, appointment.DoctorId, appointment.PatientId, appointment.DateTime);

		if (result.ValidationErrors.Any())
			return result;

		return Result.Success(result);
	}

	public Result<Availability> ResetFreeTime(Dictionary<DayOfWeek, List<(TimeOnly From, TimeOnly To)>> data)
	{
		var resetResult = FreeTime.Reset(data);

		if (!resetResult.IsSuccess)
			return Result.Invalid(resetResult.ValidationErrors);

		var recalculateAvailabiltiesResult = RecalculateAvailabilties();

		if (!recalculateAvailabiltiesResult.IsSuccess)
			return Result.Invalid(recalculateAvailabiltiesResult.ValidationErrors);

		return Result.Success(FreeTime);
	}

	private Result RecalculateAvailabilties()
	{
		foreach (var doctor in _doctors)
		{
			var updateClinicAndDoctorAvailabiltyResult = doctor.Update(this);

			if (!updateClinicAndDoctorAvailabiltyResult.IsSuccess)
				return Result.Invalid(updateClinicAndDoctorAvailabiltyResult.ValidationErrors);
		}

		return Result.Success();
	}

	protected Clinic()
	{
	}
}
