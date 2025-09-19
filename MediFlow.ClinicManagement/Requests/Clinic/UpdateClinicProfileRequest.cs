using MediFlow.ClinicManagement.UseCases.Clinics.Commands.UpdateClinic;

namespace MediFlow.ClinicManagement.Requests.Profile;

public class UpdateClinicProfileRequest
{
	public string Name { get; init; } = string.Empty;
	public Guid Id { get; set; }
}