using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.AddEquipment;

internal class AddEquipmentCommand : IRequest<Result<Equipment>>
{
	public string Name { get; set; } = string.Empty;
	public decimal Fees { get; set; }
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}