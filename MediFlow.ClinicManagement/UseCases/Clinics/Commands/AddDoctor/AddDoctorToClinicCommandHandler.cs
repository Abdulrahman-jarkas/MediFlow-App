using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.AddDoctor;

internal class AddDoctorToClinicCommandHandler : IRequestHandler<AddDoctorToClinicCommand, Result<Clinic>>
{
	private readonly UsersManagementDbContext _context;

	public AddDoctorToClinicCommandHandler(UsersManagementDbContext context)
	{
		_context = context;
	}

	public async Task<Result<Clinic>> Handle(AddDoctorToClinicCommand request, CancellationToken cancellationToken)
	{
		var clinic = await _context
							.Clinics
							.Include(c => c.Doctors)
							.FirstOrDefaultAsync(c => c.Id == request.ClinicId);

		if (clinic == null)
			return Result.NotFound("clinic is not exist");

		if (clinic.UserId != request.UserId)
			return Result.Unauthorized("unable to access this clinic");

		if (clinic.GetDoctor(request.DoctorId) != null)
			return Result.Conflict("Doctor is already exist");

		var doctor = await _context.Doctors.FindAsync(request.DoctorId);

		if(doctor == null)
			return Result.NotFound("doctor is not exist");

		clinic.AddDoctor(doctor);

		_context.Entry(clinic).State = EntityState.Modified;
		await _context.SaveChangesAsync();

		return Result.Success(clinic);

	}
}