using FluentValidation;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.DeleteEquipment;

class DeleteEquipmentCommandValidator : AbstractValidator<DeleteEquipmentCommand>
{
	public DeleteEquipmentCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty().NotNull();
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
	}
}
