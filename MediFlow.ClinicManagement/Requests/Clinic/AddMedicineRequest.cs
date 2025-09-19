namespace MediFlow.UsersManagement.Requests.Clinic;

internal class AddMedicineRequest
{
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
}