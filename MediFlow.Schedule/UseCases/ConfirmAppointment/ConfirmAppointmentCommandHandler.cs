using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Data;
using MediFlow.Schedule.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Schedule.UseCases.ConfirmAppointment;

public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, Result<Appointment>>
{
	private readonly ScheduleDbContext _context;

	public ConfirmAppointmentCommandHandler(ScheduleDbContext context)
	{
		_context = context;
	}
	public async Task<Result<Appointment>> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointemnt = await _context.Appointments
			.FirstOrDefaultAsync(res => res.Id == request.AppointmentId && res.ClinicId == request.ClinicId);

		if (appointemnt == null)
			return Result.NotFound("Appointemnt is not exist");

		var confirmRes = appointemnt.Confirm();

		if (confirmRes.ValidationErrors.Any())
			return confirmRes;

		_context.Entry(appointemnt).State = EntityState.Modified;
		await _context.SaveChangesAsync();

		return Result.Success(appointemnt);
	}
}