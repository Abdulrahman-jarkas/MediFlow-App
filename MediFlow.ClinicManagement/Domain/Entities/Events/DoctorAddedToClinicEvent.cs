using SharedKernel;

namespace MediFlow.ClinicManagement.Domain.Entities.Events;

internal class DoctorAddedToClinicEvent : DomainEventBase
{
	public Clinic Clinic { get; init; }
	public Doctor Doctor { get; init; }

	public DoctorAddedToClinicEvent(Clinic clinic, Doctor doctor)
	{
		Clinic = clinic;
		Doctor = doctor;
	}
}
