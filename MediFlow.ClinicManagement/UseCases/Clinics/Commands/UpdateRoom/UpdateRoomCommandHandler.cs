using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Rooms.UpdateRoom;

internal class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Result<Room>>
{
	private readonly UsersManagementDbContext _context;

	public UpdateRoomCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Room>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context
						.Clinics
						.Include(c => c.Rooms.Where(m => m.Id == request.Id))
						.FirstOrDefaultAsync(x => x.Id == request.ClinicId, cancellationToken);

		if (clinic is null)
			return Result.NotFound();

		if (clinic.UserId != request.UserId)
			return Result.Forbidden();

		var room = clinic.GetRoom(request.Id);

		if (room == null)
			return Result.NotFound("Room is not exist");

		var result = room.Update(request.Name);

		if (!result.Errors.Any())
		{
			_context.Entry(room).State = EntityState.Modified;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return result;
	}
}