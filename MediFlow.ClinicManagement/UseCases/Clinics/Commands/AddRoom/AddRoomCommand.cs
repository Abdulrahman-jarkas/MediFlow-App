using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Rooms.AddRoom;

internal class AddRoomCommand : IRequest<Result<Room>>
{
	public string Name { get; set; } = string.Empty;
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}