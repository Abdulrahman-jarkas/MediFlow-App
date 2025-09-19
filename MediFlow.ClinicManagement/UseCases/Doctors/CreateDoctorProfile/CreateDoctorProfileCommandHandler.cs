using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Data;
using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.ClinicManagement.UseCases.Doctors.Commands;

internal class CreateDoctorProfileCommandHandler : IRequestHandler<CreateDoctorProfileCommand, Result<Doctor>>
{
	private readonly UsersManagementDbContext _context;

	public CreateDoctorProfileCommandHandler(UsersManagementDbContext doctorDbContext)
	{
		_context = doctorDbContext;
	}

	public async Task<Result<Doctor>> Handle(CreateDoctorProfileCommand request, CancellationToken cancellationToken)
	{
		//var clinic = await _context.Clinics
		//				.FirstOrDefaultAsync(x => x.Id == request.C, cancellationToken);

		Result<Doctor> doctor = Doctor.Create(request.UserId, request.Name);

		if (!doctor.Errors.Any())
		{
			await _context.Doctors.AddAsync(doctor, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}

		return doctor;
	}
}
