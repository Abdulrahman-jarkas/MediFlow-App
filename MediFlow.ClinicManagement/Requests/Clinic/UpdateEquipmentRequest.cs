namespace MediFlow.UsersManagement.Requests.Clinic;

internal class UpdateEquipmentRequest
{
	public string Name { get; set; } = string.Empty;
	public decimal Fees { get; set; }
}