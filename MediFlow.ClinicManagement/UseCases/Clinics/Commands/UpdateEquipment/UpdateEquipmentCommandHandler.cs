using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.UpdateEquipment;

internal class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, Result<Equipment>>
{
	private readonly UsersManagementDbContext _context;

	public UpdateEquipmentCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Equipment>> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
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

		var updateEquipmentResult = equipment.Update(request.Name, request.Fees);

		if (!updateEquipmentResult.Errors.Any())
		{
			_context.Entry(equipment).State = EntityState.Modified;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return updateEquipmentResult;
	}
}