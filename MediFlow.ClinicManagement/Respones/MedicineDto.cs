namespace MediFlow.ClinicManagement.Respones;


	public sealed record MedicineDto(Guid Id, string Name, decimal Price, int Quantity);

