using Ardalis.Result;
using DoctorManagement.Domain.Entities;
using MediatR;

namespace MediFlow.DoctorManagement.UseCases.Commands;

public class UpdateDoctorProfileCommand : IRequest<Result<Doctor>>
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Name { get; set; } = string.Empty;
}
