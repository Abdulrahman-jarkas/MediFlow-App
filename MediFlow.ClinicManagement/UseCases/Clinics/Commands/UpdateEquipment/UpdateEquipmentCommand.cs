using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.UpdateEquipment;

internal class UpdateEquipmentCommand : IRequest<Result<Equipment>>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Fees { get; set; }
	public Guid UserId { get; set; }
	public Guid ClinicId { get; set; }
}