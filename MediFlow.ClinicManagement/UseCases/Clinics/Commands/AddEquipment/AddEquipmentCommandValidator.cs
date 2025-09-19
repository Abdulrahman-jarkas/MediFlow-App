using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.AddEquipment;

class AddEquipmentCommandValidator : AbstractValidator<AddEquipmentCommand>
{
	public AddEquipmentCommandValidator()
	{
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
		RuleFor(c => c).MustBeEntity(c => Equipment.Create(c.Name, c.Fees));
	}
}
