using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.Create;

internal sealed class CreateClinicProfileCommandValidator : AbstractValidator<CreateClinicProfileCommand>
{
	public CreateClinicProfileCommandValidator()
	{
		RuleFor(c => c).MustBeEntity(c => Clinic.Create(c.UserId, c.Name));
	}
}

