using Ardalis.Result;
using SharedKernal;

namespace MediFlow.Schedule.Domain;

public class Appointment : Entity
{
	public enum AppointmentStatus
	{
		Pendding,
		Confirmed
	}

	public Doctor? Doctor { get; init; }
	public Guid DoctorId { get; init; }

	public Clinic? Clinic { get; init; }
	public Guid ClinicId { get; init; }

	public Patient? Patient { get; init; }
	public Guid PatientId { get; init; }

	public DateTime DateTime { get; private set; }

	public AppointmentStatus Status { get; private set; }

	public Appointment(Guid doctorId, Guid clinicId, Guid patientId, DateTime dateTime)
	{
		DoctorId = doctorId;
		PatientId = patientId;
		ClinicId = clinicId;
		DateTime = dateTime;
		Status = AppointmentStatus.Pendding;
	}

	public Result<Appointment> Confirm()
	{
		if(Status == AppointmentStatus.Confirmed)
			return Result.Invalid(new ValidationError(nameof(Status), "the appointemnt already confirmed"));

		Status = AppointmentStatus.Confirmed;

		return Result.Success(this);
	}
}
