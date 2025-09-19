using Ardalis.Result;
using MediatR;

namespace MediFlow.ClinicManagement.UseCases.Medicines.DeleteMedicine;

internal class DeleteMedicineCommand : IRequest<Result>
{
	public Guid Id { get; set; }
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}