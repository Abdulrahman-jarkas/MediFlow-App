using FluentValidation;

namespace MediFlow.ClinicManagement.UseCases.Rooms.DeleteRoom;

class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
	public DeleteRoomCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty().NotNull();
		RuleFor(c => c.ClinicId).NotEmpty().NotNull();
		RuleFor(c => c.UserId).NotEmpty().NotNull();
	}
}
