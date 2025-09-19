using Ardalis.Result;
using DoctorManagement.Domain.Entities;
using MediatR;

namespace MediFlow.DoctorManagement.UseCases.Commands;

public class CreateDoctorProfileCommand : IRequest<Result<Doctor>>
{
	public string Name { get; set; } = string.Empty;
	public Guid UserId { get; set; } = Guid.Empty;
}