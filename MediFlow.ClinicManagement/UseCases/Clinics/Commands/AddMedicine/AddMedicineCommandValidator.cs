using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Medicines.AddMedicine;

class UpdateMedicineCommandValidator : AbstractValidator<AddMedicineCommand>
{
	public UpdateMedicineCommandValidator()
	{
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
		RuleFor(c => c).MustBeEntity(c => Medicine.Create(c.Name, c.Price, c.Quantity));
	}
}
