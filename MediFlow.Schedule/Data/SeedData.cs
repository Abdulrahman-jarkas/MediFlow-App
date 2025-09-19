using MediFlow.ClinicManagement.Domain.ValueObjects;
using MediFlow.Schedule.Domain;

namespace MediFlow.Schedule.Data;

public static class SeedData
{

	public static async Task SeedInitialDataAsync(ScheduleDbContext context)
	{
		if (context.Clinics.Any() || context.Doctors.Any())
			return;

		// Generate time ranges
		var morning = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(12, 0)).Value;
		var afternoon = TimeRange.Create(new TimeOnly(13, 0), new TimeOnly(16, 0)).Value;
		var evening = TimeRange.Create(new TimeOnly(17, 0), new TimeOnly(20, 0)).Value;

		var tr1 = TimeRange.Create(new TimeOnly(1, 0), new TimeOnly(3, 0)).Value;
		var tr2 = TimeRange.Create(new TimeOnly(2, 0), new TimeOnly(5, 0)).Value;
		var tr3 = TimeRange.Create(new TimeOnly(8, 0), new TimeOnly(12, 0)).Value;
		var tr4 = TimeRange.Create(new TimeOnly(11, 0), new TimeOnly(16, 0)).Value;
		var tr5 = TimeRange.Create(new TimeOnly(20, 0), new TimeOnly(23, 0)).Value;



		var weekAvailability1 = Availability.InitAvailabilty(new Dictionary<DayOfWeek, List<TimeRange>>
		{
			[DayOfWeek.Monday] = new() { tr1, tr3 },
		});

		var weekAvailability2 = Availability.InitAvailabilty(new Dictionary<DayOfWeek, List<TimeRange>>
		{
			[DayOfWeek.Monday] = new() { tr2, tr4 },
		});

		// Create clinics
		var clinic1 = new Clinic(Guid.NewGuid(), weekAvailability1);
		var clinic2 = new Clinic(Guid.NewGuid(), weekAvailability2);

		// Create doctors
		var doctor1 = new Doctor(Guid.NewGuid(), weekAvailability2);
		var doctor2 = new Doctor(Guid.NewGuid(), weekAvailability1);
		var doctor3 = new Doctor(Guid.NewGuid(), weekAvailability2);

		// Connect them
		var clinicDoctor1 = ClinicDoctor.Create(clinic1, doctor1).Value;
		var clinicDoctor2 = ClinicDoctor.Create(clinic1, doctor2).Value;
		var clinicDoctor3 = ClinicDoctor.Create(clinic2, doctor2).Value;
		var clinicDoctor4 = ClinicDoctor.Create(clinic2, doctor3).Value;

		// Add to context
		context.Clinics.AddRange(clinic1, clinic2);
		context.Doctors.AddRange(doctor1, doctor2, doctor3);
		context.Set<ClinicDoctor>().AddRange(clinicDoctor1, clinicDoctor2, clinicDoctor3, clinicDoctor4);

		await context.SaveChangesAsync();
	}

}