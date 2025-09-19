using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.Create;

public class CreateClinicProfileCommand : IRequest<Result<Clinic>>
{
	public string Name { get; set; } = string.Empty;
	public Guid UserId { get; set; } = Guid.Empty;
}