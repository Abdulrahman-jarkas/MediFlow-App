namespace MediFlow.DoctorManagement.Requests;

public class UpdateDoctorProfileRequest
{
	public string Name { get; init; } = string.Empty;
	public Guid Id { get; set; }
}