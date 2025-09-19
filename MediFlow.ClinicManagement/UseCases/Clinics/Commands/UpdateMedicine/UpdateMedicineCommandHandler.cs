using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Medicines.UpdateMedicine;

internal class DeleteMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand, Result<Medicine>>
{
	private readonly UsersManagementDbContext _context;

	public DeleteMedicineCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Medicine>> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
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

		var updateMedicineResult = medicine.Update(request.Name, request.Price, request.Quantity);

		if (!updateMedicineResult.Errors.Any())
		{
			_context.Entry(medicine).State = EntityState.Modified;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return updateMedicineResult;
	}
}