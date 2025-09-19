using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Doctors.Commands;

public class CreateDoctorProfileCommand : IRequest<Result<Doctor>>
{
	public string Name { get; set; } = string.Empty;
	public Guid UserId { get; set; } = Guid.Empty;
}