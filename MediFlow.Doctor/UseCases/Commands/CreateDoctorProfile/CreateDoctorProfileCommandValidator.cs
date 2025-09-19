using DoctorManagement.Domain.Entities;
using FluentValidation;
using MediFlow.DoctorManagement.UseCases.Commands;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.Create;

internal sealed class CreateDoctorProfileCommandValidator : AbstractValidator<CreateDoctorProfileCommand>
{
	public CreateDoctorProfileCommandValidator()
	{
		RuleFor(c => c).MustBeEntity(c => Doctor.Create(c.UserId, c.Name));
	}
}

