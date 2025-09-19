using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Rooms.AddRoom;

internal class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Result<Room>>
{
	private readonly UsersManagementDbContext _context;

	public AddRoomCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Room>> Handle(AddRoomCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context.Clinics
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var roomResult = Room.Create(request.Name);


		if (!roomResult.Errors.Any())
		{
			clinic.AddRoom(roomResult.Value);

			_context.Entry(roomResult.Value).State = EntityState.Added;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return roomResult;
	}
}
