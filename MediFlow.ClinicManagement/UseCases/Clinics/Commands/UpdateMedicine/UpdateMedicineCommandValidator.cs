using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Medicines.UpdateMedicine;

class UpdateEquipmentCommandValidator : AbstractValidator<UpdateMedicineCommand>
{
	public UpdateEquipmentCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty().NotNull();
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
		RuleFor(c => c).MustBeEntity(c => Medicine.Create(c.Name, c.Price, c.Quantity));
	}
}
