using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using MediFlow.ClinicManagement.UseCases.Clinics.Commands.Create;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Medicines.AddMedicine;

internal class AddEquipmentCommandHandler : IRequestHandler<AddMedicineCommand, Result<Medicine>>
{
	private readonly UsersManagementDbContext _context;

	public AddEquipmentCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Medicine>> Handle(AddMedicineCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context.Clinics
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var medicineResult = Medicine.Create(request.Name, request.Price, request.Quantity);


		if (!medicineResult.Errors.Any())
		{
			clinic.AddMedicine(medicineResult.Value);

			Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

			_context.Entry(medicineResult.Value).State = EntityState.Added;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return medicineResult;
	}
}
