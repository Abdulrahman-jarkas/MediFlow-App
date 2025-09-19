using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.DeleteEquipment;

internal class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand, Result>
{
	private readonly UsersManagementDbContext _context;

	public DeleteEquipmentCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context
						.Clinics
						.Include(c => c.Equipments.Where(m => m.Id == request.Id))
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var equipment = clinic.GetEquipment(request.Id);

		if (equipment == null)
			return Result.NotFound("Equipment is not exist");

		_context.Entry(equipment).State = EntityState.Deleted;

		await _context.SaveChangesAsync(cancellationToken);


		return Result.Success();
	}
}