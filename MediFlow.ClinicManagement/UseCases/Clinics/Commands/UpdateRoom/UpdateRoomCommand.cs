using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Rooms.UpdateRoom;

internal class UpdateRoomCommand : IRequest<Result<Room>>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public Guid UserId { get; set; }
	public Guid ClinicId { get; set; }
}