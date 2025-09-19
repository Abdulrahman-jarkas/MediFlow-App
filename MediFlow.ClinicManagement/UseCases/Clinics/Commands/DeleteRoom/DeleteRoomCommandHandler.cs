using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Rooms.DeleteRoom;

internal class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Result>
{
	private readonly UsersManagementDbContext _context;

	public DeleteRoomCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
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

		_context.Entry(room).State = EntityState.Deleted;

		await _context.SaveChangesAsync(cancellationToken);


		return Result.Success();
	}
}