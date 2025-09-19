using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Data;
using MediFlow.Schedule.Domain;
using MediFlow.Schedule.UseCases.Common;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Schedule.UseCases.SetDoctorFreeTime;

internal class SetDoctorAvailabilityCommandHandler : IRequestHandler<SetDoctorAvailabilityCommand, Result<Doctor>>
{
	private readonly ScheduleDbContext _context;

	public SetDoctorAvailabilityCommandHandler(ScheduleDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Doctor>> Handle(SetDoctorAvailabilityCommand request, CancellationToken cancellationToken)
	{
		var doctor = await _context.Doctors
						.Include(d => d.Clinics)
						.ThenInclude(c => c.Clinic)
						.FirstOrDefaultAsync(x => x.Id == request.DoctorId, cancellationToken);

		if (doctor is null)
			return Result.NotFound();

		if (doctor.UserId != request.UserId)
			return Result.Forbidden();

		var resetResult = doctor.ResetFreeTime(request.AvailabilityItems.ToAvailabilityDictionary());

		if (!resetResult.IsSuccess)
			return Result.Invalid(resetResult.ValidationErrors);

		_context.Entry(doctor).State = EntityState.Modified;
		await _context.SaveChangesAsync();

		return Result.Success(doctor);
	}
}