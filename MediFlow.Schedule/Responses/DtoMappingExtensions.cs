using MediFlow.ClinicManagement.Domain.ValueObjects;
using MediFlow.Schedule.Domain;

namespace MediFlow.Schedule.Responses;

public static class DtoMappingExtensions
{
	public static DoctorDto ToDto(this Doctor doctor)
	{
		return new DoctorDto
		{
			Id = doctor.Id,
			UserId = doctor.UserId,
			FreeTime = doctor.FreeTime.ToDto(),
			ClinicIds = doctor.Clinics.Select(c => c.ClinicId).ToList()
		};
	}

	public static ClinicDto ToDto(this Clinic clinic)
	{
		return new ClinicDto
		{
			Id = clinic.Id,
			UserId = clinic.UserId,
			FreeTime = clinic.FreeTime.ToDto(),
			DoctorsIds = clinic.Doctors.Select(c => c.DoctorId).ToList()
		};
	}

	public static AvailabilityDto ToDto(this Availability availability)
	{
		return new AvailabilityDto
		{
			Data = availability.Data.ToDictionary(
				kvp => kvp.Key,
				kvp => kvp.Value.Select(t => new TimeRangeDto
				{
					From = t.From,
					To = t.To
				}).ToList()
			)
		};
	}
}
