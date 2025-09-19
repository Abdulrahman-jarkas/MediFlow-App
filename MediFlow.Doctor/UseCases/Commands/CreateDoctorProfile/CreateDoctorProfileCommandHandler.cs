using Ardalis.Result;
using DoctorManagement.Domain.Entities;
using MediatR;
using MediFlow.DoctorManagement.Data;

namespace MediFlow.DoctorManagement.UseCases.Commands;

internal class CreateDoctorProfileCommandHandler : IRequestHandler<CreateDoctorProfileCommand, Result<Doctor>>
{
	private readonly DoctorDbContext _doctorDbContext;

	public CreateDoctorProfileCommandHandler(DoctorDbContext doctorDbContext)
	{
		_doctorDbContext = doctorDbContext;
	}

	public async Task<Result<Doctor>> Handle(CreateDoctorProfileCommand request, CancellationToken cancellationToken)
	{
		Result<Doctor> doctor = Doctor.Create(request.UserId, request.Name);

		if (!doctor.Errors.Any())
		{
			await _doctorDbContext.Doctors.AddAsync(doctor, cancellationToken);
			await _doctorDbContext.SaveChangesAsync(cancellationToken);
		}

		return doctor;
	}
}
