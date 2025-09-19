using FluentValidation;

namespace MediFlow.DoctorManagement.UseCases.Commands;

internal class UpdateDoctorProfileCommandValidator : AbstractValidator<UpdateDoctorProfileCommand>
{
	public UpdateDoctorProfileCommandValidator()
	{
		RuleFor(p => p.Id)
			.NotEmpty()
			.NotNull();

		RuleFor(p => p.Name)
			.NotEmpty()
			.NotNull();
	}
}