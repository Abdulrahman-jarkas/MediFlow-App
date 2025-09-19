using FluentValidation;
using MediFlow.ClinicManagement.Domain.Entities;
using SharedKernal;

namespace MediFlow.ClinicManagement.UseCases.Rooms.UpdateRoom;

class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
	public UpdateRoomCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty().NotNull();
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
		RuleFor(c => c).MustBeEntity(c => Room.Create(c.Name));
	}
}
