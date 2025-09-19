using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.UpdateClinic;

internal class UpdateClinicProfileCommandHandler(UsersManagementDbContext context)
	: IRequestHandler<UpdateClinicProfileCommand, Result<Clinic>>
{
	private readonly UsersManagementDbContext _context = context;

	public async Task<Result<Clinic>> Handle(
		UpdateClinicProfileCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context.Clinics
						.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if(clinic.UserId != request.UserId)
			return Result.Forbidden();

		var updateNameResult = clinic.UpdateName(request.Name);

		if (!updateNameResult.Errors.Any())
		{
			_context.Entry(clinic).State = EntityState.Modified;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return updateNameResult;
	}
}
