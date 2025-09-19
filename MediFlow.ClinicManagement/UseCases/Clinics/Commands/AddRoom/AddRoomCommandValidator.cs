using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Rooms.AddRoom;

class AddRoomCommandValidator : AbstractValidator<AddRoomCommand>
{
	public AddRoomCommandValidator()
	{
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
		RuleFor(c => c).MustBeEntity(c => Room.Create(c.Name));
	}
}
