using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.AddEquipment;

internal class AddEquipmentCommandHandler : IRequestHandler<AddEquipmentCommand, Result<Equipment>>
{
	private readonly UsersManagementDbContext _context;

	public AddEquipmentCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Equipment>> Handle(AddEquipmentCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context.Clinics
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var equipmentResult = Equipment.Create(request.Name, request.Fees);


		if (!equipmentResult.Errors.Any())
		{
			clinic.AddEquipment(equipmentResult.Value);

			_context.Entry(equipmentResult.Value).State = EntityState.Added;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return equipmentResult;
	}
}
