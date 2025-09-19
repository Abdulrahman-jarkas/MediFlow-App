using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Domain;

namespace MediFlow.Schedule.UseCases.CreateAppointment;

public class CreateAppointmentCommand : IRequest<Result<Appointment>>
{
	public Guid UserId { get; set; }
	public Guid PatientId { get; set; }
	public Guid ClinicId { get; set; }
	public Guid DoctorId { get; set; }
	public DateTime DateTime { get; set; }
}