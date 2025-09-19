using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Doctors.Commands;

public class UpdateDoctorProfileCommand : IRequest<Result<Doctor>>
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Name { get; set; } = string.Empty;
}
