using FluentValidation;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.UpdateClinic;

internal class UpdateClinicProfileCommandValidator : AbstractValidator<UpdateClinicProfileCommand>
{
	public UpdateClinicProfileCommandValidator()
	{
		RuleFor(p => p.Id)
			.NotEmpty()
			.NotNull();

		RuleFor(p => p.Name)
			.NotEmpty()
			.NotNull();
	}
}