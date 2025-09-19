using Ardalis.Result;
using MediatR;
using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.ClinicManagement.UseCases.Medicines.AddMedicine;

public class AddMedicineCommand : IRequest<Result<Medicine>>
{
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public Guid ClinicId { get; set; }
	public Guid UserId { get; set; }
}