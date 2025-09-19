using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using MediFlow.ClinicManagement.UseCases.Doctors.Commands;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.DoctorManagement.UseCases.Commands;

internal class UpdateDoctorProfileCommandHandler(UsersManagementDbContext context)
	: IRequestHandler<UpdateDoctorProfileCommand, Result<Doctor>>
{
	private readonly UsersManagementDbContext _context = context;

	public async Task<Result<Doctor>> Handle(
		UpdateDoctorProfileCommand request, CancellationToken cancellationToken)
	{
		var doctor = await _context.Doctors
						.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (doctor is null)
			return Result.NotFound();

		if(doctor.UserId != request.UserId)
			return Result.Forbidden();

		var updateNameResult = doctor.UpdateName(request.Name);

		if (!updateNameResult.Errors.Any())
		{
			_context.Entry(doctor).State = EntityState.Modified;

			await _context.SaveChangesAsync(cancellationToken);
		}

		return updateNameResult;
	}
}
