using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;
using MediFlow.ClinicManagement.UseCases.Medicines.AddMedicine;

namespace MediFlow.ClinicManagement.UseCases.Medicines.UpdateMedicine;

internal class UpdateMedicineCommand : IRequest<Result<Medicine>>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public Guid UserId { get; set; }
	public Guid ClinicId { get; set; }
}
