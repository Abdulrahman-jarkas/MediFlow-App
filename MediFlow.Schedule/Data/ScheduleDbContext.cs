using MediFlow.Schedule.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediFlow.Schedule.Data;

public class ScheduleDbContext : DbContext
{
	public DbSet<Doctor> Doctors { get; set; }
	public DbSet<Clinic> Clinics { get; set; }
	public DbSet<Appointment> Appointments { get; set; }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<ClinicDoctor> ClinicsDoctors { get; set; }



	public ScheduleDbContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("Schedule");

		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	protected override void ConfigureConventions(
	  ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<decimal>()
		  .HavePrecision(18, 6);
	}

	protected ScheduleDbContext()
	{
	}
}