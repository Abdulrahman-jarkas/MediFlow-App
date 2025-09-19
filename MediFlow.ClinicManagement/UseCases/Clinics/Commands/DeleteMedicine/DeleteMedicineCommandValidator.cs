using FluentValidation;

namespace MediFlow.ClinicManagement.UseCases.Medicines.DeleteMedicine;

class DeleteEquipmentCommandValidator : AbstractValidator<DeleteMedicineCommand>
{
	public DeleteEquipmentCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty().NotNull();
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
	}
}
