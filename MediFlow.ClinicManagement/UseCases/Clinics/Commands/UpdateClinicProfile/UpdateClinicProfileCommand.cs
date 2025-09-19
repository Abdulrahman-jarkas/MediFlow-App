using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Clinics.Commands.UpdateClinic;

public class UpdateClinicProfileCommand : IRequest<Result<Clinic>>
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Name { get; set; } = string.Empty;
}
