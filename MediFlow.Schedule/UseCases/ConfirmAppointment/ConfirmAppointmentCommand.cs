using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Domain;

namespace MediFlow.Schedule.UseCases.ConfirmAppointment;

public class ConfirmAppointmentCommand : IRequest<Result<Appointment>>
{
	public Guid ClinicId { get; set; }
	public Guid AppointmentId { get; set; }
}