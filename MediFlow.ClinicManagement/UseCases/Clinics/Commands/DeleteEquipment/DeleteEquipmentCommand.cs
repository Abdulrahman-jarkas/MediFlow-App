using Ardalis.Result;
using MediatR;

namespace MediFlow.UsersManagement.UseCases.Clinics.Commands.DeleteEquipment;

internal class DeleteEquipmentCommand : IRequest<Result>
{
	public Guid Id { get; set; }
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}