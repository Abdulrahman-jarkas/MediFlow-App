using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.UpdateEquipment;

class UpdateEquipmentCommandValidator : AbstractValidator<UpdateEquipmentCommand>
{
	public UpdateEquipmentCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty().NotNull();
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
		RuleFor(c => c).MustBeEntity(c => Equipment.Create(c.Name, c.Fees));
	}
}
