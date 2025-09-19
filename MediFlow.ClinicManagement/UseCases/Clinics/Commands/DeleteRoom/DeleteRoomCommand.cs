using Ardalis.Result;
using MediatR;

namespace MediFlow.ClinicManagement.UseCases.Rooms.DeleteRoom;

internal class DeleteRoomCommand : IRequest<Result>
{
	public Guid Id { get; set; }
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}