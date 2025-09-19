using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Medicines.DeleteMedicine;

internal class DeleteEquipmentCommandHandler : IRequestHandler<DeleteMedicineCommand, Result>
{
	private readonly UsersManagementDbContext _context;

	public DeleteEquipmentCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context
						.Clinics
						.Include(c => c.Medicines.Where(m => m.Id == request.Id))
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var medicine = clinic.GetMedicine(request.Id);

		if (medicine == null)
			return Result.NotFound("Medicine is not exist");

		_context.Entry(medicine).State = EntityState.Deleted;

		await _context.SaveChangesAsync(cancellationToken);


		return Result.Success();
	}
}