using MediFlow.ClinicManagement.Domain.Entities;

namespace MediFlow.UsersManagement.Respones;

public class ClinicDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public List<DoctorDto> Doctors { get; set; } = new();
}

public class DoctorDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
}

internal static class ClinicMappings
{
	public static ClinicDto ToDto(this Clinic clinic)
	{
		return new ClinicDto
		{
			Id = clinic.Id,
			Name = clinic.Name,
			Doctors = clinic.Doctors?
				.Select(d => d.ToDto())
				.ToList() ?? new()
		};
	}

	public static DoctorDto ToDto(this Doctor doctor)
	{
		return new DoctorDto
		{
			Id = doctor.Id,
			Name = doctor.Name
		};
	}
}