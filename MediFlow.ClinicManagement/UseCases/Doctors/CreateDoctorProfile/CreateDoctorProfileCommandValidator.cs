using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Doctors.Commands;

internal sealed class CreateDoctorProfileCommandValidator : AbstractValidator<CreateDoctorProfileCommand>
{
	public CreateDoctorProfileCommandValidator()
	{
		RuleFor(c => c).MustBeEntity(c => Doctor.Create(c.UserId, c.Name));
	}
}

