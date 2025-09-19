using Ardalis.Result;
using MediFlow.ClinicManagement.Domain.ValueObjects;
using SharedKernal;

namespace MediFlow.Schedule.Domain;

public class ClinicDoctor : Entity
{
	public Guid ClinicId { get; init; }
	public Clinic Clinic { get; init; }

	public Guid DoctorId { get; init; }
	public Doctor Doctor { get; init; }

	public Availability FreeTime { get; private set; }

	private ClinicDoctor(Clinic clinic, Doctor doctor, Availability availability)
	{
		Clinic = clinic;
		ClinicId = clinic.Id;

		Doctor = doctor;
		DoctorId = doctor.Id;

		FreeTime = availability;
	}

	public static Result<ClinicDoctor> Create(Clinic clinic, Doctor doctor)
	{
		var result = clinic.FreeTime.Merge(doctor.FreeTime);

		if (!result.IsSuccess)
			return Result.Invalid(result.ValidationErrors);

		return new ClinicDoctor(clinic, doctor, result.Value);
	}

	public Result<Availability> Update(Clinic clinic)
	{
		var result = clinic.FreeTime.Merge(Doctor.FreeTime);

		if (!result.IsSuccess)
			return result;

		FreeTime = result.Value;

		return Result.Success();
	}

	public Result<Availability> Update(Doctor doctor)
	{
		var result = Clinic.FreeTime.Merge(doctor.FreeTime);

		if (!result.IsSuccess)
			return result;

		FreeTime = result.Value;

		return Result.Success();
	}

	protected ClinicDoctor()
	{ }
}
