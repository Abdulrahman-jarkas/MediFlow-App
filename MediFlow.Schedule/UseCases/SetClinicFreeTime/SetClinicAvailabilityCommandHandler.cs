using Ardalis.Result;
using MediatR;
using MediFlow.Schedule.Data;
using MediFlow.Schedule.Domain;
using MediFlow.Schedule.UseCases.Common;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Schedule.UseCases.SetClinicFreeTime;

internal class SetClinicAvailabilityCommandHandler : IRequestHandler<SetClinicAvailabilityCommand, Result<Clinic>>
{
	private readonly ScheduleDbContext _context;

	public SetClinicAvailabilityCommandHandler(ScheduleDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Clinic>> Handle(SetClinicAvailabilityCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context.Clinics
						.Include(c => c.Doctors)
						.ThenInclude(d => d.Doctor)
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var resetResult = clinic.ResetFreeTime(request.AvailabilityItems.ToAvailabilityDictionary());

		if (!resetResult.IsSuccess)
			return Result.Invalid(resetResult.ValidationErrors);

		_context.Entry(clinic).State = EntityState.Modified;
		await _context.SaveChangesAsync();

		return Result.Success(clinic);
	}
}